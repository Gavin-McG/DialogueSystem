using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Choice Dialogue Nodes. 
    /// </summary>
    public class ChoiceNode : OptionContext
    {
        protected sealed override string NextPortName => "TimeOut";
        public sealed override bool UsesWeight => false;

        private ChoiceDialogue _asset;
        private INodeOption _dataOption;
        //private readonly List<IPort> _valuePorts = new();
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _dataOption = context.AddOption<TabGroup<TextData, ChoiceData>>("Data").Build();
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            
        }

        public sealed override ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<ChoiceDialogue>();
            _asset.name = "ChoiceDialogue";
            return _asset;
        }
        
        public sealed override void AssignObjectReferences()
        {
            _dataOption.TryGetValue(out TabGroup<TextData, ChoiceData> data);
            _asset.textData = data.first;
            _asset.choiceData = data.second;
        }

        public sealed override DialogueTrace GetInputData() => _asset;
    }

}
