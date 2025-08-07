using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class BeginDialogueNode : Node, IDialogueReferenceNode
    {
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            DialogueGraphUtility.DefineEventOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            asset.name = "Dialogue Asset";
            
            return asset;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var asset = DialogueGraphUtility.GetObject<DialogueAsset>(this, dialogueDict);
            var dialogueObject = DialogueGraphUtility.GetConnectedDialogue(this, dialogueDict);
            
            asset.nextDialogue = dialogueObject;
            asset.events = DialogueGraphUtility.GetEvents(this);
        }
    }

}
