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
        
        public static IPort AddInputPort(Node.IPortDefinitionContext context, string portName, Type type=null, string displayName = null, PortConnectorUI portUI=PortConnectorUI.Circle, object defaultValue = null) 
        {
            var builder = context.AddInputPort(portName)
                .WithConnectorUI(portUI);
            
            if (displayName != null)
                builder = builder.WithDisplayName(displayName);

            if (type != null)
            {
                var typedBuilder = builder.WithDataType(type);
                if (defaultValue != null)
                    typedBuilder.WithDefaultValue(defaultValue);
                return typedBuilder.Build();
            }
            
            return builder.Build();
        }
        public static IPort AddOutputPort(Node.IPortDefinitionContext context, string portName, Type type=null, string displayName = null, PortConnectorUI portUI=PortConnectorUI.Circle) 
        {
            var builder = context.AddOutputPort(portName)
                .WithConnectorUI(portUI);
            
            if (displayName != null)
                builder = builder.WithDisplayName(displayName);

            if (type != null)
            {
                var typedBuilder = builder.WithDataType(type);
                return typedBuilder.Build();
            }
            
            return builder.Build();
        }

        public static IPort AddNextPort(Node.IPortDefinitionContext context, string portName = NextPortName) =>
            AddOutputPort(context, portName, portUI: PortConnectorUI.Arrowhead);
        public static IPort AddPreviousPort(Node.IPortDefinitionContext context, string portName = PreviousPortName) =>
            AddInputPort(context, portName, portUI: PortConnectorUI.Arrowhead);
        public static IPort AddDataInputPort(Node.IPortDefinitionContext context, string portName = DataPortName) => 
            AddInputPort(context, portName);
        

        private static IPort GetInputPortByName(INode node, string portName)
        {
            try {
                return node.GetInputPortByName(portName);
            } catch (KeyNotFoundException) {
                return null;
            }
        }
        
        private static IPort GetOutputPortByName(INode node, string portName)
        {
            try {
                return node.GetInputPortByName(portName);
            } catch (KeyNotFoundException) {
                return null;
            }
        }
        
        
        
        /// <summary>
        /// Retrieves the value assigned to a node option, or a default if unavailable.
        /// </summary>
        /// <remarks>
        /// - If the option is missing, returns <c>default(T)</c>.  
        /// - Uses <c>INodeOption.TryGetValue&lt;T&gt;</c> internally.  
        /// - Safe against type mismatches (returns default instead of throwing).  
        /// </remarks>
        public static T GetOptionValueOrDefault<T>(Node node, string optionName)
        {
            var option = node.GetNodeOptionByName(optionName);
            if (option == null) return default;
            
            return option.TryGetValue<T>(out var value) ? value : default;
        }
        
        /// <summary>
        /// Retrieves the value assigned to a port, either directly or through a connected variable node.
        /// </summary>
        /// <remarks>
        /// Resolution order:
        /// 1. If no port exists → returns <c>default(T)</c>.  
        /// 2. If port has no connected node → tries to use its directly assigned value.  
        /// 3. If connected to an <c>IVariableNode</c> → retrieves its default variable value.  
        /// 4. Otherwise → returns <c>default(T)</c>.  
        /// </remarks>
        public static T GetPortValueOrDefault<T>(INode node, string portName)
        {
            // Get port reference
            var port = GetInputPortByName(node, portName);
            if (port == null) return default;
            
            // Check connected port and node
            var connectedPort = port.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    // No connection → use directly assigned value
                    return port.TryGetValue(out T attachedObject) ? attachedObject : default;
                case IVariableNode variableNode:
                    // Connected to variable node → use its default value
                    return variableNode.variable
                        .TryGetDefaultValue(out T varObject) ? varObject : default;
                default:
                    return default;
            }
        }


        public static INodeOption AddNodeOption(Node.IOptionDefinitionContext context, 
            string optionName, Type type, string displayName=null, object defaultValue=null, string tooltip=null)
        {
            var builder = context.AddOption(optionName, type);
                
            if (displayName != null)
                builder = builder.WithDisplayName(displayName);
                    
            if (defaultValue != null)
                builder = builder.WithDefaultValue(defaultValue);
            
            if (tooltip != null)
                builder = builder.WithTooltip(tooltip);
            
            return builder.Build();
        }
    }
}