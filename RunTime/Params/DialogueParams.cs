using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WolverineSoft.DialogueSystem.Values;

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
    public sealed class DialogueParams
    {
        public DialogueType dialogueType;
        private TextData _baseData;
        private ChoiceData _choiceData;
        private List<ResponseData> _options;

        public DialogueParams(TextData baseData)
        {
            dialogueType = DialogueType.Basic;
            this._baseData = baseData;
            _choiceData = null;
            _options = new List<ResponseData>();
        }

        public DialogueParams(TextData baseData, ChoiceData choiceData, List<ResponseData> options)
        {
            this.dialogueType = DialogueType.Choice;
            this._baseData = baseData;
            this._choiceData = choiceData;
            this._options = options;
        }

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            _baseData = ShallowClone(copyObj._baseData);
            _choiceData = ShallowClone(copyObj._choiceData);
            _options = copyObj._options?.Select(ShallowClone).ToList();
        }
        
        private static readonly MethodInfo MemberwiseCloneMethod =
            typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

        private static T ShallowClone<T>(T obj) where T : class
        {
            if (obj == null) return null;
            return (T)MemberwiseCloneMethod.Invoke(obj, null);
        }

        /// <summary>
        /// Returns the Base Parameters of the dialogue as type T
        /// </summary>
        public T GetBaseParams<T>() where T : TextData
        {
            if (_baseData is T tBaseParams) return tBaseParams;
            return null;
        }

        /// <summary>
        /// Returns the Choice Parameters of the dialogue as type T
        /// </summary>
        public T GetChoiceParams<T>() where T : ChoiceData
        {
            if (_choiceData is T tChoiceParams) return tChoiceParams;
            return null;
        }
        
        /// <summary>
        /// Returns the Options Parameters of the dialogue as type T
        /// </summary>
        public List<T> GetOptions<T>() where T : ResponseData
        {
            return _options
                .OfType<T>()
                .ToList();
        }
        
        //get getters
        public Type GetBaseParamsType() => _baseData?.GetType();
        public Type GetChoiceParamsType() => _choiceData?.GetType();
        public Type GetOptionsType() => _options?.FirstOrDefault()?.GetType();

        public void ReplaceValues(IValueContext context)
        {
            _baseData.ReplaceText(context);
            //_options.ForEach(option => option.ReplaceText(context));
        }
    }

    public sealed class DialogueParams<TBase, TChoice, TOption>
        where TBase : TextData
        where TChoice : ChoiceData
        where TOption : ResponseData
    {
        public DialogueType dialogueType;
        public TBase BaseParams;
        public TChoice ChoiceParams;
        public List<TOption> Options;

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            BaseParams = copyObj.GetBaseParams<TBase>();
            ChoiceParams = copyObj.GetChoiceParams<TChoice>();
            Options = copyObj.GetOptions<TOption>();
        }
    }
    
}
