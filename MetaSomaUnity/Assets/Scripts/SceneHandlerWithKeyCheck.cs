using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SceneHandlerWithKeyCheck : SceneHandler
{
    // The dialogue node to display when the player is NOT holding the key
    [SerializeField] private string reminderDialogueNode = "";  // BoardedChapel

    protected override void Update()
    {
        if (base.isTriggering && 
            Input.GetKeyDown(KeyCode.Return) &&
            !DialogueManager.instance.dialogueRunner.IsDialogueRunning)
        {
            if (GameManager.instance.isHoldingChapelKey)
            {
                if (isStraightToNextLevel)
                {
                    GameManager.instance.LoadNextLevel();
                }
                else
                {
                    if (nextLevelName == null) return;
                    SetSceneIndex(nextLevelName);
                    //SceneManager.LoadScene(nextLevelName);
                }
            }
            else
            {
                DialogueManager.instance.dialogueRunner.StartDialogue(reminderDialogueNode);
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited scene transition.");
            
            isTriggering = false;

            if (DialogueManager.instance.dialogueRunner.IsDialogueRunning)
            {
                DialogueManager.instance.dialogueRunner.Stop();
            }
        }
    }

    [YarnCommand("PickUpKey")]
    public void PickUpKey()
    {
        GameManager.instance.isHoldingChapelKey = true;
    }
}
