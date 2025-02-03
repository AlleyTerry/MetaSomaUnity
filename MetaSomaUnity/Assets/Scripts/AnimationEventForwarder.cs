using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    public void TriggerParentFunction()
    {
        InteractableItemBase parent = GetComponentInParent<InteractableItemBase>();
        
        if (parent != null)
        {
            parent.HideIndicator();
        }
    }
}
