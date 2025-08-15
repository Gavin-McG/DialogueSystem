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
        public static ScriptableObject GetObject(IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogueObject = dialogueDict.GetValueOrDefault(dialogueObjectNode);
            return dialogueObject;
        }

        public static T GetObject<T>(IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict) where T : ScriptableObject
        {
            var dialogueObject = GetObject(dialogueObjectNode, dialogueDict);
            return (dialogueObject is T objectAsT) ? objectAsT : null;
        }
        
        public static ScriptableObject GetObjectFromNode(INode node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return  GetObject(dialogueObjectNode, dialogueDict);
        }
        
        public static T GetObjectFromNode<T>(INode node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict) where T : ScriptableObject
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return  GetObject<T>(dialogueObjectNode, dialogueDict);
        }

        private static ScriptableObject GetConnectedTrace(INode node, string portName,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogueObjectNode = GetConnectedTraceNode(node, portName);
            if (dialogueObjectNode == null) return null;
            return GetObject(dialogueObjectNode, dialogueDict);
        }

        private static T GetConnectedTrace<T>(INode node, string portName,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict) where T : ScriptableObject
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
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogueObject = GetConnectedTrace(node, NextPortName, dialogueDict);
            return dialogueObject is DialogueTrace dialogueTrace ? dialogueTrace : null;
        }
        

        public static T GetDialogueObjectValueOrNull<T>(INode node, string portName,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict) where T : ScriptableObject
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

        public static List<DSEventCaller> GetEvents(INode node, 
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict, string portName=NextPortName)
        {
            var eventPort = GetOutputPortByName(node, portName);

            var connectedPorts = new List<IPort>();
            eventPort.GetConnectedPorts(connectedPorts);
            var connectedNodes = connectedPorts.Select(port => port.GetNode());

            var events = new List<DSEventCaller>();
            foreach (var connectedNode in connectedNodes)
            {
                if (connectedNode is DSEventNode eventNode)
                {
                    var dialogueEventCaller = eventNode.GetEvent(dialogueDict);
                    if (dialogueEventCaller)
                        events.Add(dialogueEventCaller);
                }
            }
            return events;
        }
        
        public static List<Keywords.KeywordEntry> GetKeywordEntries(INode node, 
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict, string portName=NextPortName)
        {
            var eventPort = GetOutputPortByName(node, portName);

            var connectedPorts = new List<IPort>();
            eventPort.GetConnectedPorts(connectedPorts);
            var connectedNodes = connectedPorts.Select(port => port.GetNode());

            var keywords = new List<Keywords.KeywordEntry>();
            foreach (var connectedNode in connectedNodes)
            {
                if (connectedNode is ModifyKeyWordNode keyWordNode)
                {
                    var keywordEntry = keyWordNode.GetEntry();
                    if (keywordEntry != null)
                        keywords.Add(keywordEntry);
                }
            }
            return keywords;
        }
        
        public static List<Values.ValueEntry> GetValueEntries(INode node, 
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict, string portName=NextPortName)
        {
            var eventPort = GetOutputPortByName(node, portName);

            var connectedPorts = new List<IPort>();
            eventPort.GetConnectedPorts(connectedPorts);
            var connectedNodes = connectedPorts.Select(port => port.GetNode());

            var keywords = new List<Values.ValueEntry>();
            foreach (var connectedNode in connectedNodes)
            {
                if (connectedNode is SetValueNode valueNode)
                {
                    var valueEntry = valueNode.GetEntry();
                    if (valueEntry != null)
                        keywords.Add(valueEntry);
                }
            }
            return keywords;
        }

        public static void AssignKeywordAndEventReferences(INode node, DialogueTrace dialogueTrace,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            dialogueTrace.events = GetEvents(node, dialogueDict);
            dialogueTrace.keywords = GetKeywordEntries(node, dialogueDict);
            dialogueTrace.values = GetValueEntries(node, dialogueDict);
        }
    }

}
