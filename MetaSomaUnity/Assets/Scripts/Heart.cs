using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;

public class Heart : MonoBehaviour
{
    public float health = 3;
    public InMemoryVariableStorage variableStorage;
    public Color color1 = new Color(1, 0, 0);
    public Color color2 = new Color(1, 0.5f, 0);
    public Color color3 = new Color(1, 1, 0);
    public DialogueRunner dialogueRunner;

    [YarnCommand("takeDamage")]
    public void takeDamage()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            health--;
            if (health == 2)
            {
                rend.material.color = color2;
            }
            else if (health == 1)
            {
                rend.material.color = color3;
            }
            else if (health == 0)
            {
                rend.material.color = Color.black;
                Debug.Log("u ded");
            }
           
        }
    }
    [YarnCommand("Heal")]
    public void Heal()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            health++;
            if (health == 2)
            {
                rend.material.color = color2;
            }
            else if (health == 1)
            {
                rend.material.color = color3;
            }
            else if (health == 3)
            {
                rend.material.color = color1;
                Debug.Log("u fully healed");
            }
           
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        variableStorage.SetValue("$currentHealth", health);
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = color1;
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
