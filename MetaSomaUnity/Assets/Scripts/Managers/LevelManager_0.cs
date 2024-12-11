using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity.Example;

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
        Debug.LogWarning("LevelManager_0 Start");
        
        // INIT
        cutSceneDialogueNode = "STARTBattle1Dialogue";
        battleDialogueNode = "Battle1Dialogue";
        
        // TRANSITION ANIMATION
        viewportAnimator.Play("SmallViewport");
        
        // LINNEAUS
        linnaeusAnimation = GameObject.Find("LinnaeusAnimation");
        linnaeusAnimation.SetActive(false);
        
        // INTRO DIALOGUE -- this is temp, will be removed later
        dialogueRunner.StartDialogue(introDialogueNode);
        GameManager.instance.isInBattle = true;
        
        // DEBUG
        Debug.Log("LevelManager_0 Running");
    }

    public override void CutsScene()
    {
        base.CutsScene();
        linnaeusAnimation.SetActive(true); 
        
        Debug.Log("Cut Scene Started");
    }
    
    public override void BattleScene()
    {
        base.BattleScene();
        
        // ANIMATION
        viewportAnimator.Play("SmallViewportTransition");
        
        // LINNEAUS
        //linneausAnimation.SetActive(true); // For now we don't really have the cutscene, will be commented out later
        
        Invoke(nameof(DisableAnimator), 0.65f);
        
        Debug.Log("Battle Scene Started");
    }

    public override void ExitBattleDialogue()
    {
        base.ExitBattleDialogue();
        
        // ANIMATION
        viewportAnimator.Play("SmallViewportTransition_Reversed");
        
        linnaeusAnimation.SetActive(false);
        
        // TEMPPPPP
        Destroy(GameObject.Find("OverworldLinnaeusDraft"));
    }
}
