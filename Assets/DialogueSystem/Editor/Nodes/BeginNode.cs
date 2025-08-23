using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Node which represents the beginning of a dialogue interaction.
    /// Has options corresponding to <see cref="DialogueSettings"/> 
    /// </summary>
    public abstract class BeginNode<T> : Node, IDialogueTraceNode, IBeginNode
    where T : DialogueSettings
    {
        private const string EndEventPortName = "endEvents";
        private const string EndEventPortDisplayName = "End Events";
        
        protected sealed override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<T>(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);
            DialogueGraphUtility.DefineBasicOutputPort(context, EndEventPortName, EndEventPortDisplayName);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            asset.name = "Dialogue Asset";
            
            asset.settings = DialogueGraphUtility.AssignFromFieldOptions<T>(this);
            
            return asset;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var asset = DialogueGraphUtility.GetObject<DialogueAsset>(this, dialogueDict);
            var dialogueObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            asset.nextDialogue = dialogueObject;

            DialogueGraphUtility.AssignDialogueData(this, asset.data, dialogueDict);
            DialogueGraphUtility.AssignDialogueData(this, asset.endData, dialogueDict, EndEventPortName);
            
            var settings = (T)asset.settings;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref settings);
        }

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
