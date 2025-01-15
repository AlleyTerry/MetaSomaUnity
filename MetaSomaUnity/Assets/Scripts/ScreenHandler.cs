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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered scene transition.");
            
            GameManager.instance.LoadNextLevel();
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
}
