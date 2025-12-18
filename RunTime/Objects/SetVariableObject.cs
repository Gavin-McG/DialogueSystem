
using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    public class SetVariableObject : DialogueObject
    {
        [SerializeField] public DialogueObject nextDialogue;
        [SerializeField] public string variableName;
        [SerializeField] public Variable variable;

        public override DialogueObject GetNextDialogue(AdvanceContext context, DialogueManager manager)
        {
            manager.SetVariable(variableName, variable);
            return nextDialogue;
        }
    }
}
