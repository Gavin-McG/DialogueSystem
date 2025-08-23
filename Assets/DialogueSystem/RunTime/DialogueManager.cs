using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WolverineSoft.DialogueSystem.Keywords;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
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
        private readonly ValueContext _valueContext = new ValueContext();

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
        
        #region VALUES
        public void DefineValue(string valueName, object value, ValueScope scope = ValueScope.Manager) => _valueContext.DefineValue(valueName, value, scope);
        public void UndefineValue(string valueName, ValueScope scope = ValueScope.Manager) => _valueContext.UndefineValue(valueName, scope);
        public bool IsValueDefined(string valueName) => _valueContext.IsValueDefined(valueName);
        public object GetValue(string valueName) => _valueContext.GetValue(valueName);
        public T GetValue<T>(string valueName) => _valueContext.GetValue<T>(valueName);
        public ValueScope GetValueScope(string valueName) => _valueContext.GetValueScope(valueName);
        public void ClearValues(ValueScope scope) => _valueContext.ClearValues(scope);
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
                details.ReplaceValues(_valueContext);
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
            details.ReplaceValues(_valueContext);
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
