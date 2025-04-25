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
    
    // CREDIT PAGE
    public GameObject creditPage;
    [SerializeField] private bool isCreditPageActive = false;
    
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
        
        //SetUpCreditPage();
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

    private void SetUpCreditPage()
    {
        // set up the credit page
        creditPage = GameObject.Find("CreditPageCanvas");
        if (creditPage != null)
        {
            Debug.Log("Credit page found.");
            creditPage.SetActive(false);
            isCreditPageActive = false;
        }
        else
        {
            Debug.LogError("Credit page not found.");
        }
    }

    public override void Initialize()
    {
        Debug.Log("initialize main menu");
        SetButton();
        
        SetUpCreditPage();
    }

    protected override void Update()
    {
        if (isCreditPageActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                creditPage.SetActive(false);
                isCreditPageActive = false;
                
                currentButtonIndex = 0;
                SelectButton(currentButtonIndex);
            }
        }
        else
        {
            base.Update();
        
            // Menu navigation
            SwitchSelection();
            ConfirmSelection();
        }
    }

    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        
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
            if (currentButtonIndex != 0)
            {
                hasSelected = true;
            }
            
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
                
                if (creditPage != null)
                {
                    creditPage.SetActive(true);
                    isCreditPageActive = true;
                }
                else
                {
                    Debug.LogError("Credit page not found.");
                }
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
