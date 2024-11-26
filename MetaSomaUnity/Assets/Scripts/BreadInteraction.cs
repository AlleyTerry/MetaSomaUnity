using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadInteraction : InteractableItemBase
{
    protected override void interactWithItem()
    {
        if (interactionType == InteractionType.Consume)
        {
            ConsumeItem();
        }
    }

    protected override void ConsumeItem()
    {
        ImerisHunger.instance.IncreaseHunger(5);
        base.ConsumeItem();
    }
}
