using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    internal static partial class DialogueGraphUtility
    {
        private const string NextPortName = "Next";
        private const string PreviousPortName = "Previous";
        private const string DataPortName = "Data";

        public static IPort AddPreviousPort(Node.IPortDefinitionContext context, string portName=null)
        {
            return context.AddInputPort(PreviousPortName)
                .WithDisplayName(portName ?? PreviousPortName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static IPort AddNextPort(Node.IPortDefinitionContext context, string portName=null)
        {
            return context.AddOutputPort(NextPortName)
                .WithDisplayName(portName ?? NextPortName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public static string GetVariableName(IPort port)
        {
            var connectedNode = port?.firstConnectedPort.GetNode();            
            
            if (connectedNode is not IVariableNode varNode) return null;

            return varNode.variable.name;
        }
        
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