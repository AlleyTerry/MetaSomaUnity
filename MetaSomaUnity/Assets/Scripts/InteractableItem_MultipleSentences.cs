using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemMultipleSentences : InteractableItemBase
{
    [SerializeField] private string continuingDialogueNode;

    protected override void Start()
    {
        base.Start();
        
        isEnterToContinue = true;
    }

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
        if (base.isIndicator) GetComponentInChildren<Animator>().Play("EyeIndicator_BNW_backward");
    }
}
