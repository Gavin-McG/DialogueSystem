using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public abstract class ChoiceOption : Option
    {
        [HideInDialogueGraph] public OptionParams optionParams;
    }

}
