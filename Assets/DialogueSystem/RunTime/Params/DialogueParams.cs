using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime.Values;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    [Serializable]
    public class DialogueParams
    {
        public enum DialogueType { Basic, Choice }
        
        public DialogueType dialogueType;
        private BaseParams baseParams;
        private ChoiceParams choiceParams;
        private List<OptionParams> options;

        public DialogueParams(BaseParams baseParams)
        {
            dialogueType = DialogueType.Basic;
            this.baseParams = baseParams;
            choiceParams = null;
            options = new List<OptionParams>();
        }

        public DialogueParams(BaseParams baseParams, ChoiceParams choiceParams, List<OptionParams> options)
        {
            this.dialogueType = DialogueType.Choice;
            this.baseParams = baseParams;
            this.choiceParams = choiceParams;
            this.options = options;
        }

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            baseParams = copyObj.baseParams?.Clone();
            choiceParams = copyObj.choiceParams?.Clone();
            options = copyObj.options?.Select(option => option.Clone()).ToList();
        }

        public T GetBaseParams<T>() where T : BaseParams
        {
            if (baseParams is T tBaseParams) return tBaseParams;
            return null;
        }

        public T GetChoiceParams<T>() where T : ChoiceParams
        {
            if (choiceParams is T tChoiceParams) return tChoiceParams;
            return null;
        }

        public List<T> GetOptions<T>() where T : OptionParams
        {
            return options
                .OfType<T>()
                .ToList();
        }

        internal void ReplaceValues(IValueContext context)
        {
            baseParams.Text = ReplaceTextValues(context, baseParams.Text);
            options = options?.Select(option => {
                option.Text = ReplaceTextValues(context, option.Text);
                return option;
            }).ToList();
        }

        public static string ReplaceTextValues(IValueContext context, string text)
        {
            return Regex.Replace(text, @"\{(.*?)\}", match =>
            {
                string key = match.Groups[1].Value;
                object value = context.GetValue(key);
                return value?.ToString() ?? "";
            });
        }
    }
    
}
