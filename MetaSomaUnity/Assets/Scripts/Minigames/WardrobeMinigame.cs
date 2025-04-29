using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class WardrobeMinigame : MonoBehaviour
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
    public GameObject WardrobeText;
    
    // this is for indicator
    public Animator indicatorAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        //dialogueRunner = DialogueManager.instance.dialogueRunner;
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
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //add force
                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * upForce, ForceMode.Impulse);
                timesPressed++;
            
            }
        }
    }
    
    [YarnCommand("StartWardrobeMiniGame")]
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
        // hide indicator
        indicatorAnimator.gameObject.SetActive(false);
        
        //text shows whether or not they got the right number of inputs
        if (timesPressed >= pressedNumber)
        {
            //play yarnspinner dialogue
            //bust.SetActive(false);
            WardrobeText.SetActive(false);
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 200, ForceMode.Impulse);
            if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue("WardrobeWin");
        }
        else
        {
            isGameEnded = false;
            //play yarnspinner dialogue
            if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue("WardrobeLose");
        }
    }
}
