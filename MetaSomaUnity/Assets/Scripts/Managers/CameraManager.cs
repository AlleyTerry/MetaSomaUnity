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

    /*[YarnCommand ("PrepSwitchFollowTarget")]
    public void PrepSwitchFollowTarget(GameObject target, float offsetX, float slowDamping)
    {
        Debug.Log("PrepSwitchFollowTarget called.");
        
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
        
        if (tempCameraTarget != null) Destroy(tempCameraTarget);
        
        Vector3 anchor = (target.transform.position + 
                          FindObjectOfType<ImerisMovement>().transform.position) / 2;
        anchor.x += offsetX;
        anchor.y = FindObjectOfType<ImerisMovement>().transform.position.y;
        
        tempCameraTarget = new GameObject("TempCameraTarget");
        tempCameraTarget.transform.SetPositionAndRotation(anchor, Quaternion.identity);
        tempCameraTarget.transform.SetParent(null);
        
        // slowdown the camera movement
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = slowDamping;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = slowDamping;
    }*/

    [YarnCommand ("SwitchFollowTarget")]
    public void SwitchFollowTarget()
    {
        Debug.Log("SwitchFollowTarget called.");
        
        /*
        if (virtualCamera == null)
        {
            virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
        
        virtualCamera.Follow = tempCameraTarget.transform;*/
        
        if (virtualCamera != null) virtualCamera.Priority = 10;
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 15;
    }

    
    [YarnCommand ("ResetCamera")]
    public void ResetCamera()
    {
        /*Debug.Log("ResetCamera called.");

        virtualCamera.Follow = FindObjectOfType<ImerisMovement>().transform;
        
        if (tempCameraTarget != null)
        {
            Destroy(tempCameraTarget);
            tempCameraTarget = null;
        }

        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping =
            GameManager.instance.currentLevelManager.damping;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping =
            GameManager.instance.currentLevelManager.damping;*/

        //StartCoroutine(DelayedResetCamera());
        if (virtualCamera != null) virtualCamera.Priority = 20;
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 5;
    }
    
    private IEnumerator DelayedResetCamera()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (virtualCamera != null) virtualCamera.Priority = 20;

        yield return new WaitForSeconds(0.1f);
        if (virtualCameraPanning != null) virtualCameraPanning.Priority = 5;
    }
}
