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
        private IPort _nextPort;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context);
        }
  
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<EventObject>();
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }
}