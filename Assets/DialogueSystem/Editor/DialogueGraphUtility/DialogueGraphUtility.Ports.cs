using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    public static partial class DialogueGraphUtility
    {
        public const string NextPortName = "next";
        public const string PreviousPortName = "previous";
        public const string EventPortName = "event";

        public const string NextPortDefaultDisplayName = "Next";
        public const string PreviousPortDefaultDisplayName = "Previous";
        
        public static void DefineNodeInputPort(Node.IPortDefinitionContext context)
        {
            context.AddInputPort(PreviousPortName)
                .WithDisplayName(PreviousPortDefaultDisplayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static void DefineNodeInputPort(Node.IPortDefinitionContext context, string displayName)
        {
            context.AddInputPort(PreviousPortName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        
        public static void DefineNodeOutputPort(Node.IPortDefinitionContext context)
        {
            context.AddOutputPort(NextPortName)
                .WithDisplayName(NextPortDefaultDisplayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static void DefineNodeOutputPort(Node.IPortDefinitionContext context, string displayName)
        {
            context.AddOutputPort(NextPortName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        public static void DefineEventInputPort(Node.IPortDefinitionContext context)
        {
            context.AddInputPort(EventPortName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .WithDisplayName("")
                .Build();
        }
        
        public static void DefineEventOutputPort(Node.IPortDefinitionContext context)
        {
            context.AddOutputPort(EventPortName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .WithDisplayName("Event")
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
            return node.GetNodeOptionByName(optionName)
                .TryGetValue<T>(out var value) ? value : default(T);
        }
    }
}