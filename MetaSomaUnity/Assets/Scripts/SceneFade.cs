using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    // Reference to the Image component used for fading
    public Image fadeImage;
    // Duration of the fade effect
    public float fadeDuration = 1f;
    // Timer to track the fade progress
    private float fadeTimer = 0f;
    // Flags to indicate if fading in or out
    private bool isFadingIn = true;
    private bool isFadingOut = false;
    // Index of the scene to load
    private int sceneToLoad;

    private void Start()
    {
        // Initialize with a fully opaque image for fade-in effect
        StartFadeIn();
    }

    private void Update()
    {
        // If not fading in or out, do nothing, saving performance
        if (!isFadingIn && !isFadingOut) return;
        
        // Handle fade-in effect
        if (isFadingIn)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (fadeTimer / fadeDuration));
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            // Check if fade-in is complete
            if (fadeTimer >= fadeDuration)
            {
                isFadingIn = false;
            }
        }
        // Handle fade-out effect
        else if (isFadingOut)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            // Check if fade-out is complete and load the new scene
            if (fadeTimer >= fadeDuration)
            {
                isFadingOut = false;
                SceneManager.LoadSceneAsync(sceneToLoad);
            }
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} Loaded! Fading In...");
        StartFadeIn();
    }


    // Method to initiate the fade-out effect and load a new scene
    public void LoadScene(int sceneIndex)
    {
        sceneToLoad = sceneIndex;
        isFadingOut = true;
        fadeTimer = 0f;
    }

    public void StartFadeIn()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        isFadingIn = true;
        fadeTimer = 0f;
    }
}