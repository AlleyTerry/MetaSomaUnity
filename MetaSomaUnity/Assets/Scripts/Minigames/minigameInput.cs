using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class minigameInput : MonoBehaviour
{
    public float upForce = 1.5f;

    public TextMeshProUGUI text;
    public bool isPressed = false;
    public int timesPressed = 0;
    public float timeLeft = 3f;
    public int pressedNumber;
    public TextMeshProUGUI timerText;
    public GameObject bust;
    public DialogueRunner dialogueRunner;
    public GameObject bustSpot;
    public GameObject bustSpot2;
    public GameObject bustText;
    public float forceAmount = 100f;
    
    // this is for indicator
    public Animator indicatorAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = DialogueManager.instance.dialogueRunner;
        GameObject bust;

        indicatorAnimator = GetComponentInChildren<Animator>();
        indicatorAnimator.gameObject.SetActive(false);
    }

    private bool isGameEnded = false;
    
    // Update is called once per frame
    void Update()
    {
        //input makes the item go up
        // rapid input makes the item go up faster
        if (timeLeft <= 0 && !isGameEnded)
        {
            isPressed = false;
            //timerText.text = "0";
            EndGame();
            isGameEnded = true;
        }
        
        GoUp();
    }

    public void GoUp()
    {
        if (isPressed)
        {
            timeLeft -= Time.deltaTime;
            //timerText.text = Mathf.Round(timeLeft).ToString();
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //add force
                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Impulse);
                timesPressed++;
            
            }
        }
    }
    
    [YarnCommand("StartMiniGame")]
    public void StartMiniGame()
    {
        isGameEnded = false;
        // show indicator
        indicatorAnimator.gameObject.SetActive(true);
        indicatorAnimator.Play("HandIndicator");
        //start the timer countdown to 0 from timeLeft
        //start the timer countdown to 0 from timeLeft
        timeLeft = 3f;
        isPressed = true;
        //bust.SetActive(true);
    }

    public void EndGame()
    {
        //text shows whether or not they got the right number of inputs
        if (timesPressed >= pressedNumber)
        {
            //play yarnspinner dialogue
            //bust.SetActive(false);
            //lerp the bust to the floor
            //the thing the bust is on should be false
            bustSpot.SetActive(false);
            bustSpot2.SetActive(true);
            indicatorAnimator.gameObject.SetActive(false);
            bustText.SetActive(false);
            gameObject.GetComponent<Rigidbody>().AddTorque(0, 0, 90);
            if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue("minigameSuccess");
        }
        else
        {
            // hide indicator
            indicatorAnimator.gameObject.SetActive(false);
            isGameEnded = false;
            //play yarnspinner dialogue
            if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue("minigameFail");
        }
    }
    
    [YarnCommand("TurnOffBust")]
    public void TurnOffBust()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
