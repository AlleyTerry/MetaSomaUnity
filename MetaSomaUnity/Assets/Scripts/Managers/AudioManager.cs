using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

// MUST REQUIRE AN AUDIO SOURCE COMPONENT
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // INSTANCE
    public static AudioManager instance;
    // AUDIO SOURCE COMPONENT
    [SerializeField] private AudioSource audioSource;
    
    // AUDIO CLIPS
    public AudioClip openingCrawl;

    public AudioClip beeHumming;

    [FormerlySerializedAs("level1")] 
    public AudioClip levelExploration;
    
    public AudioClip linnaeusBattleStart;
    public AudioClip linnaeusBattleEnd;
    public AudioClip Memories;
    public AudioClip OpeningCutscene;
    public AudioClip LevelAmbience;
    public AudioClip EndPrologue;
    
    // AUDIO CLIPS
    public List<AudioClip> sfxClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> sfxDict;
    
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
        
        // POPULATE SFX DICT
        sfxDict = new Dictionary<string, AudioClip>();
        
        foreach (var clip in sfxClips)
        {
            sfxDict.Add(clip.name, clip);
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
                audioSource.clip = levelExploration;
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
            case "Memories":
                audioSource.clip = Memories;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.05f, 0.25f, 0.95f);
                break;
            case "OpeningCutscene":
                audioSource.clip = OpeningCutscene;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.05f, 0.25f, 0.95f);
                break;
            case "LevelAmbience":
                audioSource.clip = LevelAmbience;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.05f, 0.25f, 0.95f);
                break;
            case "EndPrologue":
                audioSource.clip = EndPrologue;
                MAS_Manager.PlayBackgroundMusic(
                    audioSource.clip, 0.05f, 0.25f, 0.95f);
                break;
        }
        
        //audioSource.Play();
    }

    [YarnCommand("PlaySFX")]
    public void PlaySFX(string clipName)
    {
        if (sfxDict.ContainsKey(clipName))
        {
            MAS_Manager.PlaySoundEffect(sfxDict[clipName], transform.position, 1);
        }
    }
    
    [YarnCommand ("StopMusic")]
    // STOP MUSIC
    public void StopMusic(float fadeOutTime)
    {
        //audioSource.Stop();
        MAS_Manager.PlayBackgroundMusic(null, fadeOutTime, 0, 1);
        isMusicPlaying = false;
    }
    
    private AudioSource beeHummingPlayer;

    // adding second audio source for the bee humming
    [YarnCommand("PlayBeeHumming")]
    public void PlayBeeHumming()
    {
        beeHummingPlayer = gameObject.AddComponent<AudioSource>();
        beeHummingPlayer.playOnAwake = false;
        beeHummingPlayer.loop = true;

        beeHummingPlayer.clip = beeHumming;
        StartCoroutine(FadeInRoutine(beeHummingPlayer, beeHumming, 2.25f));
    }
    
    private IEnumerator FadeInRoutine(AudioSource audioSource, AudioClip clip, float duration)
    {
        audioSource.clip = clip;
        audioSource.volume = 0f;
        audioSource.Play();

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    [YarnCommand("StopBeeHumming")]
    public void StopBeeHumming()
    {
        if (beeHummingPlayer != null)
        {
            Debug.Log("Stopping bee humming");
            StartCoroutine(FadeOutRoutine(beeHummingPlayer, 1f));
        }
    }
    
    private IEnumerator FadeOutRoutine(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.Stop();
        Destroy(beeHummingPlayer);
    }
}
