using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenHandler : MonoBehaviour
{
    /*public string sceneName;
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
        GameManager.instance.CurrentLevelIndex++;
    }*/
    
    private bool isTriggered = false;
    
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
            GameManager.instance.LoadNextLevel();
        }
    }
}
