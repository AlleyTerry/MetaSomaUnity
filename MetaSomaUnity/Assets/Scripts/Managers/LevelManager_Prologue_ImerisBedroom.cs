using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_Prologue_ImerisBedroom : LevelManagerBase
{
    //Galleria
    public GameObject GalleriaAnimation;
    public GameObject ShrineZoom;
    public GameObject PortraitZoom;
    public override void Initialize()
    {
        base.Initialize();
        //change heart animation to prologue viewport
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("PureBlack");
        UIManager.instance.DisableAnimator();
        
        GameManager.instance.HUD.SetActive(false);
        GameManager.instance.CGDisplay.SetActive(true);
        
        
        // Disable NPC animation display
        ImerisAnimation.SetActive(false);
        NPCAnimation.SetActive(false);
        
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Prologue/PrologueImerisRoom");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        GalleriaAnimation = GameObject.Find("GalleriaAnimation");
        GalleriaAnimation.SetActive(false);
        
        ShrineZoom = GameObject.Find("ShrineZoom");
        ShrineZoom.SetActive(false);
        PortraitZoom = GameObject.Find("PortraitZoom");
        PortraitZoom.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        //play the Galleria dialogue
        FindObjectOfType<DialogueRunner>().StartDialogue("PrologueGalleria");
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        //find the BlackScreen
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [YarnCommand("ShowGalleria")]
    public void ShowGalleria()
    {
        GalleriaAnimation.SetActive(true);
        //play the Galleria animation
        GalleriaAnimation.GetComponent<Animator>().Play("ShowGalleria");
        Debug.Log("Galleria shown");
        
    }
    
    [YarnCommand("HideGalleria")]
    public void HideGalleria()
    {
        GalleriaAnimation.SetActive(false);
        Debug.Log("Galleria hidden");
    }
    
    [YarnCommand("StartLevel")]
    public void StartLevel()
    {
        // resume controls
        GameManager.instance.ResumeControls();
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("PrologueViewport");
        
        // imeris figure
        ImerisAnimation.SetActive(false);
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        //turn off black screen
        GameObject blackScreen = GameObject.Find("BlackScreen");
        blackScreen.SetActive(false);
    }
    
    [YarnCommand("ShowShrineZoom")]
    public void ShowShrineZoom()
    {
        ShrineZoom.SetActive(true);
        Debug.Log("Shrine Zoom shown");
    }
    
    [YarnCommand("HideShrineZoom")]
    public void HideShrineZoom()
    {
        ShrineZoom.SetActive(false);
    }
    
    [YarnCommand("ShowPortraitZoom")]
    public void ShowPortraitZoom()
    {
        PortraitZoom.SetActive(true);
        Debug.Log("Portrait Zoom shown");
    }
    
    [YarnCommand("HidePortraitZoom")]
    public void HidePortraitZoom()
    {
        PortraitZoom.SetActive(false);
    }
    
    
}
