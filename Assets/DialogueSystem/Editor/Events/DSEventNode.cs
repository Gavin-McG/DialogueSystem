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

        public virtual DSEventCaller GetEvent(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<DSEvent>(this, EventOptionName);
        }
    }

    public abstract class DSEventNode<T> : DSEventNode, IDialogueObjectNode
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<DSEvent<T>>(EventOptionName, "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed", defaultValue: null);
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }
        
        private T GetValueOption()
        {
            return DialogueGraphUtility.AssignFromFieldOptions<T>(this);
        }

        private DSEvent<T> GetEventOption()
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<DSEvent<T>>(this, EventOptionName);
        }

        protected void AssignEventReferenceValues(DSEventReference<T> eventReference)
        {
            eventReference.value = GetValueOption();
            eventReference.dialogueEvent = GetEventOption();

            eventReference.name = "Event Reference";
        }

        public abstract DialogueObject CreateDialogueObject();

        public override DSEventCaller GetEvent(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            return DialogueGraphUtility.GetObjectFromNode<DSEventReference<T>>(this, dialogueDict);
        }
    }
}
