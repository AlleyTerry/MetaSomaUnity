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
        
        /*StartCoroutine(DelayedSetup());
        
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            rend.sprite = sprite1;
        }*/
    }

    /*private IEnumerator DelayedSetup()
    {
        yield return new WaitForEndOfFrame();
        
        SetupYarnVariables();
        
        if (variableStorage != null)
        {
            variableStorage.SetValue("$CurrentHealth", health);
            Debug.Log("Initial health set in variableStorage after delay.");
        }
        else
        {
            Debug.LogError("variableStorage is still null after delayed setup!");
        }
    }*/

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
                GameManager.instance.SetGameState(GameState.IsDead);
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
            health--;
            Debug.Log($"Heart: Took damage. Current health: {health}");
            GameManager.instance.inMemoryVariableStorage.SetValue("$CurrentHealth", health);
            UpdateHealthUI();
        }
    }
    
    [YarnCommand("Heal")]
    public void Heal()
    {
        if (health > 3)
        {
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
