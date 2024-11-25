using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;


public class ImerisInteraction : MonoBehaviour
{
    // NOTE: THIS IS OLD VERSION OF INTERACTION SCRIPT,
    //       NEW VERSION SEE -- InteractableItemBase.cs
    
    // VARIABLES
    [SerializeField] private bool isOverlapping = false;
    public GameObject overlappedItem;
    public DialogueRunner dialogueRunner;
    
    enum InteractionType
    {
        Dialogue,
        Consume
    }
    
    // ITEM BASE
    private InteractableItemBase interactableItemBase;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOverlapping &&
            (Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E)))
        {
            Debug.Log("Interacting with " + overlappedItem.name);
            interactWithItem();
        }
    }
private void interactWithItem(InteractionType interaction)
    {
        if (interaction == InteractionType.Consume)
        {
            consumeItem();
        }
        else if (interaction == InteractionType.Dialogue)
        {
            interactWithItem();
        }
    }

    private void consumeItem()
    {
        Destroy(overlappedItem);
    }
    
    private void interactWithItem()
    {
        dialogueRunner.StartDialogue(overlappedItem.name);
    }

    // OVERLAPPING WITH INTERACTABLE ITEM
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractableItem"))
        {
            isOverlapping = true;
            overlappedItem = other.gameObject;
            interactableItemBase = overlappedItem.GetComponentInChildren<InteractableItemBase>();
            interactableItemBase.visualCue.SetActive(true);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("InteractableItem"))
        {
            isOverlapping = true;
            overlappedItem = other.gameObject;
            interactableItemBase = overlappedItem.GetComponentInChildren<InteractableItemBase>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableItem"))
        {
            isOverlapping = false;
            overlappedItem = null;
            
            // Set visual cue to false
            interactableItemBase.visualCue.SetActive(false);
            interactableItemBase = null;
        }
    }
}
