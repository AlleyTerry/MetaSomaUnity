using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class LevelManager_Cafeteria : LevelManagerBase
{
    [SerializeField]Animator imerisAnimator; 
    [SerializeField]Animator npcAnimator;
    
    [SerializeField] private Transform defaultSpawnPoint;

    public GameObject grubAnimation;
    
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        //set the npc animation controller on the NPCAnimation object
        npcAnimator = NPCAnimation.GetComponent<Animator>();
        RuntimeAnimatorController npcAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/NPCEyes/NPCController");
        //load the animator controller
        npcAnimator.runtimeAnimatorController = npcAnimatorController; 
        
        // Disable NPC animation display
        NPCAnimation.SetActive(false);
        
        //set the imeris animation controller on the ImerisAnimation object
        imerisAnimator = ImerisAnimation.GetComponent<Animator>();
        
        // Set up spawn points
        defaultSpawnPoint = GameObject.Find("SpawnPoint").transform;
        
        // Set up Imeris respawn position
        ImerisMovement imerisMovement = GameObject.FindObjectOfType<ImerisMovement>();
        imerisMovement.gameObject.transform.position = defaultSpawnPoint.position;
        
        // Prep for camera shifting
        CameraMidpointController midpoint = GameObject.FindObjectOfType<CameraMidpointController>();
        if (midpoint != null)
        {
            midpoint.targetA = ImerisMovement.instance.transform;
            midpoint.targetB = GameObject.Find("DeadGrub").transform;
            midpoint.xOffset = 0.25f;
        }
        else
        {
            Debug.LogWarning("CameraMidpointController not found in scene. \n Do we need this?");
        }
    }

    public override void StartCutsScene(string cutSceneDialogueNode)
    {
        Imeris.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        base.StartCutsScene(cutSceneDialogueNode);
    }
    
    [YarnCommand ("SwitchCameraToGrub")]
    public void SwitchCameraToGrub()
    {
        StartCoroutine(SwitchCamera());
        
        YarnCharacter imeris = 
            GameObject.Find("ImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        YarnCharacter innerImeris = 
            GameObject.Find("InnerImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        
        imeris.messageBubbleOffset.x += 2f;
        innerImeris.messageBubbleOffset.x += 2f;
    }

    private IEnumerator SwitchCamera()
    {
        CameraManager.instance.SwitchFollowTarget();
        
        // Wait for the camera to stop before trigger the second dialogue
        yield return new WaitForSeconds(1.75f);
        DialogueManager.instance.dialogueRunner.transform.GetComponent<YarnCharacterView>().UpdateBubblePosition();
    }
    
    [YarnCommand ("IntoGrubBattleScene")]
    public override void StartBattleScene()
    {
        base.StartBattleScene();
        
        // Animation
        UIManager.instance.PlayAnimation("MediumViewportTransition");
        
        StartCoroutine(DelayedDisableAnimator());
        StartCoroutine(DelayedInitializeHeart());
    }
    
    private IEnumerator DelayedDisableAnimator()
    {
        yield return new WaitForSeconds(0.65f);
        
        UIManager.instance.DisableAnimator();
        
        // Enable grub animation
        Invoke(nameof(EnableGrubAnimation), 1.25f);
    }
    
    private IEnumerator DelayedInitializeHeart()
    {
        yield return new WaitForSeconds(0.65f);
        
        GameManager.instance.InitializeHeart();
    }

    private void EnableGrubAnimation()
    {
        npcAnimator.runtimeAnimatorController = 
            Resources.Load("Animations/BattleUI/LinneausAnimation") as RuntimeAnimatorController;
        npcAnimator.Play("LinBattleIdle");
    }

    public override void ExitBattleDialogue()
    {
        base.ExitBattleDialogue();
        
        // viewport animation
        UIManager.instance.PlayAnimation("MediumViewportTransition_Reversed");
        
    }

    [YarnCommand ("StartCower")]
    public void PlayImerisAnimation()
    {
        NPCAnimation.SetActive(true);
        imerisAnimator.Play("CoweringTransition");
        npcAnimator.Play("NPCEyesTransition");
    }
    
    [YarnCommand ("EndCower")]
    public void EndImerisAnimation()
    {
        //set the animation to play in reverse
        imerisAnimator.Play("CoweringTransitionReverse");       
        //set the NPC animation to play in reverse
        npcAnimator.Play("NPCEyesTransitionReverse");
        //set npc animation to false after playing
        //NPCAnimation.SetActive(false);
    }
}
