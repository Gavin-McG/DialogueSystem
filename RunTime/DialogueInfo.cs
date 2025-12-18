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
        public string text;
        private TextParameters _textParams;
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
        
        //-----------------------------------------------
        //           Text Replacement
        //-----------------------------------------------
        
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
        
        private static string ReplaceText(string text, IVariableContext variables)
        {
            var subStrings = ExtractBracketContents(text);
            
            return Regex.Replace(text, RegexPattern, match =>
            {
                var subString = match.Groups[1].Value;

                if (variables.TryGetVariable(subString, out var variable))
                    return variable.ToString();
                
                return '{' + subString + "}";
            });
        }

        internal void ApplyVariables(IVariableContext variables)
        {
            text = ReplaceText(text, variables);

            if (dialogueType != DialogueType.Choice || responses == null) 
                return;
            
            foreach (var response in responses)
                response.text = ReplaceText(response.text, variables);
        }
    }
}
