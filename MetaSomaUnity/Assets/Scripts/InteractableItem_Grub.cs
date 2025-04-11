using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class InteractableItem_Grub : InteractableItemBase
{
    [SerializeField] private string continuingDialogueNode;
    [SerializeField] private YarnCharacter imeris;
    [SerializeField] private YarnCharacter innerImeris;
    
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
        
        imeris = GameObject.Find("ImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        innerImeris = GameObject.Find("InnerImerisWorldspacePlaceholder").GetComponent<YarnCharacter>();
        
        imeris.messageBubbleOffset.x += 2f;
        innerImeris.messageBubbleOffset.x += 2f;
    }

    private IEnumerator SwitchCamera()
    {
        CameraManager.instance.SwitchFollowTarget();
        
        // Wait for the camera to stop before trigger the second dialogue
        yield return new WaitForSeconds(1.75f);
        DialogueManager.instance.dialogueRunner.transform.GetComponent<YarnCharacterView>().UpdateBubblePosition();
        DialogueManager.instance.StartDialogue(continuingDialogueNode);
    }
}
