using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// MUST REQUIRE AN AUDIO SOURCE COMPONENT
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // AUDIO SOURCE COMPONENT
    [SerializeField] private AudioSource audioSource;
    
    // AUDIO CLIPS
    public AudioClip openingCrawl;

    public AudioClip level1;
    
    public AudioClip linnaeusBattleStart;
    public AudioClip linnaeusBattleEnd;
    
    // MAS MANAGER
    public MAS_Manager masManager;
    [SerializeField] bool isMusicPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // GET AUDIO SOURCE COMPONENT
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // GET MAS MANAGER
        if (masManager == null)
        {
            masManager = GetComponent<MAS_Manager>();
        }
    }

    [YarnCommand ("PlayMusic")]
    // PLAY MUSIC
    public void PlayMusic(string clipName)
    {
        isMusicPlaying = true;
        
        switch (clipName)
        {
            case "openingCrawl":
                audioSource.clip = openingCrawl;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0f, 0.25f, 0.8f);
                break;
            case "level1":
                audioSource.clip = level1;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.1f, 0.25f, 1f);
                break;
            case "linnaeusBattleStart":
                audioSource.clip = linnaeusBattleStart;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.05f, 0.05f, 0.75f);
                break;
            case "linnaeusBattleEnd":
                audioSource.clip = linnaeusBattleEnd;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.05f, 0.25f, 0.95f);
                break;
        }
        
        //audioSource.Play();
    }
    
    [YarnCommand ("StopMusic")]
    // STOP MUSIC
    public void StopMusic(float fadeOutTime)
    {
        //audioSource.Stop();
        MAS_Manager.PlayBackgroundMusic(null, fadeOutTime, 0, 1);
        isMusicPlaying = false;
    }
}
