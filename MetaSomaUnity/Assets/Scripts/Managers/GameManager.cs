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
    
    public InMemoryVariableStorage inMemoryVariableStorage;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO: NOTE -- THIS IS FOR DEBUGGING PURPOSES, MAY BE COMMENT OUT IN FINAL BUILD
        switch (SceneManager.GetActiveScene().name)
        {
            case "Graybox":
                CurrentLevelIndex = 1;
                break;
            case "CombinedScene":
                CurrentLevelIndex = 2;
                break;
            default:
                CurrentLevelIndex = 0;
                break;
        }
        
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
        if (inMemoryVariableStorage == null)
        {
            GetInMemoryVariableStorage();
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
            SceneManager.LoadSceneAsync(currentLevelIndex);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded.");
        
        // Reset the battle state
        isInBattle = false;
        
        // HUD
        HUD = GameObject.FindWithTag("HUD");
        
        if (HUD == null)
        {
            Debug.LogError("HUD not found in scene!");
        }
        else
        {
            HUD.SetActive(false);
        }
        
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
                        Debug.Log("No LevelManager assigned.");
                        break;
                    case 1:
                        currentLevelManager = gameObject.AddComponent<LevelManager_Graybox>();
                        break;
                    case 2:
                        currentLevelManager = gameObject.AddComponent<LevelManager_0>();
                        break;
                    default:
                        Debug.Log("Unknown Scene. No LevelManager assigned.");
                        return;
                }
                
                // Call Initialize for the current LevelManager
                GetLevelManager();
                currentLevelManager.Initialize();
                
                // UI Manager
                UIManager.instance.Initialize();
                
                // Dialogue Manager
                DialogueManager.instance.Initialize();
            }
        }
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
    
}
