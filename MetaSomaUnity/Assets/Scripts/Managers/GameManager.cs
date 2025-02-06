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
    
    public InMemoryVariableStorage inMemoryVariableStorage;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO: NOTE -- THIS IS FOR DEBUGGING PURPOSES, MAY BE COMMENT OUT IN FINAL BUILD
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level_Intro":
                CurrentLevelIndex = 1;
                break;
            case "Level_ImerisBedroom":
                CurrentLevelIndex = 2;
                break;
            case "Level_ServantsHall":
                CurrentLevelIndex = 3;
                break;
            case "Level_CommonArea":
                CurrentLevelIndex = 4;
                break;
            case "Level_Chapel":
                CurrentLevelIndex = 5;
                break;
            default:
                CurrentLevelIndex = 0;
                break;
        }
        InstantiateLevelManager();
        
        // SET UP LEVEL INDEX AND SET UP THE LEVEL MANAGER
        /*CurrentLevelIndex = 0;*/
        isInBattle = false;
        
        // SETUP YARN SYSTEM
        GetInMemoryVariableStorage();
        
        Debug.Log($"GameManager initialized. CurrentLevelIndex: {CurrentLevelIndex}, isInBattle: {isInBattle}");
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
        isInBattle = false;
        
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
            case 1:
                /*Debug.Log("No LevelManager assigned.");*/
                currentLevelManager = gameObject.AddComponent<LevelManager_Intro>();
                break;
            case 2:
                currentLevelManager = gameObject.AddComponent<LevelManager_ImerisBedroom>();
                break;
            case 3:
                currentLevelManager = gameObject.AddComponent<LevelManager_ServantsHall>();
                break;
            case 4:
                currentLevelManager = gameObject.AddComponent<LevelManager_CommonArea>();
                break;
            case 5:
                currentLevelManager = gameObject.AddComponent<LevelManager_Chapel>();
                break;
            default:
                Debug.Log("Unknown Scene. No LevelManager assigned.");
                return;
        }
        
        currentLevelManager.Initialize();
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
        Debug.Log("Initializing Heart...!!!!!!!!!!");
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
        isInBattle = true;
        /*Time.timeScale = 0;*/
    }
    
    [YarnCommand("ResumeControls")]
    public void ResumeControls()
    {
        isInBattle = false;
        Time.timeScale = 1;
    }
}
