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
    public GameObject BlackScreenFade;
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
        ImerisAnimation.SetActive(true);
        ImerisAnimation.GetComponent<Animator>().Play("PrologueImerisUI");
        NPCAnimation.SetActive(false);
        
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Prologue/PrologueImerisRoom");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        GalleriaAnimation = GameObject.Find("GalleriaAnimation");
        GalleriaAnimation.SetActive(false);
        
        //find the black screen
        BlackScreenFade = GameObject.Find("BlackScreenFade");
        //play the black screen fade animation
        BlackScreenFade.GetComponent<Animator>().Play("FadeBlackPrologue");
        
        ShrineZoom = GameObject.Find("ShrineZoom");
        ShrineZoom.SetActive(false);
        PortraitZoom = GameObject.Find("PortraitZoom");
        PortraitZoom.SetActive(false);
        //play background music
        gameObject.GetComponent<AudioManager>().PlayMusic("Memories");
    }
    // Start is called before the first frame update
    void Start()
    {
        //play the Galleria dialogue
        FindObjectOfType<DialogueRunner>().StartDialogue("PrologueGalleria");
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        
        
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
        GalleriaAnimation.GetComponent<Animator>().Play("FadeGalleria");
        Debug.Log("Galleria hidden");
    }
    
    [YarnCommand("StartPrologueLevel")]
    public void StartLevel()
    {
        //turn off black screen
        GameObject blackScreen = GameObject.Find("BlackScreen");
        blackScreen.SetActive(false);
        GalleriaAnimation.SetActive(false);
        // resume controls
        GameManager.instance.ResumeControls();
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("PrologueViewportTransitionReverse");
        
        // imeris figure
        ImerisAnimation.SetActive(true);
        GameManager.instance.HUD.SetActive(true); // show HUD
        GameManager.instance.CGDisplay.SetActive(false); // hide CG display
        
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
