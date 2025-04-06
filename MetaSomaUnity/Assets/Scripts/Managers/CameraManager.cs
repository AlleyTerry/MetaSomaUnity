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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("Duplicate CameraManager found and destroyed.");
        }
    }
    
    // VIRTUAL CAMERA
    public CinemachineVirtualCamera virtualCamera;
    
    // CINEMACHINE TARGET GROUP
    public Cinemachine.CinemachineTargetGroup targetGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [YarnCommand ("SwitchFollowTarget")]
    public void SwitchFollowTarget()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
        
        /*if (targetGroup == null)
        {
            targetGroup = GameObject.FindObjectOfType<Cinemachine.CinemachineTargetGroup>();
        }
        
        if (targetGroup == null)
        {
            Debug.LogError("Cinemachine Target Group not found!");
            return;
        }
        else
        {
            virtualCamera.Follow = targetGroup.transform;
            virtualCamera.m_Lens.OrthographicSize = 4.03f;
            Camera.main.orthographicSize = 4.03f;
        }*/
        
        
    }
}
