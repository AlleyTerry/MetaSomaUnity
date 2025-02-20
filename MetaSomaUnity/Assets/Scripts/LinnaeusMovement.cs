using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        //if the player hits A, linnaeus will turn.
        //TO DO: make this happen not when the player hits A, but when they get to the right point in the dialogue 
        if (Input.GetKeyDown(KeyCode.A))
        {
            timeToTurn = true;
        }
        
        //if linnaeus is facing backwards and it is time for him to turn around, 
        //will enable the animator (automatically playing the turn animation)
        //and then invoke the repeated blinking function after half a second
        if (timeToTurn && linSpriteRenderer.sprite == backSprite)
        {
            linAnimator.enabled = true;
            Invoke("StartBlinking", .5f);
        }
        
    }

    //plays the blink animation
    void LinnaeusBlink()
    {
        linAnimator.Play("Lin_Blink");
    }
    
    //the if prevents this from being called over and over
    //the invoke repeating function will make linnaeus blink every 4 seconds after .1 seconds from when it's called
    void StartBlinking()
    {
        if (blinkNotStarted)
        {
            InvokeRepeating("LinnaeusBlink", .1f,4f);
            blinkNotStarted = false;
        }
    }
}
