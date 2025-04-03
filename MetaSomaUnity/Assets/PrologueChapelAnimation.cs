using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
