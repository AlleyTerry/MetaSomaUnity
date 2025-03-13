using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_Prologue_ServantsHall : LevelManagerBase
{
    public GameObject BlackSreen;
    public override void Initialize()
    //initialize the level
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(false);
        GameManager.instance.CGDisplay.SetActive(true);
        
        
        // Disable NPC animation display
        ImerisAnimation.SetActive(false);
        NPCAnimation.SetActive(false);
        
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Prologue/PrologueServantsHall");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
    }
    // Start is called before the first frame update
    void Start()
    {
        //play the Galleria dialogue
        FindObjectOfType<DialogueRunner>().StartDialogue("PrologueServantsHall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [YarnCommand("StartLevel")]
    public void StartLevel()
    {
        // resume controls
        GameManager.instance.ResumeControls();
        
        // imeris figure
        ImerisAnimation.SetActive(true);
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
    }
}
