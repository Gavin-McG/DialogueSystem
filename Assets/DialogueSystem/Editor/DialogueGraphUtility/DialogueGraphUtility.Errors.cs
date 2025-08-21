using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    public static partial class DialogueGraphUtility
    {
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

        public static void CheckPreviousConnection(INode node, GraphLogger infos)
        {
            var prevPort = GetInputPortByName(node, PreviousPortName);
            if (prevPort==null) return;
            
            if (!prevPort.isConnected)
                infos.LogWarning("Node cannot be reached", node);
        }
        
        public static void MultipleOutputsCheck(INode node, GraphLogger infos)
        {
            int traceCount = GetConnectedTraceCount(node);
                
            if (traceCount > 1)
                infos.LogError($"A Next Dialogue Port cannot exceed more than 1 trace Connection. ", node);
        }

        public static void HasOptionsCheck(ContextNode node, GraphLogger infos)
        {
            if (node.blockCount == 0)
                infos.LogWarning($"{node.GetType().Name} should have at least 1 option", node);
        }
    }
}