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
    
    [SerializeField] Animator CGDisplayAnimator;
    [SerializeField] RuntimeAnimatorController CGDisplayAnimatorController;
    
    public override void Initialize()
    {
        base.Initialize();
        
        // Intro cut scene
        CGDisplayAnimator = transform.GetChild(1).GetComponentInChildren<Animator>();
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/IntroScene/Open_Crawl");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        
        GameManager.instance.CGDisplay.SetActive(true);
        
        DisableImerisAnimation();
        DisableNPCAnimation();
        
        // Start intro dialogue
        DialogueManager.instance.StartDialogue(openingCrawlDialogueNode);
    }
    
    [YarnCommand("LeaveOpeningCrawl")]  
    public void LeaveOpeningScene()
    {
        GameManager.instance.LoadNextLevel();
    }

    protected override void Update()
    {
        base.Update();
    }
    
    [YarnCommand("PlayAnimation")]
    public void AnimationState(string state)
    {
        CGDisplayAnimator.Play(state);
    }
}
