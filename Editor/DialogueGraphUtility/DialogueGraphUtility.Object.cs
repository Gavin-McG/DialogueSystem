using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Helper functions that are commonly used within the DialogueGraph Editor
    /// </summary>
    internal static partial class DialogueGraphUtility
    {
        /* ─────────────────────────────────────────────
         * PORT VALUE RETRIEVAL METHODS
         * ───────────────────────────────────────────── */

        /// <summary>
        /// Gets a ScriptableObject of type T from a port. 
        /// Object can be retrieved via assignment, IVariableNode, or GenericObjectNode
        /// </summary>
        public static T GetPortValue<T>(IPort port) {
            // Try to get object value from connected variable
            var connectedPort = port.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    return port.TryGetValue(out T attachedObject) ? attachedObject : default;
                case IVariableNode variableNode:
                    return variableNode.variable.TryGetDefaultValue(out T varObject) ? varObject : default;
                case IOutputDataNode<T> dataNode:
                    return dataNode.GetOutputData();
                default:
                    return default;
            }
        }

        /* ─────────────────────────────────────────────
         * DATA RETRIEVAL & ASSIGNMENT METHODS
         * ───────────────────────────────────────────── */
        
        public static IEnumerable<T> GetAllData<T>(IPort port)
        {
            var connectedPorts = new List<IPort>();
            port.GetConnectedPorts(connectedPorts);
            
            return connectedPorts
                .Select(port => port.GetNode())
                .OfType<IInputDataNode<T>>()
                .Select(node => node.GetInputData())
                .Where(data => data != null);
        }
        public static T GetFirstData<T>(IPort port) => GetAllData<T>(port).FirstOrDefault();
        
        public static void AssignDialogueData(
            DialogueData data,
            IPort port)
        {
            data.events = GetAllData<DSEventReference>(port).ToList();
            data.values = GetAllData<ValueEditor>(port).ToList();
        }
        
        public static DialogueTrace GetTrace(IPort port) => GetFirstData<DialogueTrace>(port);
    }
}
