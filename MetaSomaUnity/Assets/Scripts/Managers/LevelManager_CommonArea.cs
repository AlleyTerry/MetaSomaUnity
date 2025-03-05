using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager_CommonArea : LevelManagerBase
{
    [FormerlySerializedAs("item")] 
    [SerializeField] private GameObject bustMiniGameHolder;
    [SerializeField] private GameObject bustBackGroundHolder;
    
    [SerializeField] private Transform defaultSpawnPoint;
    [SerializeField] private Transform spawnPointFromCafe;
    [SerializeField] private Transform spawnPointFromChapel;

    private Transform GetSpawnPoint(string lastScene)
    {
        switch (lastScene)
        {
            case "Level_ServantsHall":
                return defaultSpawnPoint;
            case "Level_Cafeteria":
                return spawnPointFromCafe;
            case "Level_Chapel":
                return spawnPointFromChapel;
            default:
                return defaultSpawnPoint;
        }
    }

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
        bustBackGroundHolder = GameObject.Find("BustBackground");
        
        // Set up spawn points
        defaultSpawnPoint = GameObject.Find("SpawnPoint").transform;
        spawnPointFromCafe = GameObject.Find("SpawnPointFromCafe").transform;
        spawnPointFromChapel = GameObject.Find("SpawnPointFromChapel").transform;
        
        Transform spawnPoint = GetSpawnPoint(GameManager.instance.GetPreviousScene());
        Debug.Log("Spawn Point: " + spawnPoint.position);
        
        // Set up Imeris respawn position
        GameObject.FindObjectOfType<ImerisMovement>().gameObject.transform.position = spawnPoint.position;
        
        if (GameManager.instance.talkToGrub)
        {
            // if key is found, set bust2 to active
            bust1.SetActive(false);
            bust2.SetActive(true);
            bustBackGroundHolder.SetActive(true);
            
            bustMiniGameHolder.SetActive(true);
        }
        else
        {
            // by default, set bust1 to active
            bust1.SetActive(true);
            bust2.SetActive(false);
            bustBackGroundHolder.SetActive(false);
            
            // minigame is not active
            bustMiniGameHolder.SetActive(false);
        }

        //StartCoroutine(DelayedSetSpawn(GameManager.instance.GetPreviousScene()));
    }

    private void DelayedSetSpawn(string previousScene)
    {
        //yield return new WaitForEndOfFrame();
        
        // Set up Imeris respawn position
        Debug.Log("Previous Scene: " + GameManager.instance.GetPreviousScene());
        
        ImerisMovement imerisMovement = Imeris.GetComponent<ImerisMovement>();
        
        /*if (previousScene == "Level_ServantsHall")
        {
            Debug.Log("Imeris coming from Servants Hall.");
            
            imerisMovement.SetSpawnPosition(imerisPositionOrigin, imerisMovement.isFacingRight);
        }
        else if (previousScene == "Level_Cafeteria")
        {
            imerisMovement.SetSpawnPosition(imerisPositionFromCafe,imerisMovement.isFacingRight);
        }
        else if (previousScene == "Level_Chapel")
        {
            // todo: nothing now??
        }*/
    }
}
