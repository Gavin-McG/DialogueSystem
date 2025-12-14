using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Primary Component for operating the Backend of the Dialogue System.
    /// Primary functions are <see cref="BeginDialogue"/> and <see cref="AdvanceDialogue(AdvanceContext)"/>.
    /// <see cref="EndDialogue"/> Is only to be used when ending an interaction prematurely.
    /// Also provides interfaces for values and keywords
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager current;
        
        //Event invoked when dialogue is started
        public readonly UnityEvent StartedDialogue = new();

        [SerializeField, Delayed] private string managerName;
        
        private DialogueAsset _currentDialogue;
        private DialogueObject _startObject;
        private DialogueObject _currentObject;
        private AdvanceContext _previousContext;
        [HideInInspector] public List<int> optionIndexes;

        private void OnValidate()
        {
            if (managerName == "Global")
            {
                managerName = "";
                Debug.LogWarning("DialogueManager name cannot be \"Global\"");
            }
        }

        #region SETTINGS
        
        /// <summary>
        /// Gets type of current Dialogue Settings
        /// </summary>
        public Type GetSettingsType() => _currentDialogue?.settingsData.GetType();

        /// <summary>
        /// Gets the Dialogue Settings for the current dialogue
        /// </summary>
        public SettingsData GetSettings()
        {
            return _currentDialogue?.settingsData;
        }
        
        /// <summary>
        /// Gets the Dialogue Settings for the current dialogue
        /// </summary>
        public T GetSettings<T>() where T : SettingsData
        {
            //null if no dialogue is active
            if (_currentDialogue == null) return null;
            
            var settings = _currentDialogue.settingsData;
            if (settings is T tSettings) return tSettings;
            return null;
        }
        
        #endregion

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
            _startObject = dialogueAsset.GetStartDialogue(startName);
            _currentObject = null;

            StartedDialogue.Invoke();
        }
        
        /// <summary>
        /// Retrieve the next dialogue using context about the user's interaction with strict types.
        /// Returns null if end of dialogue is reached.
        /// </summary>
        public DialogueInfo AdvanceDialogue(AdvanceContext context) {
            current = this;
            _previousContext = context;

            if (_currentDialogue == null)
            {
                Debug.LogWarning("Attempting to Advance Dialogue while no dialogue is active");
                return null;
            }
            
            //Advance until finding a Dialogue Object with output
            do {
                if (!_currentObject) _currentObject = _startObject; //Use start object if current is null
                else _currentObject = _currentObject.GetNextDialogue(context, this);
            } while (_currentObject != null && _currentObject is not IDialogueOutput);

            if (_currentObject is IDialogueOutput outputDialogue)
            {
                var details = outputDialogue.GetDialogueDetails(context, this);
                //TODO replace values in text
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
        }
    }
    

}
