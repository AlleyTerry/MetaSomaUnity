using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            /*DontDestroyOnLoad(gameObject);*/
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Duplicate GameManager found and destroyed.");
        }
    }
    
    private Animator viewportAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        viewportAnimator = GameManager.instance.HUD?.transform.GetChild(0).GetComponent<Animator>();
        
        if (viewportAnimator == null)
        {
            Debug.LogError("ViewportAnimator not found! Check HUD structure.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
