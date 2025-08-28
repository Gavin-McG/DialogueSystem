using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Node which represents the beginning of a dialogue interaction.
    /// Has options corresponding to <see cref="DialogueSettings"/> 
    /// </summary>
    public abstract class BeginNode<T> : Node, IDialogueTraceNode, IBeginNode, IInputDataNode<DialogueTrace>
    where T : DialogueSettings
    {
        private const string EndEventPortName = "End Events";
        
        private IPort _nextPort;
        private IPort _endPort;
        private DialogueAsset _asset;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.AddTypeOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddTypePorts<T>(context);
            
            _nextPort = DialogueGraphUtility.AddNextPort(context);
            _endPort = DialogueGraphUtility.AddNextPort(context, EndEventPortName);
        }

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<DialogueAsset>();
            _asset.name = "Dialogue Asset";
            return _asset;
        }

        public void AssignObjectReferences()
        {
            var dialogueObject = DialogueGraphUtility.GetTrace(_nextPort);
            _asset.nextDialogue = dialogueObject;

            DialogueGraphUtility.AssignDialogueData(_asset.data, _nextPort);
            DialogueGraphUtility.AssignDialogueData(_asset.endData, _endPort);
            
            var settings = Activator.CreateInstance<T>();
            DialogueGraphUtility.AssignFromNode(this, ref settings);
            _asset.settings = settings;
        }

        public DialogueTrace GetInputData() => _asset;

        public void DisplayErrors(GraphLogger infos)
        {
            DialogueGraphUtility.MultipleOutputsCheck(this, infos);
            DialogueGraphUtility.CheckPreviousConnection((INode)this, infos);
            
            EndEventCheck(this, infos);
        }

        private void EndEventCheck(INode node, GraphLogger infos)
        {
            int traceCount = DialogueGraphUtility.GetConnectedTraceCount(node, EndEventPortName);
            
            if (traceCount > 0)
                infos.LogError("Cannot Connect Dialogue Trace to End Events Port", node);
        }
    }

}
