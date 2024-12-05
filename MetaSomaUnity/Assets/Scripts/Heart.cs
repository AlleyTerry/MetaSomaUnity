using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [YarnCommand("takeDamage")]
    public void takeDamage()
    {
        Image renderer = GetComponent<Image>();
        //Renderer rend = GetComponent<Renderer>();
        if (renderer != null)
        {
            health--;
            Debug.Log(health);
            variableStorage.SetValue("$CurrentHealth", health);
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
            variableStorage.SetValue("$CurrentHealth", health);
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
        variableStorage.SetValue("$CurrentHealth", health);
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            rend.sprite = sprite1;
        }

        dialogueRunner = GameManager.instance.dialogueRunner;
        variableStorage = GameManager.instance.inMemoryVariableStorage;
    }

    // Update is called once per frame
    void Update()
    {
        variableStorage.SetValue("$CurrentHealth", health);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage();
        }

        health = Mathf.Clamp(health, 0, 3);
        
        // For testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P pressed");
            Image renderer = GetComponent<Image>();
            renderer.sprite = sprite3;
        }
    }
}
