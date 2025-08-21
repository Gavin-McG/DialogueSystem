using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    internal static partial class DialogueGraphUtility
    {
        /// <summary>
        /// Get the number of IDialogueTraceNodes connected to an output port
        /// </summary>
        /// <param name="node">The node to check the port of</param>
        /// <param name="portName">The name of the port</param>
        /// <returns>Number of connected IDialogueTraceNode</returns>
        public static int GetConnectedTraceCount(INode node, string portName = NextPortName)
        {
            var nextPort = GetOutputPortByName(node, portName);
            if (nextPort == null) return 0;

            List<IPort> connectedPorts = new();
            nextPort.GetConnectedPorts(connectedPorts);

            var tracePorts = connectedPorts
                .Select(port => port.GetNode())
                .OfType<IDialogueTraceNode>().ToList();

            return tracePorts.Count;
        }

        /// <summary>
        /// Checks whether a given Trace node with a previous port is connected
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <param name="infos">Logger to send output info to</param>
        public static void CheckPreviousConnection(INode node, GraphLogger infos)
        {
            var prevPort = GetInputPortByName(node, PreviousPortName);
            if (prevPort == null) return;

            if (!prevPort.isConnected)
            {
                infos.LogWarning(
                    $"Node of type [{node.GetType().Name}] has no incoming connection on port '{PreviousPortName}'. " +
                    $"This node cannot be reached.", 
                    node);
            }
        }

        /// <summary>
        /// Checks whether a node has too many Trace nodes connected to its "next" port
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <param name="infos">Logger to send output info to</param>
        public static void MultipleOutputsCheck(INode node, GraphLogger infos)
        {
            int traceCount = GetConnectedTraceCount(node);

            if (traceCount > 1)
            {
                infos.LogError(
                    $"Node of type [{node.GetType().Name}] has {traceCount} connections on its '{NextPortName}' port. " +
                    $"Expected: 0 or 1 connection. Actual: {traceCount}.", 
                    node);
            }
        }

        /// <summary>
        /// Check whether a ContextNode has at least one option available
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <param name="infos">Logger to send output info to</param>
        public static void HasOptionsCheck(ContextNode node, GraphLogger infos)
        {
            if (node.blockCount == 0)
            {
                infos.LogWarning(
                    $"Context node of type [{node.GetType().Name}] has no options. " +
                    $"Expected: at least 1 option. Actual: {node.blockCount}.", 
                    node);
            }
        }
    }
}