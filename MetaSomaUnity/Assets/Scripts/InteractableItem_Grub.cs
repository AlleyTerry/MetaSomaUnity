using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InteractableItem_Grub : InteractableItemBase
{
    [SerializeField] private string continuingDialogueNode;
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (other.CompareTag("Player"))
        {
            GameManager.instance.FreezeControls();
            StartCoroutine(DisableCollider());
        }
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.95f);
        gameObject.GetComponent<Collider>().enabled = false; // DISABLE COLLIDER
        isOverlapping = false;
        GetComponentInChildren<Animator>().Play("EyeIndicator_BNW_backward");
    }

    [YarnCommand ("SwitchCameraToGrub")]
    public void SwitchCameraToGrub()
    {
        StartCoroutine(SwitchCamera());
    }

    private IEnumerator SwitchCamera()
    {
        GameObject target = GameObject.Find("DeadGrub");

        if (target == null)
        {
            Debug.LogError("DeadGrub not found in scene.");
            yield break;
        }
        else
        {
            CameraManager.instance.PrepSwitchFollowTarget(target, 0.25f, 3.5f);
        }
        
        yield return new WaitForSeconds(0.65f);
        
        CameraManager.instance.SwitchFollowTarget();
        
        // Wait for the camera to stop before trigger the second dialogue
        yield return new WaitForSeconds(0.75f);
        DialogueManager.instance.StartDialogue(continuingDialogueNode);
    }
}
