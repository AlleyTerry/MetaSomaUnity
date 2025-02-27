using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;
using Yarn;

public enum InteractionType
{
    Dialogue,
    Consume
}

public class InteractableItemBase : MonoBehaviour, ITriggerable
{
    // VISUAL CUE
    public GameObject visualCue;
    
    // PLAYER OVERLAPPING
    [SerializeField] protected bool isOverlapping = false;
    
    // ITEM VARIABLES
    private GameObject interactableItem;
    public string objName;
    public InteractionType interactionType;
    
    /*[SerializeField] private DialogueRunner dialogueRunner;*/
 
    // Start is called before the first frame update
    void Start()
    {
        /*dialogueRunner = GameManager.instance.dialogueRunner;*/
        
        visualCue = transform.GetChild(0).gameObject;
        visualCue.SetActive(false);
        
        objName = this.gameObject.name;
        interactableItem = this.GameObject();

        // Keeping the visual cue behind everything for debugging purposes
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -10;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isOverlapping)
        {
            // Older version, need player to press F or Enter
            /*if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
            {
                //Debug.Log("Interacting with " + this.GameObject().name);
                interactWithItem();
            }*/
            
            // Newer version, triggering automatically
            interactWithItem();
        }
    }
    
    public void HideIndicator()
    {
        visualCue.SetActive(false);
    }
    
    protected virtual void interactWithItem()
    {
        if (interactionType == InteractionType.Consume)
        {
            ConsumeItem();
        }
        else if (interactionType == InteractionType.Dialogue)
        {
            DialogueItem();
        }
    }

    protected virtual void ConsumeItem()
    {
        Debug.Log("you consumed " + this.GameObject().name);
        Destroy(interactableItem);
    }
    
    protected virtual void DialogueItem()
    {
        //Debug.Log("you are talking to " + this.GameObject().name);
        
        if (!DialogueManager.instance.dialogueRunner.IsDialogueRunning)
        {
            /*GameManager.instance.FreezeControls();*/
            DialogueManager.instance.StartDialogue(objName);
        }
    }
    
    // OVERLAPPING WITH INTERACTABLE ITEM
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = true;
            visualCue.SetActive(true);
        }
    }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = true;
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = false;
            // Set visual cue to false
            /*visualCue.SetActive(false);*/
            
            GetComponentInChildren<Animator>().Play("EyeIndicator_BNW_backward");
            
            if (DialogueManager.instance.dialogueRunner.IsDialogueRunning)
            {
                DialogueManager.instance.StopDialogue();
            }
        }
    }

    public void OnTriggerAction()
    {
        HideIndicator();
    }
}

public interface ITriggerable
{
    void OnTriggerAction();
}
