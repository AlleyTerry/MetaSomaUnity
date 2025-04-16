using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : LevelManagerBase
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

    public void EnterLevel()
    {
        DialogueManager.instance.dialogueRunner.StartDialogue("EnterLevel");
    }

}
