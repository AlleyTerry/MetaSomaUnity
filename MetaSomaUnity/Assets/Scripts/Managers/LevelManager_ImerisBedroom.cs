using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_ImerisBedroom : LevelManagerBase
{
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(false);
        GameManager.instance.CGDisplay.SetActive(true);
        
        // Disable NPC animation display
        ImerisAnimation.SetActive(false);
        NPCAnimation.SetActive(false);
        
        // Play BGM
        //gameObject.GetComponent<AudioManager>().PlayMusic("level1");
        
        // Play intro CG
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/ImerisBedroomIntro/ImerisBedroomIntro");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        
        Invoke(nameof(DelayedFreezeControls), 0.1f);
        AnimationState("BedroomIntro_01_09");
        
        // Set Animation Trigger Vars
        CGDisplayAnimator.GetComponentInChildren<AnimationEventTrigger>().animationStateName = "BedroomIntro_10";
        CGDisplayAnimator.GetComponentInChildren<AnimationEventTrigger>().dialogueNodeName = "OpeningCutSceneText";
        
        if (CGDisplayAnimator.GetComponentInChildren<AnimationEventTrigger>().animationStateName == "" || 
            CGDisplayAnimator.GetComponentInChildren<AnimationEventTrigger>().dialogueNodeName == "")
        {
            Debug.LogWarning("Animation State Name or Dialogue Node Name not set in AnimationEventTrigger.");
        }
        // play background music
        gameObject.GetComponent<AudioManager>().PlayMusic("OpeningCutscene");
    }
    
    private void DelayedFreezeControls()
    {
        GameManager.instance.FreezeControls();
    }

    [YarnCommand("EnterLevel")]
    public void EnterLevel()
    {
        Invoke(nameof(SetUpLevel), 1.75f);
    }
    
    private void SetUpLevel()
    {
        GameObject.FindObjectOfType<SceneFade>().StartFadeIn();
        
        // resume controls
        GameManager.instance.ResumeControls();
        
        // imeris figure
        ImerisAnimation.SetActive(true);
        ImerisAnimation.GetComponent<Animator>().Play("ImerisBattleIdle");
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        // play background music
        gameObject.GetComponent<AudioManager>().PlayMusic("LevelAmbience");
    }
}
