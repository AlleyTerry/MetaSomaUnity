using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PrologueChapelAnimation : MonoBehaviour
{
    public GameObject ImerisSpot;
    public GameObject LinSpot;
    public GameObject Linnaeus;
    public GameObject Imeris;

    
    // Start is called before the first frame update
    void Start()
    {
        Imeris = GameObject.Find("Imeris");
        ImerisSpot = GameObject.Find("ImerisSpot");
        LinSpot = GameObject.Find("LinSpot");
        Linnaeus = GameObject.Find("Linnaeus");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCharacters()
    {
        Debug.Log("this has been fired");
        
        Imeris.transform.position = ImerisSpot.transform.position;
        Linnaeus.transform.position = LinSpot.transform.position;
        
        UIManager.instance.PlayAnimation("PrologueViewportTransitionReverse");
    }

    public void DisableUIManager()
    {
        UIManager.instance.DisableAnimator();
        //play yarn dialogue
        FindObjectOfType<DialogueRunner>().StartDialogue("LinnaeusSermon");
        
    }
    
    [YarnCommand("ImerisZoom")]
    public void ImerisZoom()
    {
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
