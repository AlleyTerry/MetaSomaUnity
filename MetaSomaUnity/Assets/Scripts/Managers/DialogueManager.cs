using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    private GameObject lineview;

    private string lastSpeakerName = null;
    
    // different UI background image
    public Sprite narratorBackground;
    public Sprite imerisBackground;
    
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log($"{GetType().Name} disabled in Menu scene.");
            //enabled = false; // disable the script
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
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }
        
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
            
            lineview = dialogueRunner.GetComponentInChildren<LineView>().gameObject;
            
            GameObject lineviewBackground = lineview.transform.GetChild(0).gameObject;
            Vector2 rtMin = lineviewBackground.GetComponent<RectTransform>().offsetMin;
            Vector2 rtMax = lineviewBackground.GetComponent<RectTransform>().offsetMax;
            
            switch (speakerName)
            {
                case "Imeris":
                    characterNameText.alignment = TextAlignmentOptions.Left;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.TopLeft;
                    dialogueTextBox.fontStyle = FontStyles.Normal; // Reset the text style
                    dialogueTextBox.fontWeight = FontWeight.SemiBold;
                    lineview.GetComponent<RectTransform>().sizeDelta = new Vector2(450f, 0);
                    break;

                case "Linnaeus":
                    characterNameText.alignment = TextAlignmentOptions.Right;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.TopRight;
                    dialogueTextBox.fontStyle = FontStyles.Normal; // Reset the text style
                    dialogueTextBox.fontWeight = FontWeight.SemiBold;
                    lineview.GetComponent<RectTransform>().sizeDelta = new Vector2(450f, 0);
                    break;

                case "Narrator":
                    characterNameText.alignment = TextAlignmentOptions.Center;
                    characterNameText.color = new Color(255, 255, 255, 0);
                    dialogueTextBox.alignment = TextAlignmentOptions.Top;
                    dialogueTextBox.fontStyle = FontStyles.Normal; // Reset the text style
                    dialogueTextBox.fontWeight = FontWeight.SemiBold;
                    lineview.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f, 0);
                    break;
                
                case "IMERIS":
                    characterNameText.alignment = TextAlignmentOptions.Center;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.Top;
                    dialogueTextBox.fontStyle = FontStyles.Normal; // Reset the text style
                    dialogueTextBox.fontWeight = FontWeight.SemiBold;
                    lineview.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f, 0);
                    break;
                
                case "imeris":
                    characterNameText.alignment = TextAlignmentOptions.Center;
                    characterNameText.color = new Color(255, 255, 255, 0);
                    dialogueTextBox.alignment = TextAlignmentOptions.Top;
                    dialogueTextBox.fontStyle = FontStyles.Italic; // Italicize the text
                    dialogueTextBox.fontWeight = FontWeight.SemiBold;
                    lineview.GetComponent<RectTransform>().sizeDelta = new Vector2(450f, 0);
                    break;
                
                default:
                    characterNameText.alignment = TextAlignmentOptions.Center;
                    characterNameText.color = new Color(255, 255, 255, 255);
                    dialogueTextBox.fontStyle = FontStyles.Normal; // Reset the text style
                    dialogueTextBox.fontWeight = FontWeight.SemiBold;
                    lineview.GetComponent<RectTransform>().sizeDelta = new Vector2(450f, 0);
                    break;
            }

            switch (speakerName)
            {
                case "Narrator":
                    lineviewBackground.GetComponent<Image>().sprite = narratorBackground;
                    rtMax.y = 20;
                    lineviewBackground.GetComponent<RectTransform>().offsetMax = rtMax;
                    break;
                case "imeris":
                    rtMax.y = 20;
                    lineviewBackground.GetComponent<RectTransform>().offsetMax = rtMax;
                    break;
                case "IMERIS":
                    lineviewBackground.GetComponent<Image>().sprite = narratorBackground;
                    rtMax.y = 80;
                    lineviewBackground.GetComponent<RectTransform>().offsetMax = rtMax;
                    break;
                default:
                    lineviewBackground.GetComponent<Image>().sprite = imerisBackground;
                    rtMax.y = 80;
                    lineviewBackground.GetComponent<RectTransform>().offsetMax = rtMax;
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
