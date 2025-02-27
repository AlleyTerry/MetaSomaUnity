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
    
    //public static minigameInput instance;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        GameObject bust;
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
        //start the timer countdown to 0 from timeLeft
        //start the timer countdown to 0 from timeLeft
        timeLeft = 3f;
        isPressed = true;
        bust.SetActive(true);
    }

    public void EndGame()
    {
        //text shows whether or not they got the right number of inputs
        if (timesPressed >= pressedNumber)
        {
            //play yarnspinner dialogue
            bust.SetActive(false);
            dialogueRunner.StartDialogue("minigameSuccess");
        }
        else
        {
            //play yarnspinner dialogue
            dialogueRunner.StartDialogue("minigameFail");
            
        }
        
    }
}
