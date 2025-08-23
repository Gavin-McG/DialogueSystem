using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Generic Base Class for Custom Type Events that aren't natively supported by the Graph Toolkit.
    /// Alternative to <see cref="EventNode"/> 
    /// </summary>
    /// <typeparam name="T">value type of the Event</typeparam>
    /// <typeparam name="TEvent">type of the Event</typeparam>
    public abstract class CustomEventNode<T, TEvent> : Node, IDataNode<DSEventReference>, IErrorNode
        where TEvent : DSEvent<T>
    {
        private const string EventOptionName = "eventObject";
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.AddNodeOption(context, 
                EventOptionName, typeof(TEvent), "Event",
                tooltip: "Event object to be acted upon when Dialogue is passed");
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
            
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
        
        public void DisplayErrors(GraphLogger infos)
        {
            var eventObject = DialogueGraphUtility.GetOptionValueOrDefault<TEvent>(this, EventOptionName);
            if (eventObject == null)
                infos.LogWarning("EventNode must have a Event Object Assigned", this);
            
        }
    }
}
