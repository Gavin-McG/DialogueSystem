using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
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
                case IDataNode<T> dataNode:
                    return dataNode.GetData();
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
                .OfType<IDataNode<T>>()
                .Select(node => node.GetData())
                .Where(data => data != null);
        }
        
        public static T GetFirstData<T>(IPort port) where T : class {
            var connectedPorts = new List<IPort>();
            port.GetConnectedPorts(connectedPorts);

            return connectedPorts
                .Select(p => p.GetNode())
                .OfType<IDataNode<T>>()
                .FirstOrDefault()
                ?.GetData();
        }
        
        public static DialogueObject GetTrace(IPort port) => GetFirstData<DialogueObject>(port);
    }
}
