using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    public bool talkToGrub = false; // talk to half-dead grub
    // SINGLETON
    public bool isTestMode = true; // TODO: REMOVE THIS IN FINAL BUILD
    
    public static GameManager instance;
    
    private void Awake()
    {
        Debug.Log("GameManager initialized. Test mode: " + isTestMode);
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Duplicate GameManager found and destroyed.");
        }
        
        Debug.Log("GameManager Awake completed.");
    }
    
    // SCENE HISTORY
    public string previousScene;
    // IMERIS FACING
    public bool isFacingRight = false;
    
    // HUD && CG PLAYER && FADE EFFECT
    public GameObject HUD;
    public GameObject CGDisplay;
    public SceneFade sceneFade;
    
    // LEVEL MANAGER
    [SerializeField] private int currentLevelIndex;
    
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
    
    // KEY FOR ENTERING THE CHAPEL,
    // WITHOUT THIS KEY, PLAYER CANNOT ENTER CHAPEL
    public bool isHoldingChapelKey = false;
    
    // SCENE CHANGE BUFFER
    private bool isBuffering = false;

    public bool IsBuffering
    {
        get => isBuffering;
        set
        {
            isBuffering = value;

            if (isBuffering)
            {
                Invoke(nameof(ResetBuffer), 2.0f);
            }
        }
    }
    
    private void ResetBuffer () => isBuffering = false;

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
            Debug.Log($"Game State switched to: {currentGameState}");
            HandleGameStateSwitch();
        }
    }
    
    // TODO: MERGE THIS TO GAME STATE
    public bool isInBattle = false;
    
    public InMemoryVariableStorage inMemoryVariableStorage;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO: NOTE -- THIS IS FOR DEBUGGING PURPOSES, MAY BE COMMENT OUT IN FINAL BUILD
        switch (SceneManager.GetActiveScene().name)
        {
            /*case "Level_Intro":
                CurrentLevelIndex = 1;
                break;*/
            
            case "Prologue_ImerisRoom":
                CurrentLevelIndex = 1;
                break;
            case "Prologue_CommonRoom":
                CurrentLevelIndex = 2;
                break;
            case "Prologue_Chapel":
                CurrentLevelIndex = 3;
                break;
            
            case "Level_ImerisBedroom":
                CurrentLevelIndex = 4;
                break;
            case "Level_ServantsHall":
                CurrentLevelIndex = 5;
                break;
            case "Level_CommonArea":
                CurrentLevelIndex = 6;
                break;
            case "Level_Chapel":
                CurrentLevelIndex = 7;
                break;
            case "Level_Cafeteria":
                currentLevelIndex = 8;
                break;
            default:
                CurrentLevelIndex = 0;
                break;
        }
        InstantiateLevelManager();
        
        // SET UP LEVEL INDEX AND SET UP THE LEVEL MANAGER
        /*CurrentLevelIndex = 0;*/
        ResumeControls();
        
        // SETUP YARN SYSTEM
        GetInMemoryVariableStorage();
        
        Debug.Log($"GameManager initialized. CurrentLevelIndex: {CurrentLevelIndex}, isInBattle: {isInBattle}");
        
        // DISABLE MOUSE INPUT
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // SCENE BUFFER
        isBuffering = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (inMemoryVariableStorage == null)
        {
            GetInMemoryVariableStorage();
        }*/
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
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    
    public void LoadMainMenu() // TODO: THIS IS VERY TEMPORARY, MAY HAVE A LOT OF BUGS
    {
        CurrentLevelIndex = 0;
        SceneManager.LoadSceneAsync(0);
    }

    public void LoadFirstChapter()
    {
        
    }
    
    public void LoadNextLevel()
    {
        CurrentLevelIndex++;
    }
    
    private void HandleLevelChange()
    {
        if (currentLevelManager != null)
        {
            Destroy(currentLevelManager);
            currentLevelManager = null;
        }

        if (SceneManager.GetActiveScene().buildIndex != currentLevelIndex)
        {
            Debug.Log($"Switching to level index: {currentLevelIndex}");
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            if (sceneFade == null)
            {
                Debug.LogWarning("SceneFade not found. Loading scene without fade effect.");
                SceneManager.LoadSceneAsync(currentLevelIndex);
            }
            else
            {
                Debug.Log("Loading scene with fade effect.");
                sceneFade.LoadScene(currentLevelIndex);
            }
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded.");
        
        // Reset the battle state
        ResumeControls();
        
        // Heart
        GetInMemoryVariableStorage();
        
        // HUD && CGD
        HUD = transform.Find("HUD")?.gameObject;
        
        if (HUD == null)
        {
            Debug.LogError("HUD not found in scene!");
        }
        else
        {
            HUD.SetActive(false);
        }
        
        CGDisplay = transform.Find("CGDisplay")?.gameObject;
        
        if (CGDisplay == null)
        {
            Debug.LogError("CGDisplay not found in scene!");
        }
        else
        {
            CGDisplay.SetActive(false);
        }

        if (sceneFade == null)
        {
            sceneFade = transform.Find("ScreenFade")?.GetComponent<SceneFade>();
        }
        
        if (scene.buildIndex == currentLevelIndex)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (currentLevelManager == null)
            {
                Debug.Log($"Initializing LevelManager for level index: {currentLevelIndex}");
                
                // ADD LEVEL MANAGER TO THE SCENE
                InstantiateLevelManager();
                
                // Call Initialize for the current LevelManager
                GetLevelManager();
                
                // UI Manager
                UIManager.instance.Initialize();
                
                // Dialogue Manager
                DialogueManager.instance.Initialize();
            }
        }
    }

    public void InstantiateLevelManager()
    {
        switch (currentLevelIndex)
        {
            case 0: // Main menu
                currentLevelManager = gameObject.AddComponent<LevelManager_MainMenu>();
                break;
            /*case 1:
                /*Debug.Log("No LevelManager assigned.");#1#
                currentLevelManager = gameObject.AddComponent<LevelManager_Intro>();
                break;*/
            
            // Actual levels
            case 4:
                currentLevelManager = gameObject.AddComponent<LevelManager_ImerisBedroom>();
                break;
            case 5:
                currentLevelManager = gameObject.AddComponent<LevelManager_ServantsHall>();
                break;
            case 6:
                currentLevelManager = gameObject.AddComponent<LevelManager_CommonArea>();
                break;
            case 7:
                currentLevelManager = gameObject.AddComponent<LevelManager_Chapel>();
                break;
            case 8:
                currentLevelManager = gameObject.AddComponent<LevelManager_Cafeteria>();
                break;
            
            // Prologue levels
            case 1:
                currentLevelManager = gameObject.AddComponent<LevelManager_Prologue_ImerisBedroom>();
                break;
            case 2:
                currentLevelManager = gameObject.AddComponent<LevelManager_Prologue_CommonRoom>();
                break;
            case 3:
                currentLevelManager = gameObject.AddComponent<LevelManager_Prologue_Chapel>();
                break;
            default:
                Debug.Log("Unknown Scene. No LevelManager assigned.");
                return;
        }
        
        currentLevelManager.Initialize();
    }

    public void SetPreviousScene(string sceneName)
    {
        previousScene = sceneName;
    }
    
    public string GetPreviousScene()
    {
        return previousScene;
    }
    
    public void GetInMemoryVariableStorage()
    {
        // SETUP IN MEMORY VARIABLE STORAGE
        inMemoryVariableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }
    
    public void GetLevelManager()
    {
        // SETUP LEVEL MANAGER
        if (currentLevelManager == null)
        {
            currentLevelManager = GetComponent<LevelManagerBase>();
            if (currentLevelManager == null)
            {
                Debug.LogError("Failed to find LevelManagerBase! Ensure a LevelManager is properly set up in the scene.");
            }
            else
            {
                Debug.Log($"LevelManager {currentLevelManager.GetType().Name} found successfully.");
            }
        }
    }
    
    public void InitializeHeart()
    {
        //Debug.Log("Initializing Heart...!!!!!!!!!!");
        Heart heart = transform.GetChild(0).Find("Viewport_Heart")?.GetComponent<Heart>();
        
        if (heart != null)
        {
            heart.enabled = true;
            heart.Initialize();
        }
        else
        {
            Debug.LogError("Heart not found in scene!");
        }
    }

    public void FreezeControls()
    {
        GameObject.FindObjectOfType<ImerisMovement>().rb.velocity = Vector3.zero;
        isInBattle = true;
        /*Time.timeScale = 0;*/
    }
    
    [YarnCommand("ResumeControls")]
    public void ResumeControls()
    {
        isInBattle = false;
        Time.timeScale = 1;
    }
    
    [YarnCommand("TalkedToHalfDeadGrub")]
    public void TalkedToHalfDeadGrub()
    {
        talkToGrub = true;
    }

    [YarnCommand("TablePush")]
    public void TablePush()
    {
        GameObject table = GameObject.FindGameObjectWithTag("table");
        table.GetComponent<Rigidbody>().isKinematic = true;
    }
}
