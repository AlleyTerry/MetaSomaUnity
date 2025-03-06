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
        GameManager.instance.CGDisplay.SetActive(true);
        
        DisableImerisAnimation();
        DisableNPCAnimation();
        
        // Start intro dialogue
        DialogueManager.instance.StartDialogue(openingCrawlDialogueNode);
        
        // Play BGM
        gameObject.GetComponent<AudioManager>().PlayMusic("openingCrawl");
        Debug.Log("Opening Crawl music playing.");
    }
    
    [YarnCommand("LeaveOpeningCrawl")]  
    public void LeaveOpeningScene()
    {
        Invoke(nameof(NextScene), 1f);
    }

    private void NextScene()
    {
        GameManager.instance.LoadNextLevel();
    }

    protected override void Update()
    {
        base.Update();
    }
}
