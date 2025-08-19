using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime.Keywords;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{

    public class DialogueManager : MonoBehaviour, IKeywordContext
    {
        private readonly KeywordContext _keywordContext = new KeywordContext();
        
        private readonly Dictionary<string, object> insertValues = new();
        
        [HideInInspector] public UnityEvent<DialogueSettings> beginDialogueEvent = new();

        private DialogueAsset currentDialogue;
        private DialogueTrace currentTrace;
        [HideInInspector] public List<int> optionIndexes;
        
        #region KEYWORDS
        public void DefineKeyword(string keyword, KeywordScope scope) => _keywordContext.DefineKeyword(keyword, scope);
        public void UndefineKeyword(string keyword, KeywordScope scope) => _keywordContext.UndefineKeyword(keyword, scope);
        public bool IsKeywordDefined(string keyword) => _keywordContext.IsKeywordDefined(keyword);
        public void ClearKeywords(KeywordScope scope) => _keywordContext.ClearKeywords(scope);
        #endregion

        public void SetValue(string valueName, object value)
        {
            insertValues[valueName] = value;
        }

        public object GetValue(string valueName)
        {
            if (insertValues.TryGetValue(valueName, out var value))
                return value;

            return $"{{No Value \"{valueName}\"}}";
        }
        
        private string ReplaceValues(string input)
        {
            return Regex.Replace(input, @"\{(.*?)\}", match =>
            {
                string key = match.Groups[1].Value;
                object value = GetValue(key);
                return value?.ToString() ?? "";
            });
        }
        
        

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
            do {
                currentTrace.RunOperations(this);
                currentTrace = currentTrace.GetNextDialogue(context, this);
            } while (currentTrace != null && currentTrace is not IDialogueOutput);

            if (currentTrace is IDialogueOutput outputDialogue)
            {
                var details = new DialogueParams(outputDialogue.GetDialogueDetails(context, this));
                details.baseParams.Text = ReplaceValues(details.baseParams.Text);
                ClearKeywords(KeywordScope.Single);
                return details;
            }
            
            EndDialogue();
            return null;
        }
        
        public DialogueParams GetNextDialogue() => GetNextDialogue(new AdvanceDialogueContext());

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
