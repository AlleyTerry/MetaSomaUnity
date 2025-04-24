using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager_MainMenu : LevelManagerBase
{
    // BUTTONS
    public Button buttonStart;
    public Button buttonQuit;
    public Button buttonCredit;
    
    public List<Button> buttons = new List<Button>();
    [SerializeField] private int currentButtonIndex = 0;
    
    bool hasSelected = false;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        SetButton();
        //play background music
        gameObject.GetComponent<AudioManager>().PlayMusic("Memories");
        // ADD LISTENERS
        //buttonStart.onClick.AddListener(GameManager.instance.LoadNextLevel);
        //buttonQuit.onClick.AddListener(QuitGame);
    }

    private void SetButton()
    {
        // SETUP BUTTONS
        buttonCredit = GameObject.Find("ButtonCredit").GetComponent<Button>();
        buttonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        buttonQuit = GameObject.Find("ButtonQuit").GetComponent<Button>();
        
        // Set up buttons
        buttons.Add(buttonCredit);
        buttons.Add(buttonStart);
        buttons.Add(buttonQuit);
        
        // set start button to be the default selected button
        currentButtonIndex = 1;
        SelectButton(currentButtonIndex);
    }

    public override void Initialize()
    {
        SetButton();
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
        
        /*// visual highlight
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<Image>().color = i == index ? Color.red : Color.white;
        }*/
        
        // todo: audio
    }

    private void SwitchSelection()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) 
            && !hasSelected)
        {
            currentButtonIndex = (currentButtonIndex - 1 + buttons.Count) % buttons.Count;
            SelectButton(currentButtonIndex);
            HighlightButtonText(currentButtonIndex);
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)) 
                 && !hasSelected)
        {
            currentButtonIndex = (currentButtonIndex + 1) % buttons.Count;
            SelectButton(currentButtonIndex);
            HighlightButtonText(currentButtonIndex);
        }
    }
    
    // set button text to be highlighted when on selected
    private void HighlightButtonText(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == index)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = 
                    new Color32(244, 0, 24, 255);
            }
            else
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = 
                    new Color32(184, 30, 30, 255);
            }
        }
    }
    
    private void ConfirmSelection()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !hasSelected)
        {
            hasSelected = true;
            
            Button selectedButton = buttons[currentButtonIndex];
            selectedButton.interactable = false;

            if (selectedButton == buttonStart)
            {
                GameManager.instance.LoadNextLevel();
            }
            else if (selectedButton == buttonQuit)
            {
                QuitGame();
            }
            else if (selectedButton == buttonCredit)
            {
                Debug.Log("Credit button clicked.");
                // todo: implement credit functionality
            }
            else
            {
                Debug.LogError("No action assigned to this button.");
            }
        }
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }
}
