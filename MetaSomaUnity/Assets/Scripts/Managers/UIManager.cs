using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Duplicate UI Manager found and destroyed.");
        }
    }
    
    [SerializeField] private Animator viewportAnimator;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log($"{GetType().Name} disabled in Menu scene.");
            //enabled = false; // disable the script
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("DialogueManager skipped initialization in Menu scene.");
            return;
        }
        
        viewportAnimator = GameManager.instance.HUD?.transform.GetChild(0).GetComponent<Animator>();
        
        if (viewportAnimator == null)
        {
            Debug.LogError("ViewportAnimator not found! Check HUD structure.");
        }
    }
    
    public void PlayAnimation(string animationName)
    {
        if (CheckViewportAnimator())
        {
            viewportAnimator.Play(animationName);
        }
    }
    
    public void EnableAnimator()
    {
        if (CheckViewportAnimator())
        {
            viewportAnimator.enabled = true;
        }
    }
    
    public void DisableAnimator()
    {
        //Debug.Log("Disabling viewport animator!!!!!!!!!!");
        if (CheckViewportAnimator())
        {
            viewportAnimator.enabled = false;
        }
    }
    
    private bool CheckViewportAnimator()
    {
        if (viewportAnimator == null)
        {
            viewportAnimator = GameManager.instance.HUD?.transform.GetChild(0).GetComponent<Animator>();

            if (viewportAnimator == null)
            {
                Debug.LogError("ViewportAnimator is null! Check HUD structure.");
                return false;
            }
        }
        return true;
    }
    
    
    
}
