using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_Cafeteria : LevelManagerBase
{
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        // Disable NPC animation display
        NPCAnimation.SetActive(false);
    }
}
