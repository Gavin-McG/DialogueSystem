using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Class used to provide data from the backend of the Dialogue system to the UI.
    /// Uses Getter functions to retrieve params of your derived type
    /// </summary>
    [Serializable]
    public sealed class DialogueParams
    {
        public enum DialogueType { Basic, Choice }
        
        public DialogueType dialogueType;
        private BaseParams _baseParams;
        private ChoiceParams _choiceParams;
        private List<OptionParams> _options;

        public DialogueParams(BaseParams baseParams)
        {
            dialogueType = DialogueType.Basic;
            this._baseParams = baseParams;
            _choiceParams = null;
            _options = new List<OptionParams>();
        }

        public DialogueParams(BaseParams baseParams, ChoiceParams choiceParams, List<OptionParams> options)
        {
            this.dialogueType = DialogueType.Choice;
            this._baseParams = baseParams;
            this._choiceParams = choiceParams;
            this._options = options;
        }

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            _baseParams = copyObj._baseParams?.Clone();
            _choiceParams = copyObj._choiceParams?.Clone();
            _options = copyObj._options?.Select(option => option.Clone()).ToList();
        }

        /// <summary>
        /// Returns the Base Parameters of the dialogue as type T
        /// </summary>
        public T GetBaseParams<T>() where T : BaseParams
        {
            if (_baseParams is T tBaseParams) return tBaseParams;
            return null;
        }

        /// <summary>
        /// Returns the Choice Parameters of the dialogue as type T
        /// </summary>
        public T GetChoiceParams<T>() where T : ChoiceParams
        {
            if (_choiceParams is T tChoiceParams) return tChoiceParams;
            return null;
        }
        
        /// <summary>
        /// Returns the Options Parameters of the dialogue as type T
        /// </summary>
        public List<T> GetOptions<T>() where T : OptionParams
        {
            return _options
                .OfType<T>()
                .ToList();
        }
        
        //get getters
        public Type GetBaseParamsType() => _baseParams?.GetType();
        public Type GetChoiceParamsType() => _choiceParams?.GetType();
        public Type GetOptionsType() => _options?.FirstOrDefault()?.GetType();
        

        internal void ReplaceValues(IValueContext context)
        {
            _baseParams.Text = ReplaceTextValues(context, _baseParams.Text);
            _options = _options?.Select(option => {
                option.Text = ReplaceTextValues(context, option.Text);
                return option;
            }).ToList();
        }

        internal static string ReplaceTextValues(IValueContext context, string text)
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
