using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing the main asset of a DialogueGraph and the first Trace when starting a dialogue
    /// </summary>
    public class DialogueAsset : ScriptableObject, IVariableContext
    {
        [SerializeField] public List<StartObject> startPoints;
        [SerializeField] public VariableContainer variables;

        public DialogueObject GetStartDialogue(string startName)
        {
            return startPoints
                .FirstOrDefault(ds => ds.startName == startName);
        }
        
        //-----------------------------------------------
        //              IVariableContext
        //-----------------------------------------------
        
        public bool IsReadOnly => true;

        public bool TryGetVariable(string name, out Variable variable) =>
            variables.TryGetVariable(name, out variable);

        public void SetVariable(string name, Variable variable) => throw new InvalidOperationException();

        //----Set Methods----

        public void SetString(string name, string value) => throw new InvalidOperationException();
        public void SetFloat(string name, float value) => throw new InvalidOperationException();
        public void SetInt(string name, int value) => throw new InvalidOperationException();
        public void SetBool(string name, bool value) => throw new InvalidOperationException();
        
        //----Get Methods----

        public bool TryGetString(string name, out string value) => variables.TryGetString(name, out value);
        public bool TryGetFloat(string name, out float value) => variables.TryGetFloat(name, out value);
        public bool TryGetInt(string name, out int value) => variables.TryGetInt(name, out value);
        public bool TryGetBool(string name, out bool value) => variables.TryGetBool(name, out value);
    }
}
