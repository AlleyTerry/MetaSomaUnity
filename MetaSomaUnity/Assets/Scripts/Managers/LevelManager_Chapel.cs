using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;
using Yarn.Unity.Example;

// TODO: MAKE HUD.ACTIVE TRUE IN THIS LEVEL !!!!!!!!!
public class LevelManager_Chapel : LevelManagerBase
{
    // LINNEAUS
    [FormerlySerializedAs("linneausAnimation")] 
    public GameObject linnaeusAnimation;
    //IMERIS
    [SerializeField] private Animator imerisAnimator;
    
    
    // INTRO DIALOGUE NODE
    public string introDialogueNode = "ChapelStart";
    
    [SerializeField] private Transform defaultSpawnPoint;
    
    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        ImerisMovement.instance.currentState = new BeforeAnyEvolutionState(SubState.Healthy);
        
        GameManager.instance.isInBattle = false;
        
        // LINNEAUS
        linnaeusAnimation = base.NPCAnimation;
        //NPCAnimation = base.NPCAnimation;
        
        //NPC EYES
        
        // Spawn point
        // Set up spawn points
        defaultSpawnPoint = GameObject.Find("SpawnPoint").transform;
        
        // Set up Imeris respawn position
        ImerisMovement imerisMovement = GameObject.FindObjectOfType<ImerisMovement>();
        imerisMovement.gameObject.transform.position = defaultSpawnPoint.position;
        
        if (linnaeusAnimation != null)
        {
            DisableLinnaeusAnimation();
        }
        else
        {
            Debug.LogWarning("LinnaeusAnimation not found in scene.");
        }
        
        imerisAnimator = ImerisAnimation.GetComponent<Animator>();
    }

    public override void StartCutsScene(string cutSceneDialogueNode)
    {
        base.StartCutsScene(cutSceneDialogueNode);
        
        Imeris.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Imeris.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
        // VCAM SWITCH FOLLOWING TARGET
        StartCoroutine(DelayedVCamSwitch());
        
        if (linnaeusAnimation == null) linnaeusAnimation = GameObject.Find("NPCAnimation");
        
        linnaeusAnimation.SetActive(true); 
        imerisAnimator.Play("CoweringTransition");
        linnaeusAnimation.GetComponent<Animator>().Play("NPCEyesTransition");
    }

    private IEnumerator DelayedVCamSwitch()
    {
        CameraManager.instance.PrepSwitchFollowTarget();
        
        yield return new WaitForSeconds(0.5f);
        
        CameraManager.instance.SwitchFollowTarget();
    }
    
    [YarnCommand("IntoBattleScene")]
    public override void StartBattleScene()
    {
        base.StartBattleScene();
        
        // ANIMATION
        UIManager.instance.PlayAnimation("MediumViewportTransition");
        imerisAnimator.Play("CoweringTransitionReverse");
        linnaeusAnimation.GetComponent<Animator>().Play("NPCEyesTransitionReverse");
        //switch the npc animator to linnaeus
        RuntimeAnimatorController linnaeusAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/BattleUI/LinneausAnimation");
        linnaeusAnimation.GetComponent<Animator>().runtimeAnimatorController = linnaeusAnimatorController;
        //play the linneaus idle
        linnaeusAnimation.GetComponent<Animator>().Play("LinBattleIdle");
        
        // LINNEAUS
        //linneausAnimation.SetActive(true); // For now we don't really have the cutscene, will be commented out later

        StartCoroutine(DelayedDisableAnimator());
        StartCoroutine(DelayedInitializeHeart());
    }
    
    private IEnumerator DelayedDisableAnimator()
    {
        yield return new WaitForSeconds(0.65f);
        
        UIManager.instance.DisableAnimator();
    }
    
    private IEnumerator DelayedInitializeHeart()
    {
        yield return new WaitForSeconds(0.65f);
        
        GameManager.instance.InitializeHeart();
    }

    public override void ExitBattleDialogue()
    {
        base.ExitBattleDialogue();
        
        DialogueManager.instance.StopDialogue();
        
        // ANIMATION
        UIManager.instance.PlayAnimation("MediumViewportTransition_Reversed");
        
        DisableNPCAnimation();
        
        Debug.Log("Level 0 battle ended.");
        
        // TEMPPPPP
        Invoke(nameof(AfterExitBattleDialogue), 0.5f);
    }

    private void AfterExitBattleDialogue()
    {
        Destroy(GameObject.Find("OverworldLinnaeusDraft"));
        DialogueManager.instance.StartDialogue("Ending");
        GameManager.instance.isInBattle = true;
    }

    [YarnCommand("LevelChapelDeadScene")]
    public override void DeadScene()
    {
        base.DeadScene();

        if (DialogueManager.instance.dialogueRunner.IsDialogueRunning)
        {
            DialogueManager.instance.dialogueRunner.Dialogue.Stop();
        }

        StartCoroutine(DelayedStartDialogue("PreDeadDialogue"));
        
        Debug.Log("Level Chapel Dead Scene started.");
        
        DisableLinnaeusAnimation();
        DisableImerisAnimation();
    }

    IEnumerator DelayedStartDialogue(string dialogueNode)
    {
        yield return new WaitForEndOfFrame();
        
        DialogueManager.instance.StartDialogue(dialogueNode);
    }
    
    [YarnCommand("DisableLinnaeusAnimation")]
    public void DisableLinnaeusAnimation()
    {
        DisableNPCAnimation();
    }
}
