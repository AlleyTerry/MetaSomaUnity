using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAS_BgFade : MonoBehaviour
{
    public AudioClip bgAudio;
    [Min(0)]
    public float fadeOutTime = 1;
    [Min(0)]
    public float fadeInTime = 1;
    [Range(0, 1)]
    public float fadeToVolume = 1;
    public bool autoPlay = false;

    public void Awake()
    {
        if (autoPlay)
        {
            this.StartFadeBg();
        }
    }

    public void StartFadeBg()
    {
        //if (!bgAudio)
        //{
        //    Debug.LogWarning("MAS_BgPlay: bgAudio missing");
        //    return;
        //}
        MAS_Manager.PlayBackgroundMusic(bgAudio, fadeOutTime, fadeInTime, fadeToVolume);
    }
}
