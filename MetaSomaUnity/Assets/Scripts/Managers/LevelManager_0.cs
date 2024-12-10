using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_0 : LevelManagerBase
{
    // LINNEAUS
    public GameObject linneausAnimation;
    
    // INTRO DIALOGUE NODE
    public string introDialogueNode = "";
    
    protected override void Start()
    {
        base.Start();
        
        // LINNEAUS
        linneausAnimation = dialogueRunner.gameObject.transform.GetChild(0).GetChild(3).gameObject;
        linneausAnimation.SetActive(false);
        
        // TRANSITION ANIMATION
        viewportAnimator.Play("SmallViewport");
        
        // INTRO DIALOGUE -- this is temp, will be removed later
        dialogueRunner.StartDialogue(introDialogueNode);

        GameManager.instance.isInBattle = true;
        
        // DEBUG
        Debug.Log("LevelManager_0 Running");
    }

    public override void CutsScene()
    {
        base.CutsScene();
        linneausAnimation.SetActive(true); 
        
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
        
        linneausAnimation.SetActive(false);
        
        // TEMPPPPP
        Destroy(GameObject.Find("OverworldLinnaeusDraft"));
    }
}
