using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemMultipleSentences : InteractableItemBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (other.CompareTag("Player"))
        {
            GameManager.instance.FreezeControls();
            
        }
    }
    
    
}
