using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Node which represents the beginning of a dialogue interaction.
    /// Has options corresponding to <see cref="SettingsData"/> 
    /// </summary>
    public class BeginNode : Node, IDialogueTraceNode, IInputDataNode<DialogueTrace>
    {
        private const string EndEventPortName = "End Events";

        private DialogueAsset _asset;
        private INodeOption _dataOption;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _dataOption = context.AddOption<SettingsData>("Data").Build();
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            
        }

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<DialogueAsset>();
            _asset.name = "DialogueAsset";
            return _asset;
        }

        public void AssignObjectReferences()
        {
            //_dataOption.TryGetValue(out _asset.settingsData);
        }

        public DialogueTrace GetInputData() => _asset;
    }

}
