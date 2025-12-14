using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// enum to designate the type of Dialogue
    /// </summary>
    public enum DialogueType { Basic, Choice }
    
    /// <summary>
    /// Class used to provide data from the backend of the Dialogue system to the UI.
    /// Uses Getter functions to retrieve params of your derived type
    /// </summary>
    [Serializable]
    public class DialogueInfo
    {
        public DialogueType dialogueType;
        private string text;
        public TextParameters _textParams;
        private ChoiceParameters _choiceParams;
        public List<ResponseInfo> responses;

        internal DialogueInfo(string text, TextParameters textParams)
        {
            dialogueType = DialogueType.Basic;
            this.text = text;
            this._textParams = textParams;
            this._choiceParams = null;
            this.responses = null;
        }

        internal DialogueInfo(string text, TextParameters textParams, ChoiceParameters choiceParams, List<ResponseInfo> responses)
        {
            dialogueType = DialogueType.Choice;
            this.text = text;
            this._textParams = textParams;
            this._choiceParams = choiceParams;
            this.responses = responses;
        }
        
        private const string RegexPattern = @"\{(.*?)\}";
        
        private static List<string> ExtractBracketContents(string text)
        {
            List<string> result = new List<string>();
            foreach (Match match in Regex.Matches(text, RegexPattern))
            {
                result.Add(match.Groups[1].Value);
            }
            return result.Distinct().ToList();
        }
        
        // internal void ReplaceText(IValueContext context)
        // {
        //     var subStrings = ExtractBracketContents(text);
        //     
        //     text = Regex.Replace(text, RegexPattern, match =>
        //     {
        //         var subString = match.Groups[1].Value;
        //         int index = subStrings.IndexOf(subString);
        //         if (index >= values.Count)
        //             return match.Value; // safeguard: leave the placeholder as-is
        //
        //         var valueSO = values[index];
        //         if (valueSO == null)
        //             return string.Empty;
        //
        //         var val = valueSO.GetValue(context);
        //         return val?.ToString() ?? string.Empty;
        //     });
        // }
        

        /// <summary>
        /// Returns the Base Parameters of the dialogue as type T
        /// </summary>
        public T GetTextParams<T>() where T : TextParameters
        {
            if (_textParams is T tBaseParams) return tBaseParams;
            return null;
        }

        /// <summary>
        /// Returns the Choice Parameters of the dialogue as type T
        /// </summary>
        public T GetChoiceParams<T>() where T : ChoiceParameters
        {
            if (_choiceParams is T tChoiceParams) return tChoiceParams;
            return null;
        }
        
        //type getters
        public Type GetBaseParamsType() => _textParams?.GetType();
        public Type GetChoiceParamsType() => _choiceParams?.GetType();
    }
}
