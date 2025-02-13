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
        base.Update();

        if (base.isTriggered && 
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
            else if (!GameManager.instance.isHoldingChapelKey)
            {
                DialogueManager.instance.dialogueRunner.StartDialogue(reminderDialogueNode);
            }
        }
    }

    [YarnCommand("PickUpKey")]
    public void PickUpKey()
    {
        GameManager.instance.isHoldingChapelKey = true;
    }
}
