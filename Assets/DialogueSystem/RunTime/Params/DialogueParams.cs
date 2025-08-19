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
        
        public DialogueType dialogueType = DialogueType.Basic;
        public BaseParams baseParams;
        public ChoiceParams choiceParams;
        public List<OptionParams> options = new();
        
        public DialogueParams() {}

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            baseParams = copyObj.baseParams?.Clone();
            choiceParams = copyObj.choiceParams?.Clone();
            options = copyObj.options.Select(option => option.Clone()).ToList();
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
