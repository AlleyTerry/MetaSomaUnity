using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager_CommonArea : LevelManagerBase
{
    
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        //give the minigameInput instance a reference to the item
       GameObject item = GameObject.Find("/Parallax/Midground/item");
       item.SetActive(false);
       minigameInput.instance.bust = item;
       item.SetActive(false);
        
        
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
