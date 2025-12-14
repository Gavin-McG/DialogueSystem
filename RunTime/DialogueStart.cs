using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    [Serializable]
    public class DialogueStart
    {
        [SerializeField] public DialogueObject startDialogue;
        [SerializeField] public string startName;
    }
}
