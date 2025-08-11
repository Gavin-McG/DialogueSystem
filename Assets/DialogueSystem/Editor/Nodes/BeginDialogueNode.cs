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
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<DialogueGraphSettings>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<DialogueGraphSettings>(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            asset.name = "Dialogue Asset";
            
            asset.settings = DialogueGraphUtility.AssignFromFieldOptions<DialogueGraphSettings>(this);
            
            return asset;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var asset = DialogueGraphUtility.GetObject<DialogueAsset>(this, dialogueDict);
            var dialogueObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref asset.settings);
            
            asset.nextDialogue = dialogueObject;
            asset.events = DialogueGraphUtility.GetEvents(this, dialogueDict);
        }
    }

}
