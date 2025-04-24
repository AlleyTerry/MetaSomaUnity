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

    // Update is called once per frame
    void Update()
    {
        //input makes the item go up
        // rapid input makes the item go up faster
        if (timeLeft <= 0)
        {
            isPressed = false;
            //timerText.text = "0";
            EndGame();
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
            bust.transform.position = Vector3.Lerp(bust.transform.position, new Vector3(bust.transform.position.x, -2.5f, bust.transform.position.z), Time.deltaTime * 2f);
            if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue("minigameSuccess");
        }
        else
        {
            // hide indicator
            indicatorAnimator.gameObject.SetActive(false);
            
            //play yarnspinner dialogue
            if (!dialogueRunner.IsDialogueRunning) dialogueRunner.StartDialogue("minigameFail");
        }
    }
}
