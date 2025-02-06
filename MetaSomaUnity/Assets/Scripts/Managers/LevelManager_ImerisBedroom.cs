using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_ImerisBedroom : LevelManagerBase
{
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        // Disable NPC animation display
        NPCAnimation.SetActive(false);
        
        // Play BGM
        gameObject.GetComponent<AudioManager>().PlayMusic("level1");
    }
}
