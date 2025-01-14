using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;
using Yarn.Unity.Example;

// TODO: MAKE HUD.ACTIVE TRUE IN THIS LEVEL !!!!!!!!!
public class LevelManager_0 : LevelManagerBase
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
        
        Debug.Log("LevelManager_0.Initialize started.");
        
        GameManager.instance.HUD.SetActive(true);
        
        ImerisMovement.instance.currentState = new BeforeAnyEvolutionState(SubState.Healthy);
        
        // INIT
        /*cutSceneDialogueNode = "STARTBattle1Dialogue";
        battleDialogueNode = "Battle1Dialogue";*/
        
        /*// TRANSITION ANIMATION
        if (viewportAnimator == null && GameManager.instance.HUD != null)
        {
            viewportAnimator = GameManager.instance.HUD.transform.GetChild(0).gameObject.GetComponent<Animator>();
        }

        if (viewportAnimator != null)
        {
            viewportAnimator.Play("SmallViewport");
        }
        else
        {
            Debug.LogError("viewportAnimator is null in Initialize. Check HUD structure.");
        }*/
        
        // LINNEAUS
        linnaeusAnimation = GameObject.Find("LinnaeusAnimation");
        
        if (linnaeusAnimation != null)
        {
            linnaeusAnimation.SetActive(false);
        }
        else
        {
            Debug.LogWarning("LinnaeusAnimation not found in scene.");
        }
        
        /*// INTRO DIALOGUE -- this is temp, will be removed later
        // RE-ASSIGN DIALOGUE RUNNER IF NECESSARY
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
        }

        if (dialogueRunner == null)
        {
            Debug.LogError("dialogueRunner is null. Dialogue cannot start.");
        }
        else if (!string.IsNullOrEmpty(introDialogueNode) && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(introDialogueNode);
            Debug.Log($"Dialogue started with node: {introDialogueNode}");
        }
        else
        {
            Debug.LogError("introDialogueNode is null or empty. Dialogue cannot start.");
        }*/

        GameManager.instance.isInBattle = true;

        Debug.Log("LevelManager_0.Initialize finished.");
    }

    public override void StartCutsScene(string cutSceneDialogueNode)
    {
        base.StartCutsScene(cutSceneDialogueNode);
        
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
        
        Invoke(nameof(UIManager.instance.DisableAnimator), 0.65f);
    }

    public override void ExitBattleDialogue()
    {
        base.ExitBattleDialogue();
        
        DialogueManager.instance.StopDialogue();
        
        // ANIMATION
        UIManager.instance.PlayAnimation("SmallViewportTransition_Reversed");
        
        linnaeusAnimation.SetActive(false);
        
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
        linnaeusAnimation.SetActive(false);
        DialogueManager.instance.StartDialogue("DeadDialogue");
    }
    
    [YarnCommand("DisableLinnaeusAnimation")]
    private void DisableLinnaeusAnimation()
    {
        linnaeusAnimation.SetActive(false);
    }
}
