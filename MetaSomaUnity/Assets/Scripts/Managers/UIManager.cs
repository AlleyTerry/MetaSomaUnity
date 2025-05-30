using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject particleDisplay;
    public RawImage particleImage;
    public float alphaParticlesOn = 255f;
    public float alphaParticlesOff = 0.0f;
    
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
        
        //particleImage = GameObject.Find("ParticlesDisplay").GetComponent<RawImage>();
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
    
    [YarnCommand("PlayViewportAnimation")]
    public void PlayViewportAnimation(string animationName)
    {
        EnableAnimator();
        viewportAnimator.Play(animationName);
    }
    
    [YarnCommand("EnableParticles")]
    public void EnableParticles()
    {
        if (particleImage != null)
        {
            particleImage.color = new Color(particleImage.color.r, particleImage.color.g, particleImage.color.b, alphaParticlesOn);
        }
        else
        {
            Debug.LogError("Particle image is null! Check assignment.");
        }
    }
    
    [YarnCommand("DisableParticles")]
    public void DisableParticles()
    {
        particleDisplay = GameObject.Find("ParticlesDisplay");
        particleImage = particleDisplay.GetComponent<RawImage>();
        if (particleDisplay == null)
        {
            Debug.LogError("Particle display is null! Check assignment.");
            return;
        }
        if (particleImage != null)
        {
            Debug.Log("Disabling particles");
            particleImage.color = new Color(particleImage.color.r, particleImage.color.g, particleImage.color.b, alphaParticlesOff);
        }
        else
        {
            Debug.LogError("Particle image is null! Check assignment.");
        }
    }
}
