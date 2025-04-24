using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Yarn.Unity.Example;

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
        if (virtualCamera == null) Debug.LogWarning("VirtualCamera not found.");
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 15;
        if (virtualCameraPanning == null) Debug.LogWarning("VirtualCameraPanning not found.");
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

    [YarnCommand ("ResetDialogueBoxOffset")]
    public void ResetDialogueBoxOffset()
    {
        YarnCharacter imeris = GameObject.Find("ImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        YarnCharacter innerImeris = GameObject.Find("InnerImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        
        YarnCharacter grub = GameObject.Find("GrubWorldspacePlaceholder")?.GetComponent<YarnCharacter>();
        
        imeris.messageBubbleOffset.x -= 2f;
        innerImeris.messageBubbleOffset.x -= 2.3f;
        
        if (grub != null) grub.messageBubbleOffset.x -= 2f;
    }
}
