using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_CommonArea : LevelManagerBase
{
    
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        // Disable NPC animation display
        NPCAnimation.SetActive(false);
        
        //look for key bool
        if (GameManager.instance.talkToGrub)
        {
            
            //if key is found, set bust2 to active
  
            GameObject bust1 = GameObject.Find("/Parallax/Midground/Interactables/Bust");
            bust1.SetActive(false);
            GameObject bust2 = GameObject.Find("/Parallax/Midground/Interactables/Bust2");
            bust2.SetActive(true);
        }
    }
}
