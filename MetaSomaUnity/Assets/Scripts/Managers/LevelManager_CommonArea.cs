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
            
            //if key is found, set table to active
            GameObject table = GameObject.FindGameObjectWithTag("table");
            table.GetComponent<BoxCollider>().isTrigger = false;
            GameObject tableDialogue = GameObject.Find("/Parallax/Midground/Interactables/Table");
            tableDialogue.SetActive(true);
            GameObject tableStopper = GameObject.Find("/Parallax/Midground/TableStopper");
            tableStopper.GetComponent<BoxCollider>().isTrigger = false;
            GameObject bust1 = GameObject.Find("/Parallax/Midground/Interactables/Bust");
            bust1.SetActive(false);
        }
    }
}
