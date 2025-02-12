using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    /*public string sceneName;
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
        GameManager.instance.CurrentLevelIndex++;
    }*/
    
    private bool isTriggered = false;

    [SerializeField] private bool isStraightToNextLevel = true;
    [SerializeField] private string nextLevelName = "";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered scene transition.");
            
            isTriggered = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited scene transition.");
            
            isTriggered = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered && 
            Input.GetKeyDown(KeyCode.Return))
        {
            if (isStraightToNextLevel)
            {
                GameManager.instance.LoadNextLevel();
            }
            else
            {
                if (nextLevelName == null) return;
                SetSceneIndex(nextLevelName);
                //SceneManager.LoadScene(nextLevelName);
            }
        }
    }

    private void SetSceneIndex(string sceneName)
    {
        int targetIndex = 0;
        
        switch (nextLevelName)
        {
            case "Level_CommonArea":
                targetIndex = 4;
                break;
            case "Level_Chapel":
                targetIndex = 5;
                break;
            case "Level_Cafeteria":
                targetIndex = 6;
                break;
        }
        
        GameManager.instance.CurrentLevelIndex = targetIndex;
    }
}
