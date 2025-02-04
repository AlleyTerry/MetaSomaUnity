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
    
    // INTRO DIALOGUE NODE
    public string introDialogueNode = "ChapelStart";
    
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
        
        if (linnaeusAnimation != null)
        {
            DisableLinnaeusAnimation();
        }
        else
        {
            Debug.LogWarning("LinnaeusAnimation not found in scene.");
        }
    }

    public override void StartCutsScene(string cutSceneDialogueNode)
    {
        base.StartCutsScene(cutSceneDialogueNode);

        if (linnaeusAnimation == null) linnaeusAnimation = GameObject.Find("NPCAnimation");
        
        linnaeusAnimation.SetActive(true); 
    }

    
    [YarnCommand("IntoBattleScene")]
    public override void StartBattleScene()
    {
        base.StartBattleScene();
        
        // ANIMATION
        UIManager.instance.PlayAnimation("SmallViewportTransition");
        
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
        UIManager.instance.PlayAnimation("SmallViewportTransition_Reversed");
        
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

    [YarnCommand("Level0DeadScene")]
    public override void DeadScene()
    {
        base.DeadScene();

        if (DialogueManager.instance.dialogueRunner.IsDialogueRunning)
        {
            DialogueManager.instance.dialogueRunner.Stop();
        }
        
        Debug.Log("Level 0 Dead Scene started.");
        
        DisableLinnaeusAnimation();
        DialogueManager.instance.StartDialogue("DeadDialogue");
    }
    
    [YarnCommand("DisableLinnaeusAnimation")]
    public void DisableLinnaeusAnimation()
    {
        DisableNPCAnimation();
    }
}
