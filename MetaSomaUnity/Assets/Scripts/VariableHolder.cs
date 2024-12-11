using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;

public class VariableHolder : MonoBehaviour
{
    public static VariableHolder instance;

    private GameObject heart;
    public static float health = 3;

    public float Health
    {
        get => health;
        set
        {
            Health = value;
            heart.GetComponent<Heart>().health = value;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    [YarnCommand("GetHealth")]
    public float GetHealth()
    {
        return health;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        heart = GameObject.Find("Heart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
