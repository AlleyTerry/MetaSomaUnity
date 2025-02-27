using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    public void TriggerParentFunction()
    {
        ITriggerable parent = GetComponentInParent<ITriggerable>();
        
        if (parent != null)
        {
            parent.OnTriggerAction();
        }
    }
}
