using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Duplicate DialogueManager found and destroyed.");
        }
    }
    
    // YARN DIALOGUE RUNNER
    public DialogueRunner dialogueRunner;
    
    // DIALOGUE UI
    private TextMeshProUGUI characterNameText;
    private TextMeshProUGUI dialogueTextBox;

    private string lastSpeakerName = null;
    
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" || !enabled)
        {
            Debug.Log($"{GetType().Name} disabled in Menu scene.");
            enabled = false; // disable the script
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Check if it's the Menu scene
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("DialogueManager skipped initialization in Menu scene.");
            return;
        }
        
        // SETUP DIALOGUE RUNNER
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        
        if (dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner not found in the scene!");
        }
        
        // SETUP DIALOGUE UI
        GetDialogueUI();
        
        if (dialogueRunner == null || characterNameText == null || dialogueTextBox == null)
        {
            Debug.LogError("DialogueManager setup incomplete: Missing DialogueRunner or UI references.");
        }
    }

    private void LateUpdate()
    {
        if (CheckDialogueRunner() && 
            dialogueRunner.IsDialogueRunning)
        {
            UpdateDialogueText();
        }
    }

    // START DIALOGUE
    public void StartDialogue(string nodeName)
    {
        if (CheckDialogueRunner() && 
            !string.IsNullOrEmpty(nodeName) && 
            !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(nodeName);
            Debug.Log($"Dialogue started: {nodeName}");
        }
        else
        {
            Debug.LogWarning("Cannot start dialogue: DialogueRunner or nodeName is null.");
        }
    }
    
    // STOP DIALOGUE
    public void StopDialogue()
    {
        dialogueRunner?.Stop();
    }

    private bool CheckDialogueRunner()
    {
        if (dialogueRunner == null)
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
            GetDialogueUI();
            
            if (dialogueRunner == null)
            {
                Debug.LogError("DialogueRunner not found in the scene!");
                return false;
            }
        }

        return true;
    }
    
    public void UpdateDialogueText()
    {
        // TODO: VERY IMPORTANT!! CANNOT FIND CHARACTER NAME!!
        if (!CheckDialogueRunner() || 
            characterNameText == null || 
            dialogueTextBox == null)
        {
            Debug.LogError("DialogueManager setup incomplete: Missing DialogueRunner or UI references.");
            return;
        }

        string speakerName = GetCurrentSpeakerName();

        if (speakerName != null && 
            speakerName != lastSpeakerName)
        {
            switch (speakerName)
            {
                case "Imeris":
                    characterNameText.alignment = TextAlignmentOptions.Left;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.TopLeft;
                    break;

                case "Linnaeus":
                    characterNameText.alignment = TextAlignmentOptions.Right;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.TopRight;
                    break;

                case "Narrator":
                    characterNameText.alignment = TextAlignmentOptions.Center;
                    characterNameText.color = new Color(255, 255, 255, 0);
                    dialogueTextBox.alignment = TextAlignmentOptions.Top;
                    break;
                default:
                    characterNameText.alignment = TextAlignmentOptions.Center;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    break;
            }

            lastSpeakerName = speakerName;
        }
    }

    private string GetCurrentSpeakerName()
    {
        return characterNameText?.text;
    }
    
    private void GetDialogueUI()
    {
        characterNameText = GameObject.FindWithTag("SpeakerNameGetter")?.GetComponent<TextMeshProUGUI>();
        dialogueTextBox = dialogueRunner?.dialogueViews[0].gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }
}
