using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomLineView : LineView
{
    public string speakerName;

    public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
    {
        base.InterruptLine(dialogueLine, onInterruptLineFinished);

        speakerName = dialogueLine.CharacterName;
    }

    public string GetSpeakerName()
    {
        return speakerName;
    }
}
