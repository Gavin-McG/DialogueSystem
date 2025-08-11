using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DSEventNode : Node
    {
        protected const string EventOptionName = "eventObject";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<DSEvent>(EventOptionName, "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed", defaultValue: null);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineEventInputPort(context);
        }

        public virtual DSEventCaller GetEvent(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<DSEvent>(this, EventOptionName);
        }
    }

    public abstract class DSEventNode<T, TEvent, TReference> : DSEventNode, IDialogueReferenceNode
        where TEvent : DSEvent<T>
        where TReference : DSEventReference<T>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<TEvent>(EventOptionName, "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed", defaultValue: null);
            
            DialogueGraphUtility.DefineFieldOptions<TReference>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
            
            DialogueGraphUtility.DefineFieldPorts<TReference>(context);

        }

        private TEvent GetEventOption()
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<TEvent>(this, EventOptionName);
        }

        private void AssignEventReferenceValues(TReference eventReference)
        {
            DialogueGraphUtility.AssignFromFieldOptions(this, ref eventReference);
            eventReference.dialogueEvent = GetEventOption();

            eventReference.name = "Event Reference";
        }

        public ScriptableObject CreateDialogueObject()
        {
            var eventReference = ScriptableObject.CreateInstance<TReference>();
            AssignEventReferenceValues(eventReference);
            
            return eventReference;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var obj = DialogueGraphUtility.GetObjectFromNode<TReference>(this, dialogueDict);
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref obj);
        }

        public override DSEventCaller GetEvent(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            return DialogueGraphUtility.GetObjectFromNode<TReference>(this, dialogueDict);
        }
    }
}
