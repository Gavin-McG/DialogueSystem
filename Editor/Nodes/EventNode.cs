using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Node used to define where/how events are called in a graph.
    /// Defines a port based on the type parameter T of the assigned DSEvent 
    /// </summary>
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    internal class EventNode : Node, IDialogueNode
    {
        private EventObject _asset;
        private INodeOption _stallOption;
        private INodeOption _eventOption;
        private INodeOption _valueOption;
        private IPort _nextPort;
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _stallOption = context.AddOption<bool>("Stall")
                .WithTooltip("Whether the Event Stalls Dialogue")
                .Build();
            
            _eventOption = DialogueGraphUtility.DefineEventOption(context);
            _valueOption = DialogueGraphUtility.DefineEventValueOption(context, _eventOption);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context);
        }
  
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _stallOption.TryGetValue(out bool stall);

            _asset = stall ? 
                ScriptableObject.CreateInstance<StallObject>() : 
                ScriptableObject.CreateInstance<EventObject>();
            _asset.name = stall ? "StallEvent" : "Event";
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            _asset.eventCaller = DialogueGraphUtility.GetEventCaller(_eventOption, _valueOption);
                
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }
}