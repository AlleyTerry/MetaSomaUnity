using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;

public class Heart : MonoBehaviour
{
    public InMemoryVariableStorage variableStorage;
    
    public float health = 3;
    public Sprite sprite1, sprite2, sprite3, sprite4;
    
    private Image renderer;
    
    private void Awake()
    {
        renderer = GetComponent<Image>();
    }

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

    public void Initialize()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log($"{GetType().Name} disabled in Menu scene.");
            return;
        }
        
        SetupYarnVariables();
        variableStorage.SetValue("$CurrentHealth", health);
        
        UpdateHealthUI();
    }

    public void SetupYarnVariables()
    {
        GameManager.instance.GetInMemoryVariableStorage();
        variableStorage = GameManager.instance.inMemoryVariableStorage;
        
        if (variableStorage == null)
        {
            Debug.LogError($"variableStorage is null in SetupYarnVariables! Called from: {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}");
        }
        else
        {
            Debug.Log($"variableStorage successfully assigned in SetupYarnVariables. Called from: {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}");
            //Debug.LogWarning(variableStorage.gameObject.name);
        }
    }
    
    private void UpdateHealthUI()
    {
        if (renderer == null)
        {
            Debug.LogError("Heart: Renderer is null. Cannot update health UI.");
            return;
        }
        
        switch (health)
        {
            case 3:
                renderer.sprite = sprite1;
                break;
            case 2:
                renderer.sprite = sprite2;
                break;
            case 1:
                renderer.sprite = sprite3;
                break;
            case 0:
                renderer.sprite = sprite4;
                Debug.Log("Heart: Player is dead.");
                //GameManager.instance.SetGameState(GameState.IsDead);
                break;
            default:
                Debug.LogWarning("Heart: Health value out of range.");
                break;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage();
        }*/

        health = Mathf.Clamp(health, 0, 3);
    }
    
    [YarnCommand("TakeDamage")]
    public void TakeDamage()
    {
        if (health > 0)
        { 
            // particles
            if (GameObject.FindObjectOfType<LevelManager_Chapel>())
            {
                GameManager.instance.currentLevelManager.GetComponent<LevelManager_Chapel>().PlayHeartbreakParticles();
            }
            
            // screenshake
            if (UIShakeHandler.instance != null)
            {
                UIShakeHandler.instance.ShakeLow();
            }
            else
            {
                Debug.LogWarning("Heart: UIShakeHandler is null. Cannot shake camera.");
            }
            
            // audio feedback
            if (GetComponentInParent<AudioManager>() != null)
            {
                GetComponentInParent<AudioManager>().PlaySFX("WetCrack");
            }
            else
            {
                Debug.LogWarning("Heart: AudioManager is null. Cannot play audio.");
            }
            
            health--;
            Debug.Log($"Heart: Took damage. Current health: {health}");
            GameManager.instance.inMemoryVariableStorage.SetValue("$CurrentHealth", health);
            UpdateHealthUI();
        }
    }

    [YarnCommand("CheckHealth")]
    public void CheckHealth()
    {
        if (health <= 0)
        {
            //DialogueManager.instance.dialogueRunner.Stop();
            Debug.Log("Heart: Player is already dead.");
            
            /*if (!DialogueManager.instance.dialogueRunner.IsDialogueRunning)
            {
                DialogueManager.instance.dialogueRunner.StartDialogue("DeadDialogue");
            }*/
            
            GameManager.instance.SetGameState(GameState.IsDead);
            Debug.Log("Heart: GameState set to IsDead.");
        }
    }
    
    [YarnCommand("Heal")]
    public void Heal()
    {
        if (health < 3)
        {
            if (GameObject.FindObjectOfType<LevelManager_Chapel>())
            {
                GameManager.instance.currentLevelManager.GetComponent<LevelManager_Chapel>()
                    .PlayHeartHealParticles(3-health);
            }

            health++;
            Debug.Log($"Heart: Healed. Current health: {health}");
            GameManager.instance.inMemoryVariableStorage.SetValue("$CurrentHealth", health);
            UpdateHealthUI();
        }
        else
        {
            Debug.Log("Heart: Health is already full.");
        }
    }
}
