using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    public static partial class DialogueGraphUtility
    {
        private const string NextPortName = "next";
        private const string PreviousPortName = "previous";
        private const string EventPortName = "event";

        private const string NextPortDefaultDisplayName = "Next";
        private const string PreviousPortDefaultDisplayName = "Previous";
        
        
        public static void DefineBasicInputPort(Node.IPortDefinitionContext context, string portName)
        {
            context.AddInputPort(portName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static void DefineBasicInputPort(Node.IPortDefinitionContext context, string portName, string displayName)
        {
            context.AddInputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static void DefineNodeInputPort(Node.IPortDefinitionContext context)
        {
            DefineBasicInputPort(context, PreviousPortName, PreviousPortDefaultDisplayName);
        }
        
        public static void DefineNodeInputPort(Node.IPortDefinitionContext context, string displayName)
        {
            DefineBasicInputPort(context, PreviousPortName, displayName);
        }
        
        public static void DefineBasicOutputPort(Node.IPortDefinitionContext context, string portName)
        {
            context.AddOutputPort(portName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public static void DefineBasicOutputPort(Node.IPortDefinitionContext context, string portName, string displayName)
        {
            context.AddOutputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static void DefineNodeOutputPort(Node.IPortDefinitionContext context)
        {
            DefineBasicOutputPort(context, NextPortName, NextPortDefaultDisplayName);
        }
        
        public static void DefineNodeOutputPort(Node.IPortDefinitionContext context, string displayName)
        {
            DefineBasicOutputPort(context, NextPortName, displayName);
        }
        
        public static void DefineEventInputPort(Node.IPortDefinitionContext context)
        {
            context.AddInputPort(EventPortName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .WithDisplayName("")
                .Build();
        }
        
        private static IPort GetInputPortByName(INode node, string portName)
        {
            try
            {
                return node.GetInputPortByName(portName);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        
        private static IPort GetOutputPortByName(INode node, string portName)
        {
            try
            {
                return node.GetOutputPortByName(portName);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public static IPort GetNextPortOrNull(INode node)
        {
            return GetOutputPortByName(node, NextPortName);
        }
        
        public static T GetOptionValueOrDefault<T>(Node node, string optionName)
        {
            var option = node.GetNodeOptionByName(optionName);
            if (option == null) return default(T);
            
            return option.TryGetValue<T>(out var value) ? value : default(T);
        }
        
        public static T GetPortValueOrDefault<T>(INode node, string portName)
        {
            //get port for object
            var port = GetInputPortByName(node, portName);
            if (port == null) return default;
            
            //try to get object value from connected variable
            var connectedPort = port?.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    //try to get value assigned directly if no connected node
                    return port.TryGetValue(out T attachedObject) ? attachedObject : default;
                case IVariableNode variableNode:
                    //return value assigned from variable node
                    return variableNode.variable
                        .TryGetDefaultValue(out T varObject) ? varObject : default;
                default:
                    return default;
            }
        }
    }
}