using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_0 : LevelManagerBase
{
    // LINNEAUS
    public GameObject linneausAnimation;
    
    protected override void Start()
    {
        base.Start();
        
        // LINNEAUS
        linneausAnimation = dialogueRunner.gameObject.transform.GetChild(0).GetChild(3).gameObject;
        linneausAnimation.SetActive(false);
        
        // TRANSITION ANIMATION
        viewportAnimator.Play("SmallViewport");
    }

    public override void CutsScene()
    {
        base.CutsScene();
        linneausAnimation.SetActive(true); 
    }
    
    public override void BattleScene()
    {
        base.BattleScene();
        
        // ANIMATION
        viewportAnimator.Play("SmallViewportTransition");
        
        // LINNEAUS
        linneausAnimation.SetActive(true); // For now we don't really have the cutscene, will be commented out later
    }
}
