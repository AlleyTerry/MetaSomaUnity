using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;

public class Heart : MonoBehaviour
{
    public float health = 3;
    [SerializeField] private InMemoryVariableStorage variableStorage;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;

    private DialogueRunner dialogueRunner;
    
    private bool triedReinitializing = false;

    [YarnCommand("takeDamage")]
    public void takeDamage()
    {
        Image renderer = GetComponent<Image>();
        //Renderer rend = GetComponent<Renderer>();
        if (renderer != null)
        {
            health--;
            Debug.Log(health);
            GameManager.instance.inMemoryVariableStorage.SetValue("$CurrentHealth", health);
            if (health == 2)
            {
                renderer.sprite = sprite2;
                
            }
            else if (health == 1)
            {
                renderer.sprite = sprite3;
               
            }
            else if (health == 0)
            {
                renderer.sprite = sprite4;
                
                GameManager.instance.SetGameState(GameState.IsDead);
              
                Debug.Log("u ded");
            }
           
        }
    }
    [YarnCommand("Heal")]
    public void Heal()
    {
        Image renderer = GetComponent<Image>();
        //Renderer rend = GetComponent<Renderer>();
        if (renderer != null)
        {
            health++;
            GameManager.instance.inMemoryVariableStorage.SetValue("$CurrentHealth", health);
            if (health == 2)
            {
                renderer.sprite = sprite2;
                
               
            }
            else if (health == 1)
            {
                renderer.sprite = sprite3;
               
            }
            else if (health == 3)
            {
                renderer.sprite = sprite1;
                Debug.Log("u fully healed");
            }
           
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log($"{GetType().Name} disabled in Menu scene.");
            enabled = false; // disable the script
            return;
        }
        
        StartCoroutine(DelayedSetup());
        
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            rend.sprite = sprite1;
        }
    }

    private IEnumerator DelayedSetup()
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
    }

    public void SetupYarnVariables()
    {
        dialogueRunner = DialogueManager.instance.dialogueRunner;
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
    
    // Update is called once per frame
    void Update()
    {
        if ((dialogueRunner == null || variableStorage == null)/* && 
            !triedReinitializing*/)
        {
            Debug.LogWarning("dialogueRunner or variableStorage is null in Update. Reinitializing...");
            SetupYarnVariables();
            triedReinitializing = true;
        }
        
        if (variableStorage != null)
        {
            variableStorage.SetValue("$CurrentHealth", health);
        }
        
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage();
        }*/

        health = Mathf.Clamp(health, 0, 3);
        
    }
}
