using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public abstract class CustomEventNode<T, TEvent> : Node, IDataNode<DSEventReference>
        where TEvent : DSEvent<T>
    {
        private const string EventOptionName = "eventObject";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<TEvent>(EventOptionName, "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed", defaultValue: null);
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineEventInputPort(context);
            
            DialogueGraphUtility.DefineFieldPorts<T>(context);
        }
        
        public DSEventReference GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
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
