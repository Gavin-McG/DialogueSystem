using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Non-choice dialogue.
    /// </summary>
    /// <typeparam name="TBaseParams">Type of <see cref="BaseParams"/> to be used by the node</typeparam>
    [Serializable]
    public class DialogueNode : Node, IDialogueTraceNode, IInputDataNode<DialogueTrace>
    {
        private Dialogue _asset;
        private INodeOption _dataOption;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _dataOption = context.AddOption<TextData>("Data").Build();
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            
        }

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<Dialogue>();
            _asset.name = "Basic Dialogue";
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            _dataOption.TryGetValue(out _asset.textData);
        }
        
        public DialogueTrace GetInputData() => _asset;
    }

}
