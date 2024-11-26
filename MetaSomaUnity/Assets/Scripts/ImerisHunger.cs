using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImerisHunger : MonoBehaviour
{
    // SINGLETON
    public static ImerisHunger instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    // CHARACTER STATE
    //private CharacterState currentState;
    
    // STATES, IS HUNGER METER RUNNING/NOT
    public bool isHungerMeterTriggered = false;
    
    // HUNGER METER
    private int hungerMeter = 100;
    private int previousHungerMeter = 100; // For telling if hunger meter is increasing or decreasing
    
    public int HungerMeter
    {
        get => hungerMeter;
        set
        { 
            hungerMeter = value;
            
            // Clamp
            hungerMeter = Mathf.Clamp(hungerMeter, 0, 100);

            switch (hungerMeter)
            {
                case <= 50 when previousHungerMeter >= 50:
                    Debug.Log("Imeris is getting hungry...");
                    // todo: change animation
                    break;
                case > 50 when previousHungerMeter <= 50:
                    Debug.Log("Imeris is getting recovered from hunger...");
                    // todo: change animation
                    break;
                case <= 10 when previousHungerMeter >= 10:
                    Debug.Log("Imeris is starving...");
                    ImerisMovement.instance.SetSubState(SubState.Hungry);
                    // todo: change animation and post-process
                    break;
                case > 10 when previousHungerMeter <= 10:
                    Debug.Log("Imeris is recovering from starvation...");
                    ImerisMovement.instance.SetSubState(SubState.Healthy);
                    // todo: change animation and post-process
                break;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StartHungerMeter"))
        {
            isHungerMeterTriggered = true;
            
            InvokeRepeating(nameof(ReduceHunger), 0.5f, 10f);
            Destroy(other.gameObject); // Destroy the object, preventing from re-triggering
        }
    }
    
    public void ReduceHunger()
    {
        previousHungerMeter = hungerMeter;
        HungerMeter -= 10;
        Debug.Log("Current hunger: " + hungerMeter);
    }
    
    public void IncreaseHunger(int amount)
    {
        previousHungerMeter = hungerMeter;
        HungerMeter += amount;
        Debug.Log("Current hunger: " + hungerMeter);
    }
}
