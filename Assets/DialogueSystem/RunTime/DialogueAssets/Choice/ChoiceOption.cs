using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public abstract class ChoiceOption : Option
    {
        [HideInDialogueGraph, SerializeReference] public OptionParams optionParams;
    }

}
