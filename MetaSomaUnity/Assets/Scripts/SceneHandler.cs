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
    
    protected bool isTriggering = false;

    [SerializeField] protected bool isStraightToNextLevel = true;
    [SerializeField] protected string nextLevelName = "";
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered scene transition.");
            
            isTriggering = true;
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited scene transition.");
            
            isTriggering = false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isTriggering && 
            Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.FreezeControls();
            
            Invoke(nameof(DestroyThis), 0.15f);
            
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

    protected void SetSceneIndex(string sceneName)
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
    
    protected void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
