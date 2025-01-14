using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public string cutSceneDialogueNode;
    
    public string battleDialogueNode;
    public float battleDialogueDelay = 0.5f;
    
    public bool IsTriggered { get; private set; } = false;
    
    public void FlagTriggerUsed()
    {
        IsTriggered = true;
    }
}
