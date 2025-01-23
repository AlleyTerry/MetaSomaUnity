using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_Intro : LevelManagerBase
{
    [SerializeField] private string openingCrawlDialogueNode = "OpeningCrawl";
    [SerializeField] DialogueManager dialogueManager;
    
    // DEBUG
    public bool isDialogueRunning = false;
    
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        dialogueManager = DialogueManager.instance;
    }
    
    [YarnCommand("LeaveOpeningCrawl")]  
    public void LeaveOpeningScene()
    {
        GameManager.instance.LoadNextLevel();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P))
        {
            DialogueManager.instance.StartDialogue(openingCrawlDialogueNode);
        }
        
    }
}
