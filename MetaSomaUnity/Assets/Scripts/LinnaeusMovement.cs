using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class LinnaeusMovement : MonoBehaviour
{
    private Animator linAnimator;
    private SpriteRenderer linSpriteRenderer;

    public Sprite backSprite;
    public Sprite frontSprite;
    
    //used to indicate when linnaeus turns to face imeris in the battle
    public bool timeToTurn = false;
    
    private bool blinkNotStarted = true;
    
    // Start is called before the first frame update
    void Start()
    {
        //initialize animator and sprite renderer, and ensure animator is not active on start
        linAnimator = GetComponent<Animator>();
        linAnimator.enabled = false;
        linSpriteRenderer = GetComponent<SpriteRenderer>();
        if (SceneManager.GetActiveScene().name == "Prologue_Chapel")
        {
            linAnimator.enabled = true;
        }
    }

    //turns on linnaeus's animator and starts his blink animation after .6 seconds, repeating every 4 seconds
    [YarnCommand("LinnaeusAnimationActivate")]
    public void LinnaeusAnimationActivate()
    {
        linAnimator.enabled = true;
        InvokeRepeating("LinnaeusBlink", 0.6f,4f);
    }

    //plays the blink animation
    void LinnaeusBlink()
    {
        linAnimator.Play("Lin_Blink");
    }
   
}
