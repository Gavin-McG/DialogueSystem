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
        private const string ValueOptionName = "value";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<DSEvent<T>>(EventOptionName, "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed", defaultValue: null);
            
            context.AddNodeOption<T>(ValueOptionName, "Value");
        }
        
        private T GetValueOption()
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<T>(this, ValueOptionName);
        }

        private DSEvent<T> GetEventOption()
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<DSEvent<T>>(this, EventOptionName);
        }

        protected void AssignEventReferenceValues(DSEventReference<T> eventReference)
        {
            eventReference.value = GetValueOption();
            eventReference.dialogueEvent = GetEventOption();
        }

        public abstract DialogueObject CreateDialogueObject();

        public override DSEventCaller GetEvent(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            return DialogueGraphUtility.GetObjectFromNode<DSEventReference<T>>(this, dialogueDict);
        }
    }
}
