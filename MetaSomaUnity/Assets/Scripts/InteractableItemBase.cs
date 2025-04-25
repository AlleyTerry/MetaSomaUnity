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
    
    // IS_JUST_SHOW_ONCE
    [SerializeField] private bool isJustShowOnce = false;
    
    // INDICATOR
    public bool isIndicator = true;
    
    /*[SerializeField] private DialogueRunner dialogueRunner;*/
    
    // BANNING ENTER TO CONTINUE
    public bool isEnterToContinue = false;
    public GameObject lineView;
 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        /*dialogueRunner = GameManager.instance.dialogueRunner;*/
        
        visualCue = transform.GetChild(0).gameObject;
        visualCue.SetActive(false);
        
        objName = this.gameObject.name;
        interactableItem = this.GameObject();

        // Keeping the visual cue behind everything for debugging purposes
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -10;

        isEnterToContinue = false;
        lineView = GameObject.FindObjectOfType<LineView>().gameObject;
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
            if (isIndicator) visualCue.SetActive(true);

            if (!isEnterToContinue)
            {
                lineView.GetComponent<DialogueAdvanceInput>().enabled = false;
            }
            else
            {
                lineView.GetComponent<DialogueAdvanceInput>().enabled = true;
            }
        }
    }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = true;
            
            if (!isEnterToContinue)
            {
                lineView.GetComponent<DialogueAdvanceInput>().enabled = false;
            }
            else
            {
                lineView.GetComponent<DialogueAdvanceInput>().enabled = true;
            }
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOverlapping = false;
            // Set visual cue to false
            /*visualCue.SetActive(false);*/
            
            if (isIndicator) GetComponentInChildren<Animator>().Play("EyeIndicator_BNW_backward");
            
            if (DialogueManager.instance.dialogueRunner.IsDialogueRunning)
            {
                DialogueManager.instance.StopDialogue();
            }

            Invoke(nameof(DisableTrigger), 0.5f);
            
            lineView.GetComponent<DialogueAdvanceInput>().enabled = true;
            
        }
    }

    protected void DisableTrigger()
    {
        if (isJustShowOnce)
        {
            // disable the trigger
            GetComponent<BoxCollider>().enabled = false;
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
