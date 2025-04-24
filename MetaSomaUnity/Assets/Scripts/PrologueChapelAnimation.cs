using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PrologueChapelAnimation : MonoBehaviour
{
    public GameObject ImerisSpot;
    public GameObject LinSpot;
    public GameObject Linnaeus;
    public GameObject Imeris;
    public bool chapelStarted = false;
    public GameObject AudioGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        //find the audio game object on the parent object
        AudioGameObject = GameObject.Find("ManagerHolder");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Prologue_Chapel" && chapelStarted == false)
     
        {
            //find the Imeris and Linnaeus game objects 
            
            Imeris = GameObject.Find("Imeris");
            ImerisSpot = GameObject.Find("ImerisSpot");
            LinSpot = GameObject.Find("LinSpot");
            Linnaeus = GameObject.Find("Linnaeus");
            chapelStarted = true;
        }
    }

    public void MoveCharacters()
    {
        Debug.Log("this has been fired");
        
        Imeris.transform.position = ImerisSpot.transform.position;
        Linnaeus.transform.position = LinSpot.transform.position;
        
        UIManager.instance.PlayAnimation("PrologueViewportTransitionReversePrologue");
    }

    public void DisableUIManagerPrologue()
    {
        UIManager.instance.DisableAnimator();
        //play yarn dialogue
        FindObjectOfType<DialogueRunner>().StartDialogue("LinnaeusSermon");
        
    }
    
    [YarnCommand("ImerisZoom")]
    public void ImerisZoom()
    {
        AudioGameObject.GetComponent<AudioManager>().PlayMusic("EndPrologue");
        Debug.Log("imeriszoom");
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("PrologueViewportHalf");
    }

    [YarnCommand("ImerisThoughts")]
    public void ImerisThoughts()
    {
        UIManager.instance.DisableAnimator();
        FindObjectOfType<DialogueRunner>().StartDialogue("ImerisZoom");
    }
    
    [YarnCommand("ImerisFindsTruth1")]
    public void ImerisFindsTruth1()
    {
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("PrologueViewportHalfClose");
    }
    
    [YarnCommand("ImerisFindsTruth2")]
    public void ImerisFindsTruth2()
    {
        UIManager.instance.DisableAnimator();
        FindObjectOfType<DialogueRunner>().StartDialogue("StartImerisTruth");
    }
    
    [YarnCommand("EndPrologue")]
    public void EndPrologue()
    {
        //UIManager.instance.DisableAnimator();
        FindObjectOfType<DialogueRunner>().StartDialogue("EndPrologue");
    }
}
