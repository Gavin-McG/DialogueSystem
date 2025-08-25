using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;

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
    public abstract class CustomEventNode<T, TEvent> : Node, IInputDataNode<DSEventReference>, IErrorNode
        where TEvent : DSEvent<T>
    {
        private INodeOption _eventOption;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _eventOption = DialogueGraphUtility.AddNodeOption(context, "Event", typeof(TEvent));
            DialogueGraphUtility.AddTypeOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddDataInputPort(context);
            DialogueGraphUtility.AddTypePorts<T>(context);
        }
        
        public DSEventReference GetInputData()
        {
            _eventOption.TryGetValue(out TEvent dialogueEvent);
            T value = default(T);
            DialogueGraphUtility.AssignFromNode(this, ref value);
            return new DSEventCaller<T>()
            {
                dialogueEvent = dialogueEvent,
                value = value
            };
        }
        
        public void DisplayErrors(GraphLogger infos)
        {
            _eventOption.TryGetValue(out TEvent dialogueEvent);
            if (dialogueEvent == null)
                infos.LogWarning("EventNode must have a Event Object Assigned", this);
            
        }
    }
}
