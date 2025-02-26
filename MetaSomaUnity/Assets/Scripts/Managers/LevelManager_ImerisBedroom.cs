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
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
    }
}
