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

public class InteractableItemBase : MonoBehaviour
{
    // VISUAL CUE
    public GameObject visualCue;
    
    // PLAYER OVERLAPPING
    [SerializeField] private bool isOverlapping = false;
    public GameObject Imeris;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        if (isOverlapping)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
            {
                //Debug.Log("Interacting with " + this.GameObject().name);
                interactWithItem();
            }
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
            GameManager.instance.FreezeControls();
            DialogueManager.instance.StartDialogue(objName);
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
        if (other.CompareTag("Player"))
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
            /*visualCue.SetActive(false);*/
            
            GetComponentInChildren<Animator>().Play("EyeIndicator_BNW_backward");
        }
    }
}
