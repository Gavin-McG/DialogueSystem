using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WolverineSoft.DialogueSystem.Keywords;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Primary Component for operating the Backend of the Dialogue System.
    /// Primary functions are <see cref="BeginDialogue"/> and <see cref="AdvanceDialogue(AdvanceDialogueContext)"/>.
    /// <see cref="EndDialogue"/> Is only to be used when ending an interaction prematurely.
    /// Also provides interfaces for values and keywords
    /// </summary>
    public sealed class DialogueManager : MonoBehaviour, IKeywordContext, IValueContext
    {
        public static DialogueManager current;
        
        //Event invoked when dialogue is started
        public readonly UnityEvent StartedDialogue = new();
        
        private readonly KeywordContext _keywordContext = new KeywordContext();

        private DialogueAsset _currentDialogue;
        private DialogueTrace _currentTrace;
        private AdvanceDialogueContext _previousContext;
        [HideInInspector] public List<int> optionIndexes;
        
        #region KEYWORDS
        public void DefineKeyword(string keyword, KeywordScope scope) => _keywordContext.DefineKeyword(keyword, scope);
        public void UndefineKeyword(string keyword, KeywordScope scope) => _keywordContext.UndefineKeyword(keyword, scope);
        public bool IsKeywordDefined(string keyword) => _keywordContext.IsKeywordDefined(keyword);
        public void ClearKeywords(KeywordScope scope) => _keywordContext.ClearKeywords(scope);
        #endregion
        
        #region SETTINGS
        
        /// <summary>
        /// Gets type of current Dialogue Settings
        /// </summary>
        public Type GetSettingsType() => _currentDialogue?.settings.GetType();

        /// <summary>
        /// Gets the Dialogue Settings for the current dialogue
        /// </summary>
        public DialogueSettings GetSettings()
        {
            return _currentDialogue?.settings;
        }
        
        /// <summary>
        /// Gets the Dialogue Settings for the current dialogue
        /// </summary>
        public T GetSettings<T>() where T : DialogueSettings
        {
            //null if no dialogue is active
            if (_currentDialogue == null) return null;
            
            var settings = _currentDialogue.settings;
            if (settings is T tSettings) return tSettings;
            return null;
        }
        
        #endregion

        /// <summary>
        /// Begin a dialogue using the DialogueAsset to be started
        /// </summary>
        public void BeginDialogue(DialogueAsset dialogueAsset)
        {
            if (_currentDialogue != null)
            {
                Debug.LogWarning($"Attempted to begin dialogue \"{dialogueAsset.name}\" while dialogue was already playing");
                return;
            }
            
            _currentDialogue = dialogueAsset;
            _currentTrace = dialogueAsset;
            StartedDialogue.Invoke();
        }

        /// <summary>
        /// Retrieve the next dialogue using context about the user's interaction.
        /// Returns null if end of dialogue is reached.
        /// </summary>
        public DialogueParams AdvanceDialogue(AdvanceDialogueContext context)
        {
            ClearKeywords(KeywordScope.Single);
            current = this;
            
            do {
                _currentTrace = _currentTrace.AdvanceDialogue(context, this);
            } while (_currentTrace != null && _currentTrace is not IDialogueOutput);

            if (_currentTrace is IDialogueOutput outputDialogue)
            {
                var details = new DialogueParams(outputDialogue.GetDialogueDetails(context, this));
                details.ReplaceValues(this);
                return details;
            }
            
            EndDialogue();
            return null;
        }
        
        /// <summary>
        /// Retrieve the next dialogue using default context.
        /// Primarily used to get the first dialogue.
        /// Returns null if end of dialogue is reached.
        /// </summary>
        public DialogueParams AdvanceDialogue() => AdvanceDialogue(new AdvanceDialogueContext());

        /// <summary>
        /// Retrieve the information about the current dialogue again.
        /// Used if you want to account for changes in conditional choice options
        /// </summary>
        public DialogueParams RefreshDialogue()
        {
            if (_currentDialogue != null)
            {
                throw new Exception($"Attempted to refresh dialogue while dialogue was not playing");
            }

            var dialogueOutput = (IDialogueOutput)_currentTrace;
            var details = new DialogueParams(dialogueOutput.GetDialogueDetails(_previousContext, this));
            details.ReplaceValues(this);
            return details;
        }
        
        /// <summary>
        /// Prematurely end dialogue
        /// </summary>
        public void EndDialogue()
        {
            if (_currentDialogue == null) return;
            
            _currentDialogue.RunEndOperations(this);
            ClearKeywords(KeywordScope.Dialogue);
            
            _currentDialogue = null;
            _currentTrace = null;
        }
    }
    

}
