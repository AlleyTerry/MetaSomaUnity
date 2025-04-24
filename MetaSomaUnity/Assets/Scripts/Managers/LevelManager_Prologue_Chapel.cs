using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;
public class LevelManager_Prologue_Chapel : LevelManagerBase
{

    public override void Initialize()
    {
        base.Initialize();
        gameObject.GetComponent<AudioManager>().StopBeeHumming();
        
        UIManager.instance.PlayAnimation("PrologueViewport");
        UIManager.instance.DisableAnimator();
        
        GameManager.instance.HUD.SetActive(false);
        GameManager.instance.CGDisplay.SetActive(true);
        
       
        // Disable NPC animation display
        ImerisAnimation.SetActive(true);
        ImerisAnimation.GetComponent<Animator>().Play("PrologueImerisUI");
        NPCAnimation.SetActive(false);
        
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Prologue/PrologueImerisRoom");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //play the Galleria dialogue
        //FindObjectOfType<DialogueRunner>().StartDialogue("PrologueGalleria");
        // resume controls
        GameManager.instance.ResumeControls();
        
        // imeris figure
        //ImerisAnimation.SetActive(true);
   
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        
        //play linn happy animation
        //LinnaeusAnimation.GetComponent<Animator>().Play("PrologueLinnaeusHappy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("SermonStart")]
    public void SermanStart()
    {
        //Imeris.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("PrologueViewportTransition");
        
    }
    
    [YarnCommand("ToIntroScene")]
    public void ToIntroScene()
    {
        Debug.Log("ToIntroScene called");
        // Load the next scene
        GameManager.instance.LoadNextLevel();
    }


}
