using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DSEventNode : Node, IDataNode<DSEventReference>
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

        public virtual DSEventReference GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            DSEvent dialogueEvent = DialogueGraphUtility.GetOptionValueOrDefault<DSEvent>(this, EventOptionName);
            return new DSEventCaller()
            {
                dialogueEvent = dialogueEvent
            };
            
        }
    }

    public abstract class DSEventNode<T, TEvent> : DSEventNode
        where TEvent : DSEvent<T>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<TEvent>(EventOptionName, "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed", defaultValue: null);
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);
            
            DialogueGraphUtility.DefineFieldPorts<T>(context);
        }
        
        public override DSEventReference GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            TEvent dialogueEvent = DialogueGraphUtility.GetOptionValueOrDefault<TEvent>(this, EventOptionName);
            T value = DialogueGraphUtility.AssignFromFieldOptions<T>(this);
            return new DSEventCaller<T>()
            {
                dialogueEvent = dialogueEvent,
                value = value
            };
            
        }
    }
}
