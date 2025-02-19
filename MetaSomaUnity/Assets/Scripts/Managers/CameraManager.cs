using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Yarn.Unity;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the virtual camera noise component
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float duration)
    {
        noise.m_AmplitudeGain = intensity;
        shakeTime = duration;
    }
    
    // some premade shake functions
    /*[YarnCommand("ShakeLow")]*/
    public void ShakeLow() => Shake(2f, 0.2f);
    public void ShakeStrong() => Shake(5f, 0.5f);

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            if (shakeTime <= 0)
            {
                noise.m_AmplitudeGain = 0f;
            }
        }
    }
}
