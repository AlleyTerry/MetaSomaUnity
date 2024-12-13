using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Yarn.Unity.Example;

public enum GameState
{
    Opening,
    InGame,
    MainMenu,
    IsDead,
    Ending
}

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
    private int currentLevelIndex;
    public int CurrentLevelIndex
    {
        get => currentLevelIndex;
        set
        {
            currentLevelIndex = value;
            HandleLevelChange();
        }
    }
    public LevelManagerBase currentLevelManager;
    
    // HUNGER METER
    public int hungerMeter = 100;
    
    // GAME STATE
    [SerializeField] private GameState currentGameState;
    public GameState CurrentGameState
    {
        get => currentGameState;
        set
        {
            currentGameState = value;
            HandleGameStateSwitch();
        }
    }
    
    // TODO: MERGE THIS TO GAME STATE
    public bool isInBattle = false;
    //public bool isDead = false;
    
    // DIALOGUE RUNNER
    public DialogueRunner dialogueRunner;
    /*public bool isDialogueRunning = false;*/
    public InMemoryVariableStorage inMemoryVariableStorage;
    
    // CURRENT CHARACTER NAME
    [SerializeField] private GameObject characterName;
    private string lastSpeakerName = null;
    
    // DIALOGUE TEXT
    private TextMeshProUGUI dialogueTextBox;
    
    // Start is called before the first frame update
    void Start()
    {
        // HUD
        HUD = GameObject.FindWithTag("HUD");
        HUD.SetActive(false);
        
        // SETUP YARN SYSTEM
        GetDialogueRunner();
        GetInMemoryVariableStorage();
        GetCharacterName();
        
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
        if (dialogueRunner == null)
        {
            GetDialogueRunner();
        }
        
        if (inMemoryVariableStorage == null)
        {
            GetInMemoryVariableStorage();
        }
        
        if (characterName == null)
        {
            GetCharacterName();
        }
    }
    
    private void HandleGameStateSwitch()
    {
        switch (currentGameState)
        {
            case GameState.Opening:
                break;
            case GameState.InGame:
                break;
            case GameState.MainMenu:
                break;
            case GameState.IsDead:
                currentLevelManager.DeadScene();
                break;
            case GameState.Ending:
                break;
        }
    }
    
    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
    }
    
    [YarnCommand("RestartGameFromOpening")]
    public void RestartGameFromOpening()
    {
        // Quit the game
        Invoke(nameof(QuitGame), 0.75f);
        ImerisMovement.instance.currentState = new BeforeAnyEvolutionState(SubState.Healthy);
        HUD.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void HandleLevelChange()
    {
        Debug.Log($"Switching to level index: {currentLevelIndex}");
        
        if (currentLevelManager != null)
        {
            Destroy(currentLevelManager);
            currentLevelManager = null;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(currentLevelIndex);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // TODO FOR DEBUGGING PURPOSES, WILL BE REMOVED IN FINAL BUILD
        Debug.Log($"Scene loaded: {scene.name}, Index: {scene.buildIndex}");
        
        /*if (GameManager.instance != null)
        {
            Debug.Log("GameManager instance exists.");
            if (GameManager.instance.HUD != null)
            {
                Debug.Log("HUD still exists in GameManager after scene load.");
                var animatorCheck = GameManager.instance.HUD.transform.GetChild(0).gameObject.GetComponent<Animator>();
                if (animatorCheck != null)
                {
                    Debug.Log("viewportAnimator still exists after scene load.");
                }
                else
                {
                    Debug.LogError("viewportAnimator is missing after scene load!");
                }
            }
            else
            {
                Debug.LogError("HUD is missing in GameManager after scene load!");
            }
        }
        else
        {
            Debug.LogError("GameManager instance is missing after scene load!");
        }*/
        
        if (scene.buildIndex == currentLevelIndex)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (currentLevelManager == null)
            {
                Debug.Log($"Initializing LevelManager for level index: {currentLevelIndex}");
                
                // NOTE: TODO: THIS IS TEMP, NEED TO BE ITERATED THROUGH
                switch (currentLevelIndex)
                {
                    case 0:
                        break;
                    case 1:
                        currentLevelManager = gameObject.AddComponent<LevelManager_Graybox>();
                        break;
                    case 2:
                        currentLevelManager = gameObject.AddComponent<LevelManager_0>();
                        break;
                }
                
                /*if (currentLevelManager != null)
                {
                    Debug.Log($"{currentLevelManager.GetType().Name} successfully added!!!!!!!!!");
                }
                else
                {
                    Debug.LogError("Failed to add LevelManager. AddComponent returned null!!!!!!");
                }*/
                
                if (currentLevelManager != null)
                {
                    StartCoroutine(DelayedInitialize());
                }
            }
        }
    }

    private IEnumerator DelayedInitialize()
    {
        yield return new WaitForEndOfFrame(); 
        
        if (currentLevelManager != null)
        {
            currentLevelManager.Initialize();
            Debug.Log($"{currentLevelManager.GetType().Name} initialized successfully after delay.");
        }
    }
    
    public void GetDialogueRunner()
    {
        //Debug.Log("Getting DialogueRunner and InMemoryVariableStorage");
        
        // SETUP DIALOGUE RUNNER
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
    
    public void GetInMemoryVariableStorage()
    {
        // SETUP IN MEMORY VARIABLE STORAGE
        inMemoryVariableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }
    
    public void GetCharacterName()
    {
        // SETUP CHARACTER NAME
        characterName = GameObject.FindWithTag("SpeakerNameGetter");
    }
    
    public void GetLevelManager()
    {
        // SETUP LEVEL MANAGER
        if (currentLevelManager == null)
        {
            currentLevelManager = GetComponent<LevelManagerBase>();
            if (currentLevelManager == null)
            {
                Debug.LogError("Failed to find LevelManagerBase!");
            }
        }
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
        if (characterName == null)
        {
            Debug.LogWarning("CharacterName GameObject is null!");
            return null;
        }

        return characterName.GetComponent<TextMeshProUGUI>()?.text;
    }
}
