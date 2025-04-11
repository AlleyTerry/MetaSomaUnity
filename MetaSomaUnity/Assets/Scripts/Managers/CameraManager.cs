using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera virtualCameraPanning;
    
    // CINEMACHINE TARGET GROUP
    public Cinemachine.CinemachineTargetGroup targetGroup;
    
    private GameObject tempCameraTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("CameraManager skipped initialization in Menu scene.");
            return;
        }
        
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
        
        if (virtualCameraPanning == null)
        {
            virtualCameraPanning = GameObject.Find("Virtual Camera Panning")?.GetComponent<CinemachineVirtualCamera>();
        }
        
        if (virtualCamera != null) virtualCamera.Priority = 10;
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 5;
        
        if(cinemachineBrain == null)
        {
            cinemachineBrain = GameObject.FindObjectOfType<CinemachineBrain>();
        }
        
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        cinemachineBrain.m_DefaultBlend.m_Time = 1.5f;
    }

    [YarnCommand ("SwitchFollowTarget")]
    public void SwitchFollowTarget()
    {
        Debug.Log("SwitchFollowTarget called.");
        
        if (virtualCamera != null) virtualCamera.Priority = 10;
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 15;
    }

    
    [YarnCommand ("ResetCamera")]
    public void ResetCamera()
    {
        if (virtualCamera != null) virtualCamera.Priority = 20;
        //if (virtualCameraPanning != null) virtualCameraPanning.Priority = 5;
    }
    
    private IEnumerator DelayedResetCamera()
    {
        yield return new WaitForSeconds(0.15f);
        
        if (virtualCamera != null) virtualCamera.Priority = 20;

        yield return new WaitForSeconds(0.1f);
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 5;
    }
}
