using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;

public class Heart : MonoBehaviour
{
    public float health = 3;
    public InMemoryVariableStorage variableStorage;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Color color1 = new Color(1, 0, 0);
    public Color color2 = new Color(1, 0.5f, 0);
    public Color color3 = new Color(1, 1, 0);
    public DialogueRunner dialogueRunner;

    [YarnCommand("takeDamage")]
    public void takeDamage()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        //Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            health--;
            Debug.Log(health);
            if (health == 2)
            {
                rend.sprite = sprite2;
                
            }
            else if (health == 1)
            {
                rend.sprite = sprite3;
               
            }
            else if (health == 0)
            {
                rend.sprite = sprite4;
              
                Debug.Log("u ded");
            }
           
        }
    }
    [YarnCommand("Heal")]
    public void Heal()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        //Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            health++;
            if (health == 2)
            {
                rend.sprite = sprite2;
               
            }
            else if (health == 1)
            {
                rend.sprite = sprite3;
               
            }
            else if (health == 3)
            {
                rend.sprite = sprite1;
                Debug.Log("u fully healed");
            }
           
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        variableStorage.SetValue("$currentHealth", health);
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            rend.sprite = sprite1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        variableStorage.SetValue("$currentHealth", health);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage();
        }

        health = Mathf.Clamp(health, 0, 3);
    }
}
