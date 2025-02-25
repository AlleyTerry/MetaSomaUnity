using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class minigameInput : MonoBehaviour
{
    public float upForce = 1.5f;

    public TextMeshProUGUI text;
    public bool isPressed = false;
    public int timesPressed = 0;
    public float timeLeft = 3f;
    public int pressedNumber;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //input makes the item go up
        // rapid input makes the item go up faster
        if (Input.GetKeyDown(KeyCode.Return))
        {
            text.text = "rappidly press up arrow to make the item go up";
            isPressed = true;
            //start the timer countdown to 0 from timeLeft
            
            
        }
        if (timeLeft <= 0)
        {
            isPressed = false;
            timerText.text = "0";
            EndGame();
        }
        
        GoUp();
    }

    public void GoUp()
    {
        

        if (isPressed)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = Mathf.Round(timeLeft).ToString();
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //add force
                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.Impulse);
                timesPressed++;
            
            }
        }

        
        
    }

    public void EndGame()
    {
        //text shows whether or not they got the right number of inputs
        if (timesPressed >= pressedNumber)
        {
            text.text = "You did it!";
        }
        else
        {
            text.text = "You failed!";
        }
        
    }
}
