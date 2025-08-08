using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{

    public static partial class DialogueGraphUtility
    {
        public static DialogueObject GetObject(IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogueObject = dialogueDict.GetValueOrDefault(dialogueObjectNode);
            return dialogueObject;
        }

        public static T GetObject<T>(IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            var dialogueObject = GetObject(dialogueObjectNode, dialogueDict);
            return (dialogueObject is T objectAsT) ? objectAsT : null;
        }
        
        public static DialogueObject GetObjectFromNode(INode node,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return  GetObject(dialogueObjectNode, dialogueDict);
        }
        
        public static T GetObjectFromNode<T>(INode node,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return  GetObject<T>(dialogueObjectNode, dialogueDict);
        }

        public static DialogueObject GetConnectedTrace(INode node, string portName,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogueObjectNode = GetConnectedTraceNode(node, portName);
            if (dialogueObjectNode == null) return null;
            return GetObject(dialogueObjectNode, dialogueDict);
        }

        public static T GetConnectedTrace<T>(INode node, string portName,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            var dialogueObject = GetConnectedTrace(node, portName, dialogueDict);
            return (dialogueObject is T objectAsT) ? objectAsT : null;
        }

        private static IDialogueTraceNode GetConnectedTraceNode(INode node, string portName)
        {
            var port = node.GetOutputPortByName(portName);
            
            var connectedPorts = new List<IPort>();
            port.GetConnectedPorts(connectedPorts);
            
            var connectedNode = connectedPorts
                .Select(conectedPort => conectedPort.GetNode())
                .OfType<IDialogueTraceNode>()
                .FirstOrDefault();
            return connectedNode;
        }

        public static DialogueTrace GetConnectedTrace(INode node, 
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogueObject = GetConnectedTrace(node, NextPortName, dialogueDict);
            return dialogueObject is DialogueTrace dialogueTrace ? dialogueTrace : null;
        }
        

        public static T GetDialogueObjectValueOrNull<T>(INode node, string portName,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            //get port for object
            var port = GetInputPortByName(node, portName);
            if (port == null) return null;
            
            //try to get profile value from connected variable
            var connectedPort = port?.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    //try to get value assigned directly if no connected node
                    return port.TryGetValue(out T attachedObject) ? attachedObject : null;
                case IVariableNode variableNode:
                    //return value assigned from variable node
                    return variableNode.variable
                        .TryGetDefaultValue(out T varObject) ? varObject : null;
                case GenericObjectNode<T> objectNode:
                    //return created profile object from profile node
                    return GetObjectFromNode<T>(objectNode, dialogueDict);
                default:
                    return null;
            }
        }

        public static List<DialogueEvent> GetEvents(INode node)
        {
            var eventPort = GetNextPortOrNull(node);

            var connectedEventPorts = new List<IPort>();
            eventPort.GetConnectedPorts(connectedEventPorts);
            var eventNodes = connectedEventPorts.Select(port => port.GetNode());

            var events = new List<DialogueEvent>();
            foreach (var eventNode in eventNodes)
            {
                if (eventNode is DialogueEventNode dialogueEventNode && dialogueEventNode.GetEvent())
                {
                    events.Add(dialogueEventNode.GetEvent());
                }
            }
            return events;
        }
        
    }

}
