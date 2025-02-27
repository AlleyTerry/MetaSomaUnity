using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager_CommonArea : LevelManagerBase
{
    [FormerlySerializedAs("item")] 
    [SerializeField] private GameObject bustMiniGameHolder;
    
    [SerializeField] private Vector3 imerisPositionOrigin = new Vector3(-14.44f, -2.317f, 0);
    [SerializeField] private Vector3 imerisPositionFromCafe = new Vector3(-7.69f, -2.317f, 0);
    [SerializeField] private Vector3 imerisPositionFromChapel;
    
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
       
        // Disable NPC animation display
        NPCAnimation.SetActive(false);
        
        //give the minigame input instance a reference to the item
        bustMiniGameHolder = GameObject.Find("BustMiniGame");
        bustMiniGameHolder.GetComponent<minigameInput>().bust = bustMiniGameHolder;
        
        //look for key bool
        GameObject bust1 = GameObject.Find("Bust");
        GameObject bust2 = GameObject.Find("Bust2");
        
        if (GameManager.instance.talkToGrub)
        {
            // if key is found, set bust2 to active
            bust1.SetActive(false);
            bust2.SetActive(true);
            
            bustMiniGameHolder.SetActive(true);
        }
        else
        {
            // by default, set bust1 to active
            bust1.SetActive(true);
            bust2.SetActive(false);
            
            // minigame is not active
            bustMiniGameHolder.SetActive(false);
        }
        
        // Set up Imeris
        Debug.Log("Previous Scene: " + GameManager.instance.GetPreviousScene());
        
        ImerisMovement imerisMovement = Imeris.GetComponent<ImerisMovement>();
        
        if (GameManager.instance.GetPreviousScene() == "Level_ServantsHall")
        {
            Debug.Log("Imeris coming from Servants Hall.");
            
            imerisMovement.SetSpawnPosition(imerisPositionOrigin, true);
        }
        else if (GameManager.instance.GetPreviousScene() == "Level_Cafeteria")
        {
            imerisMovement.SetSpawnPosition(imerisPositionFromCafe,false);
        }
        else if (GameManager.instance.GetPreviousScene() == "Level_Chapel")
        {
            // todo: nothing now??
        }
    }
}
