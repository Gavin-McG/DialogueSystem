using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Primary Component for operating the Backend of the Dialogue System.
    /// Primary functions are <see cref="BeginDialogue"/> and <see cref="AdvanceDialogue(AdvanceContext)"/>.
    /// <see cref="EndDialogue"/> Is only to be used when ending an interaction prematurely.
    /// Also provides interfaces for values and keywords
    /// </summary>
    public class DialogueManager : MonoBehaviour, IVariableContext
    {
        //Event invoked when dialogue is started
        public readonly UnityEvent StartedDialogue = new();
        
        [SerializeField] private DialogueAsset _currentDialogue;
        [SerializeField] private DialogueObject _currentObject;
        private DialogueParameters _currentParameters;
        
        private AdvanceContext _previousContext;
        [HideInInspector] public List<int> optionIndexes;

        [SerializeField] private VariableContainer variables = new ();
        
        /// <summary>
        /// Gets the Dialogue Settings for the current dialogue
        /// </summary>
        public T GetDialogueParams<T>() where T : DialogueParameters
        {
            //null if no dialogue is active
            if (_currentParameters == null) return null;
            
            if (_currentParameters is T tParams) return tParams;
            return null;
        }
        
        /// <summary>
        /// Gets type of current Dialogue Settings
        /// </summary>
        public Type GetDialogueParamsType() => _currentParameters?.GetType();
        
        /// <summary>
        /// Begin a dialogue using the DialogueAsset to be started
        /// </summary>
        public void BeginDialogue(DialogueAsset dialogueAsset, string startName="")
        {
            if (_currentDialogue != null)
            {
                Debug.LogWarning($"Attempted to begin dialogue \"{dialogueAsset.name}\" while dialogue was already playing");
                return;
            }
            
            _currentDialogue = dialogueAsset;
            _currentObject = dialogueAsset.GetStartDialogue(startName);

            StartedDialogue.Invoke();
        }
        
        /// <summary>
        /// Retrieve the next dialogue using context about the user's interaction with strict types.
        /// Returns null if end of dialogue is reached.
        /// </summary>
        public DialogueInfo AdvanceDialogue(AdvanceContext context) {
            _previousContext = context;

            if (_currentDialogue == null)
            {
                Debug.LogWarning("Attempting to Advance Dialogue while no dialogue is active");
                return null;
            }
            
            //Advance until finding a Dialogue Object with output
            do {
                _currentObject = _currentObject.GetNextDialogue(context, this);
            } while (_currentObject != null && _currentObject is not IDialogueOutput);

            if (_currentObject is IDialogueOutput outputDialogue)
            {
                var details = outputDialogue.GetDialogueDetails(context, this);
                details.ApplyVariables(this);
                return details;
            }
            
            EndDialogue();
            return null;
        }
        
        /// <summary>
        /// Retrieve the next dialogue with strict types using default context.
        /// Primarily used to get the first dialogue.
        /// Returns null if end of dialogue is reached.
        /// </summary>
        public DialogueInfo AdvanceDialogue()
        => AdvanceDialogue(new AdvanceContext());

        /// <summary>
        /// Retrieve the information about the current dialogue again.
        /// Used if you want to account for changes in conditional choice options
        /// </summary>
        public DialogueInfo RefreshDialogue()
        {
            if (_currentDialogue != null)
            {
                throw new Exception($"Attempted to refresh dialogue while dialogue was not playing");
            }

            var outputDialogue = (IDialogueOutput)_currentObject;
            var details = outputDialogue.GetDialogueDetails(_previousContext, this);
            //TODO replace values in text
            return details;
        }
        
        /// <summary>
        /// Prematurely end dialogue
        /// </summary>
        public void EndDialogue()
        {
            if (_currentDialogue == null) return;
            _currentDialogue = null;
            _currentObject = null;
            _currentParameters = null;
        }
        
        //-----------------------------------------------
        //           IVariableContext Implementation
        //-----------------------------------------------

        public bool IsReadOnly => false;

        public bool TryGetVariable(string name, out Variable variable) =>
             variables.TryGetVariable(name, out variable) || 
             (_currentDialogue?.TryGetVariable(name, out variable) ?? true);
        
        //----Set Methods----

        public void SetString(string name, string value) => variables.SetString(name, value);
        public void SetFloat(string name, float value) => variables.SetFloat(name, value);
        public void SetInt(string name, int value) => variables.SetInt(name, value);
        public void SetBool(string name, bool value) => variables.SetBool(name, value);
        
        //----Get Methods----

        public string GetString(string name)
        {
            if (variables.TryGetVariable(name, out Variable variable))
                return variable.GetString();
            if (_currentDialogue?.TryGetVariable(name, out variable) ?? false)
                return variable.GetString();
            
            Debug.LogWarning($"No Variable found with name {name}");
            return null;
        }
        
        public float GetFloat(string name) {
            if (variables.TryGetVariable(name, out Variable variable))
                return variable.GetFloat();
            if (_currentDialogue?.TryGetVariable(name, out variable) ?? false)
                return variable.GetFloat();
            
            Debug.LogWarning($"No Variable found with name {name}");
            return 0f;
        }
        
        public int GetInt(string name)
        {
            if (variables.TryGetVariable(name, out Variable variable))
                return variable.GetInt();
            if (_currentDialogue?.TryGetVariable(name, out variable) ?? false)
                return variable.GetInt();
            
            Debug.LogWarning($"No Variable found with name {name}");
            return 0;
        }
        
        public bool GetBool(string name)
        {
            if (variables.TryGetVariable(name, out Variable variable)) 
                return variable.GetBool();
            if (_currentDialogue?.TryGetVariable(name, out variable) ?? false) 
                return variable.GetBool();
            
            Debug.LogWarning($"No Variable found with name {name}");
            return false;
        }
    }
}
