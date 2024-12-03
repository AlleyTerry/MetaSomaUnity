using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager instance;
    
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
    public int hungerMeter = 100;
    
    // GAME STATE
    public bool isInBattle = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FreezeControls()
    {
        isInBattle = true;
        Time.timeScale = 0;
    }
    
    public void ResumeControls()
    {
        isInBattle = false;
        Time.timeScale = 1;
    }
}
