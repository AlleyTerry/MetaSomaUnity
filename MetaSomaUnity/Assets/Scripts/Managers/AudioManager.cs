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
    
    
    // Start is called before the first frame update
    void Start()
    {
        // GET AUDIO SOURCE COMPONENT
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    [YarnCommand ("PlayMusic")]
    // PLAY MUSIC
    public void PlayMusic(string clipName)
    {
        switch (clipName)
        {
            case "openingCrawl":
                audioSource.clip = openingCrawl;
                break;
            case "level1":
                audioSource.clip = level1;
                break;
            case "linnaeusBattleStart":
                audioSource.clip = linnaeusBattleStart;
                break;
            case "linnaeusBattleEnd":
                audioSource.clip = linnaeusBattleEnd;
                break;
        }
        audioSource.Play();
    }
    
    [YarnCommand ("StopMusic")]
    // STOP MUSIC
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
