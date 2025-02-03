using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAS_EffPlay : MonoBehaviour
{
    public AudioClip effAudio;
    [Range(0,1)]
    public float volume = 1;

    public void PlayEff()
    {
        if (!effAudio)
        {
            Debug.LogWarning("MAS_EffPlay: effAudio missing");
            return;
        }
        MAS_Manager.PlaySoundEffect(effAudio, this.transform.position, this.volume);
    }
}
