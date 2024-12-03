using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomOptionView : OptionView
{
    public void Deselect()
    {
        // Remove the visual indicator that this option is selected.
        // This can involve changing the color, removing highlighting, etc.
        base.OnPointerExit(null);
    }
}
