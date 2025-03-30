using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_Prologue_CommonRoom : LevelManagerBase
{
    //Galleria
    public GameObject GalleriaAnimation;
    public GameObject ShrineZoom;
    public GameObject PortraitZoom;
    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(false);
        GameManager.instance.CGDisplay.SetActive(true);
        
        
        // Disable NPC animation display
        ImerisAnimation.SetActive(false);
        NPCAnimation.SetActive(false);
        
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Prologue/PrologueImerisRoom");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        GalleriaAnimation = GameObject.Find("GalleriaAnimation");
        GalleriaAnimation.SetActive(false);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //play the Galleria dialogue
        //FindObjectOfType<DialogueRunner>().StartDialogue("PrologueGalleria");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   
}
