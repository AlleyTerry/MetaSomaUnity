using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
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
    
    // PARTICLES
    [SerializeField] private VisualEffect heartbreakParticles;
    
    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.ResumeControls();
        NPCAnimation.GetComponent<Animator>().Play("NPCEyesTransitionReverse");
        
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
        
        // Prep for camera shifting
        CameraMidpointController midpoint = GameObject.FindObjectOfType<CameraMidpointController>();
        if (midpoint != null)
        {
            midpoint.targetA = ImerisMovement.instance.transform;
            midpoint.targetB = GameObject.Find("Linnaeus").transform;
            midpoint.xOffset = 0.00f;
        }
        else
        {
            Debug.LogWarning("CameraMidpointController not found in scene. \n Do we need this?");
        }
        
        // PARTICLES 
        // TODO: this is more like temporary, we need to find a better way to do this
        heartbreakParticles = GameObject.Find("Heartbreak").GetComponent<VisualEffect>();
    }

    public override void StartCutsScene(string cutSceneDialogueNode)
    {
        Imeris.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Imeris.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
        // VCAM SWITCH FOLLOWING TARGET
        StartCoroutine(DelayedVCamSwitch(cutSceneDialogueNode));
        
        if (linnaeusAnimation == null) linnaeusAnimation = GameObject.Find("NPCAnimation");
        
        linnaeusAnimation.SetActive(true); 
        imerisAnimator.Play("CoweringTransition");
        linnaeusAnimation.GetComponent<Animator>().Play("NPCEyesTransition");
    }

    private IEnumerator DelayedVCamSwitch(string cutSceneDialogueNode)
    {
        yield return new WaitForSeconds(0.45f);
        
        CameraManager.instance.SwitchFollowTarget();
        
        yield return new WaitForSeconds(1.65f);
        YarnCharacter innerImeris = GameObject.Find("InnerImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        innerImeris.messageBubbleOffset.x += 2f;
        base.StartCutsScene(cutSceneDialogueNode);
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
        
        // resetting camera position
        CameraManager.instance.ResetCamera();
        
        DisableNPCAnimation();
        
        Debug.Log("Level 0 battle ended.");
        
        // TEMPPPPP
        Invoke(nameof(AfterExitBattleDialogue), 0.5f);
    }

    private void AfterExitBattleDialogue()
    {
        Destroy(GameObject.Find("OverworldLinnaeusDraft"));
        DialogueManager.instance.StartDialogue("Ending");
        
        // Resume camera following
        CameraManager.instance.ResetCamera();
        
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

        //StartCoroutine(DelayedStartDialogue("PreDeadDialogue"));
        StartCoroutine(DelayedStartDialogue("DeadDialogue"));
        
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
    
    [YarnCommand("PlayHeartbreakParticles")]
    public void PlayHeartbreakParticles()
    {
        heartbreakParticles.SendEvent("StartHeartbreak");
    }
    
    

    
    [YarnCommand("LinnAttack")]
    public void LinnAttack()
    {
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/EndBattleAnimations/EndBattleCGS");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        GameManager.instance.CGDisplay.SetActive(true);
        CGDisplayAnimator.Play("LinnAttack");
    }
    
    [YarnCommand("DontEatLinn")]
    public void DontEatLinn()
    {
        //play dontEatLinn animation
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/EndBattleAnimations/EndBattleCGS");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        GameManager.instance.CGDisplay.SetActive(true);
        CGDisplayAnimator.Play("DontEatLinn");
        
    }


    [YarnCommand("SecondBreadFrame")]
    public void SecondBreadFrame()
    {
        CGDisplayAnimator.Play("DontEatLinn2");
    }
    
    [YarnCommand("ThirdBreadFrame")]
    public void ThirdBreadFrame()
    {
        CGDisplayAnimator.Play("DontEatLinn3");
    }
    
    [YarnCommand("FourthBreadFrame")]
    public void FourthBreadFrame()
    {
        CGDisplayAnimator.Play("DontEatLinn4");
    }
    
    //end battle cgs
    [YarnCommand("EatLinn")]
    public void EatLinn()
    {
        //play eatLinn animation
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/EndBattleAnimations/EndBattleCGS");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        GameManager.instance.CGDisplay.SetActive(true);
        CGDisplayAnimator.Play("EatLinn");
    }
    
    [YarnCommand("EatLinn2")]
    public void EatLinn2()
    {
        CGDisplayAnimator.Play("EatLinn2");
    }
    
    [YarnCommand("EatLinn3")]
    public void EatLinn3()
        {
            CGDisplayAnimator.Play("EatLinn3");
        }
}
