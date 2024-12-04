using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    // HUNGER METER
    public int hungerMeter = 100;
    
    // GAME STATE
    public bool isInBattle = false;
    
    // DIALOGUE RUNNER
    public DialogueRunner dialogueRunner;
    /*public bool isDialogueRunning = false;*/
    
    // CURRENT CHARACTER NAME
    private GameObject characterName;
    private string lastSpeakerName = null;
    
    // DIALOGUE TEXT
    private TextMeshProUGUI dialogueTextBox;
    
    // Start is called before the first frame update
    void Start()
    {
        // SETUP DIALOGUE RUNNER
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        
        // SETUP CHARACTER NAME
        characterName = GameObject.FindWithTag("SpeakerNameGetter");
        
        // SETUP DIALOGUE TEXT
        dialogueTextBox = dialogueRunner.dialogueViews[0].gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        /*isDialogueRunning = dialogueRunner.IsDialogueRunning;*/
        
        
    }

    private void LateUpdate()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            SetDialogueText();
        }
    }

    public void FreezeControls()
    {
        isInBattle = true;
        Time.timeScale = 0;
    }
    
    public void ResumeControls()
    {
        isInBattle = false;
        Time.timeScale = 1;
    }
    
    private void SetDialogueText()
    {
        string speakerName = GetCurrentSpeakerName();
        
        if (speakerName != null && 
            speakerName != lastSpeakerName)
        {
            switch (speakerName)
            {
                case "Imeris":
                    Debug.Log("Imeris is speaking");
                    characterName.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                    dialogueTextBox.alignment = TextAlignmentOptions.Left;
                    break;
                case "Linnaeus":
                    Debug.Log("Linnaeus is speaking");
                    characterName.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
                    dialogueTextBox.alignment = TextAlignmentOptions.Right;
                    break;
                case "Narrator":
                    Debug.Log("Narrator is speaking");
                    characterName.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                    characterName.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0);
                    dialogueTextBox.alignment = TextAlignmentOptions.Center;
                    break;
            }
            
            lastSpeakerName = speakerName;
        }
    }
    
    private string GetCurrentSpeakerName()
    {
        string speakerName = null;
        
        speakerName = characterName.GetComponent<TextMeshProUGUI>().text;
        
        return speakerName;
    }
}
