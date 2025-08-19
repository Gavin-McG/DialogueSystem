using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime.Keywords;
using DialogueSystem.Runtime.Values;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{

    public class DialogueManager : MonoBehaviour, IKeywordContext, IValueContext
    {
        public static DialogueManager current;
        
        private readonly KeywordContext _keywordContext = new KeywordContext();
        private readonly ValueContext _valueContext = new ValueContext();
        
        [HideInInspector] public UnityEvent<DialogueSettings> beginDialogueEvent = new();

        private DialogueAsset currentDialogue;
        private DialogueTrace currentTrace;
        private AdvanceDialogueContext previousContext;
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

        public void BeginDialogue(DialogueAsset dialogueAsset)
        {
            if (currentDialogue != null)
            {
                Debug.LogWarning($"Attempted to begin dialogue \"{dialogueAsset.name}\" while dialogue was already playing");
                return;
            }
            
            currentDialogue = dialogueAsset;
            currentTrace = dialogueAsset;
            beginDialogueEvent.Invoke(dialogueAsset.settings);
        }

        public DialogueParams GetNextDialogue(AdvanceDialogueContext context)
        {
            ClearKeywords(KeywordScope.Single);
            current = this;
            
            do {
                currentTrace.RunOperations(this);
                currentTrace = currentTrace.GetNextDialogue(context, this);
            } while (currentTrace != null && currentTrace is not IDialogueOutput);

            if (currentTrace is IDialogueOutput outputDialogue)
            {
                var details = new DialogueParams(outputDialogue.GetDialogueDetails(context, this));
                details.ReplaceValues(_valueContext);
                return details;
            }
            
            EndDialogue();
            return null;
        }
        
        public DialogueParams GetNextDialogue() => GetNextDialogue(new AdvanceDialogueContext());

        public DialogueParams RefreshDialogue()
        {
            if (currentDialogue != null)
            {
                throw new Exception($"Attempted to refresh dialogue while dialogue was not playing");
            }

            var dialogueOutput = (IDialogueOutput)currentTrace;
            var details = new DialogueParams(dialogueOutput.GetDialogueDetails(previousContext, this));
            details.ReplaceValues(_valueContext);
            return details;
        }
        
        public void EndDialogue()
        {
            if (currentDialogue == null) return;
            
            currentDialogue.InvokeEndEvents();
            ClearKeywords(KeywordScope.Dialogue);
            
            currentDialogue = null;
            currentTrace = null;
        }
    }
    

}
