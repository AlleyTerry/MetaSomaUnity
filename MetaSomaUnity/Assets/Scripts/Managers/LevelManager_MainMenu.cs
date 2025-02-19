using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager_MainMenu : LevelManagerBase
{
    // BUTTONS
    public Button buttonStart;
    public Button buttonQuit;
    
    public List<Button> buttons = new List<Button>();
    private int currentButtonIndex = 0;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        // SETUP BUTTONS
        buttonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        buttonQuit = GameObject.Find("ButtonQuit").GetComponent<Button>();
        
        buttons.Add(buttonStart);
        buttons.Add(buttonQuit);
        
        SelectButton(currentButtonIndex);
        
        // ADD LISTENERS
        buttonStart.onClick.AddListener(GameManager.instance.LoadNextLevel);
        buttonQuit.onClick.AddListener(QuitGame);
    }

    protected override void Update()
    {
        base.Update();
        
        // Menu navigation
        SwitchSelection();
        ConfirmSelection();
    }

    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        
        // visual highlight
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<Image>().color = i == index ? Color.red : Color.white;
        }
        
        // todo: audio
    }

    private void SwitchSelection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentButtonIndex = (currentButtonIndex - 1 + buttons.Count) % buttons.Count;
            SelectButton(currentButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentButtonIndex = (currentButtonIndex + 1) % buttons.Count;
            SelectButton(currentButtonIndex);
        }
    }
    
    private void ConfirmSelection()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
