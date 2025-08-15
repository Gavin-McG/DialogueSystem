using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class BeginDialogueNode : Node, IDialogueTraceNode
    {
        private const string EndEventPortName = "endEvents";
        private const string EndEventPortDisplayName = "End Events";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<DialogueSettings>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<DialogueSettings>(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);
            DialogueGraphUtility.DefineBasicOutputPort(context, EndEventPortName, EndEventPortDisplayName);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            asset.name = "Dialogue Asset";
            
            asset.settings = DialogueGraphUtility.AssignFromFieldOptions<DialogueSettings>(this);
            
            return asset;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var asset = DialogueGraphUtility.GetObject<DialogueAsset>(this, dialogueDict);
            var dialogueObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            asset.nextDialogue = dialogueObject;

            DialogueGraphUtility.AssignKeywordAndEventReferences(this, asset, dialogueDict);
            asset.endEvents = DialogueGraphUtility.GetDataType<DSEventCaller>(this, dialogueDict, EndEventPortName);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref asset.settings);
        }
    }

}
