using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_Prologue_CommonRoom : LevelManagerBase
{

    public override void Initialize()
    {
        base.Initialize();
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
        
        GameObject.FindObjectOfType<ImerisMovement>().UpdateImerisFacing(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        //play the Galleria dialogue
        //FindObjectOfType<DialogueRunner>().StartDialogue("PrologueGalleria");
        // resume controls
        GameManager.instance.ResumeControls();
        
        // imeris figure
       // ImerisAnimation.SetActive(true);
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   
}
