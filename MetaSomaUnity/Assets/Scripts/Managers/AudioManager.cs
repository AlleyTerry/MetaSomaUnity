using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// MUST REQUIRE AN AUDIO SOURCE COMPONENT
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // AUDIO SOURCE COMPONENT
    private AudioSource audioSource;
    
    // AUDIO CLIPS
    public AudioClip linnaeusBattleStart;
    public AudioClip linnaeusBattleEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        // GET AUDIO SOURCE COMPONENT
        audioSource = GetComponent<AudioSource>();
    }

    [YarnCommand ("PlayMusic")]
    // PLAY MUSIC
    public void PlayMusic(string clipName)
    {
        switch (clipName)
        {
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
