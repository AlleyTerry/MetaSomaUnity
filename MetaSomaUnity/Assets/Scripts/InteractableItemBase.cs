using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;
using Yarn;

[SerializeField]public enum InteractionType
{
    Dialogue,
    Consume
}
public class InteractableItemBase : MonoBehaviour
{
    // VISUAL CUE
    public GameObject visualCue;
    [SerializeField] private bool isOverlapping = false;
    public GameObject Imeris;
    public DialogueRunner dialogueRunner;
    public string objName;
    public GameObject Item;
    public InteractionType interaction;
 
    // Start is called before the first frame update
    void Start()
    {
        visualCue = transform.GetChild(0).gameObject;
        visualCue.SetActive(false);
        objName = this.gameObject.name;
        Item = this.GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOverlapping &&
            (Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.Return)))
        {
            Debug.Log("Interacting with " + this.GameObject().name);
            interactWithItem();
        }
    }
    
    
    private void interactWithItem()
    {
        if (interaction == InteractionType.Consume)
        {
            ConsumeItem();
        }
        else if (interaction == InteractionType.Dialogue)
        {
            DialogueItem();
        }
    }

    private void ConsumeItem()
    {
        Debug.Log("you consumed " + this.GameObject().name);
        Destroy(Item);
    }
    
    private void DialogueItem()
    {
        Debug.Log("you are talking to " + this.GameObject().name);
        if (!dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(objName);
        }
        
    }
    
    // OVERLAPPING WITH INTERACTABLE ITEM
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = true;
            visualCue.SetActive(true);
        }
 
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("InteractableItem"))
        {
            isOverlapping = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = false;
            // Set visual cue to false
            visualCue.SetActive(false);
        }
    }
}
