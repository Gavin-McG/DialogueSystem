using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Helper functions that are commonly used within the DialogueGraph Editor
    /// </summary>
    internal static partial class DialogueGraphUtility
    {
        /* ─────────────────────────────────────────────
         * OBJECT RETRIEVAL METHODS
         * ───────────────────────────────────────────── */

        /// <summary>
        /// Get the ScriptableObject associated with a given IDialogueObjectNode
        /// </summary>
        public static ScriptableObject GetObject(
            IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            return dialogueDict.GetValueOrDefault(dialogueObjectNode);
        }

        /// <summary>
        /// Get the ScriptableObject associated with a given IDialogueObjectNode as type T
        /// </summary>
        public static T GetObject<T>(
            IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
            where T : ScriptableObject
        {
            var dialogueObject = GetObject(dialogueObjectNode, dialogueDict);
            return dialogueObject is T objectAsT ? objectAsT : null;
        }

        /// <summary>
        /// Get the ScriptableObject associated with a given INode
        /// </summary>
        public static ScriptableObject GetObjectFromNode(
            INode node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return GetObject(dialogueObjectNode, dialogueDict);
        }

        /// <summary>
        /// Get the ScriptableObject associated with a given INode as type T
        /// </summary>
        public static T GetObjectFromNode<T>(
            INode node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
            where T : ScriptableObject
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return GetObject<T>(dialogueObjectNode, dialogueDict);
        }

        /* ─────────────────────────────────────────────
         * TRACE RETRIEVAL METHODS
         * ───────────────────────────────────────────── */

        /// <summary>
        /// Gets the DialogueTrace object of a connected IDialogueTraceNode
        /// </summary>
        private static ScriptableObject GetConnectedTrace(
            INode node,
            string portName,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogueObjectNode = GetConnectedTraceNode(node, portName);
            return dialogueObjectNode == null ? null : GetObject(dialogueObjectNode, dialogueDict);
        }

        /// <summary>
        /// Gets the DialogueTrace object of a connected IDialogueTraceNode as type T
        /// </summary>
        private static T GetConnectedTrace<T>(
            INode node,
            string portName,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
            where T : ScriptableObject
        {
            var dialogueObject = GetConnectedTrace(node, portName, dialogueDict);
            return dialogueObject is T objectAsT ? objectAsT : null;
        }

        /// <summary>
        /// Gets the IDialogueTraceNode which is connected to a given port
        /// </summary>
        private static IDialogueTraceNode GetConnectedTraceNode(INode node, string portName)
        {
            var port = node.GetOutputPortByName(portName);

            var connectedPorts = new List<IPort>();
            port.GetConnectedPorts(connectedPorts);

            return connectedPorts
                .Select(connectedPort => connectedPort.GetNode())
                .OfType<IDialogueTraceNode>()
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the DialogueTrace from the connected IDialogueTraceNode from the "Next" port
        /// </summary>
        public static DialogueTrace GetConnectedTrace(
            INode node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogueObject = GetConnectedTrace(node, NextPortName, dialogueDict);
            return dialogueObject is DialogueTrace dialogueTrace ? dialogueTrace : null;
        }

        /* ─────────────────────────────────────────────
         * PORT VALUE RETRIEVAL METHODS
         * ───────────────────────────────────────────── */

        /// <summary>
        /// Gets a ScriptableObject of type T from a port. 
        /// Object can be retrieved via assignment, IVariableNode, or GenericObjectNode
        /// </summary>
        public static T GetDialogueObjectValueOrNull<T>(
            INode node,
            string portName,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
            where T : ScriptableObject
        {
            // Get port for object
            var port = GetInputPortByName(node, portName);
            if (port == null) return null;

            // Try to get object value from connected variable
            var connectedPort = port.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    return port.TryGetValue(out T attachedObject) ? attachedObject : null;
                case IVariableNode variableNode:
                    return variableNode.variable.TryGetDefaultValue(out T varObject) ? varObject : null;
                case GenericObjectNode<T> objectNode:
                    return GetObjectFromNode<T>(objectNode, dialogueDict);
                default:
                    return null;
            }
        }

        /* ─────────────────────────────────────────────
         * DATA RETRIEVAL & ASSIGNMENT METHODS
         * ───────────────────────────────────────────── */

        /// <summary>
        /// Gets a List of data connected to an output node from nodes of IDataNode of type T
        /// </summary>
        private static List<T> GetDataType<T>(
            INode node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict,
            string portName = NextPortName)
        {
            var port = GetOutputPortByName(node, portName);

            var connectedPorts = new List<IPort>();
            port.GetConnectedPorts(connectedPorts);

            var connectedNodes = connectedPorts.Select(port => port.GetNode());

            var result = new List<T>();
            foreach (var connectedNode in connectedNodes)
            {
                if (connectedNode is IDataNode<T> eventNode)
                {
                    var data = eventNode.GetData(dialogueDict);
                    if (data != null)
                        result.Add(data);
                    else
                        Debug.LogWarning($"Node {connectedNode} has no data");
                }
            }

            return result;
        }

        /// <summary>
        /// Assigns the Event, Keyword, and Value Data connected to a given port
        /// </summary>
        public static void AssignDialogueData(
            INode node,
            DialogueData data,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict,
            string portName = NextPortName)
        {
            data.events = GetDataType<DSEventReference>(node, dialogueDict, portName);
            data.values = GetDataType<ValueEditor>(node, dialogueDict, portName);
        }
    }
}
