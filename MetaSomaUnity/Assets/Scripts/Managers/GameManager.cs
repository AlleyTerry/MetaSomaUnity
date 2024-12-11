using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Yarn.Unity.Example;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager instance;
    
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
        }
    }
    
    // HUD
    public GameObject HUD;
    
    // LEVEL MANAGER
    private int currentLevelIndex = 0;
    public int CurrentLevelIndex
    {
        get => currentLevelIndex;
        set
        {
            currentLevelIndex = value;
            /*if (FindObjectOfType<YarnCharacterView>() != null)
            {
                Destroy(FindObjectOfType<YarnCharacterView>().gameObject);
            }*/
            HandleLevelChange();
        }
    }
    public LevelManagerBase currentLevelManager;
    
    // HUNGER METER
    public int hungerMeter = 100;
    
    // GAME STATE
    public bool isInBattle = false;
    
    // DIALOGUE RUNNER
    public DialogueRunner dialogueRunner;
    /*public bool isDialogueRunning = false;*/
    public InMemoryVariableStorage inMemoryVariableStorage;
    
    // CURRENT CHARACTER NAME
    private GameObject characterName;
    private string lastSpeakerName = null;
    
    // DIALOGUE TEXT
    private TextMeshProUGUI dialogueTextBox;
    
    // Start is called before the first frame update
    void Start()
    {
        // HUD
        HUD = GameObject.FindWithTag("HUD");
        
        // SETUP DIALOGUE RUNNER
        GetDialogueRunner();
        
        // SETUP CHARACTER NAME
        characterName = GameObject.FindWithTag("SpeakerNameGetter");
        
        // SETUP DIALOGUE TEXT
        dialogueTextBox = dialogueRunner.dialogueViews[0].gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        
        // SET UP LEVEL INDEX AND SET UP THE LEVEL MANAGER
        CurrentLevelIndex = 0;
        
        // NOTE: THIS IS FOR PURE DEBUGGING PURPOSES, COMMENT OUT IN FINAL BUILD
        switch (SceneManager.GetActiveScene().name)
        {
            case "Graybox":
                CurrentLevelIndex = 1;
                break;
            case "CombinedScene":
                CurrentLevelIndex = 2;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueRunner == null || inMemoryVariableStorage == null)
        {
            GetDialogueRunner();
        }
    }
    
    private void HandleLevelChange()
    {
        /*GetDialogueRunner();*/
        
        /*GameObject.FindObjectOfType<YarnCharacterView>().GetCamera();*/
           
        // NOTE: TODO: THIS IS TEMP, NEED TO BE ITERATED THROUGH
        switch (currentLevelIndex)
        {
            case 0:
                break;
            case 1:
                gameObject.AddComponent<LevelManager_Graybox>();
                GetLevelManager();
                break;
            case 2:
                Destroy(currentLevelManager);
                gameObject.AddComponent<LevelManager_0>();
                GetLevelManager();
                break;
        }
    }
    
    public void GetDialogueRunner()
    {
        Debug.Log("Getting DialogueRunner and InMemoryVariableStorage");
        
        // SETUP DIALOGUE RUNNER
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        inMemoryVariableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }
    
    public void GetLevelManager()
    {
        // SETUP LEVEL MANAGER
        currentLevelManager = FindObjectOfType<LevelManagerBase>();
    }
    
    
    [YarnCommand("IntoBattleScene")]
    public void IntoBattleScene()
    {
        currentLevelManager.BattleScene();
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
    
    [YarnCommand("ResumeControls")]
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
                    characterName.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.TopLeft;
                    break;
                case "Linnaeus":
                    Debug.Log("Linnaeus is speaking");
                    characterName.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
                    characterName.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
                    dialogueTextBox.alignment = TextAlignmentOptions.TopRight;
                    break;
                case "Narrator":
                    Debug.Log("Narrator is speaking");
                    characterName.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
                    characterName.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0);
                    dialogueTextBox.alignment = TextAlignmentOptions.Top;
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
