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
    
    // HUNGER METER
    private int hungerMeter = 100;
    private int previousHungerMeter = 100; // For telling if hunger meter is increasing or decreasing
    
    public int HungerMeter
    {
        get => hungerMeter;
        set
        { 
            hungerMeter = value;
            
            
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
            InvokeRepeating(nameof(ReduceHunger), 0.5f, 10f);
        }
    }
    
    public void ReduceHunger()
    {
        previousHungerMeter = hungerMeter;
        HungerMeter -= 10;
        Debug.Log("Current hunger: " + hungerMeter);
    }
    
    public void IncreaseHunger()
    {
        HungerMeter += 10;
        Debug.Log("Current hunger: " + hungerMeter);
    }
}
