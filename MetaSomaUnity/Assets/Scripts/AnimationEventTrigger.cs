using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    // THIS IS USED FOR CUTSCENE ANIMATION EVENTS
    [SerializeField] public string animationStateName = "";
    [SerializeField] public string dialogueNodeName = "";
    
    public void ChangeAnimationState()
    {
        if (animationStateName == null) return;
        
        Animator animator = GetComponent<Animator>();
        animator.Play(animationStateName);
    }
    
    public void TriggerDialogue()
    {
        if (dialogueNodeName == null) return;
        
        DialogueManager.instance.dialogueRunner.StartDialogue(dialogueNodeName);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
