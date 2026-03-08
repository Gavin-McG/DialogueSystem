using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    public abstract class RedirectObject : DialogueObject
    {
        [SerializeField] public DialogueObject defaultDialogue;
        [SerializeField] public List<OptionObject> options;
    }
}