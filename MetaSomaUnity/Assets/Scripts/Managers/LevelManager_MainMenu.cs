using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager_MainMenu : LevelManagerBase
{
    // BUTTONS
    public Button buttonStart;
    public Button buttonQuit;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        // SETUP BUTTONS
        buttonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        buttonQuit = GameObject.Find("ButtonQuit").GetComponent<Button>();
        
        // ADD LISTENERS
        buttonStart.onClick.AddListener(GameManager.instance.LoadNextLevel);
        buttonQuit.onClick.AddListener(QuitGame);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}