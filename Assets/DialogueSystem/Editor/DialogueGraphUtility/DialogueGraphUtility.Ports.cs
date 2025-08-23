using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    internal static partial class DialogueGraphUtility
    {
        private const string NextPortName = "next";
        private const string PreviousPortName = "previous";
        private const string DataPortName = "data";

        private const string NextPortDefaultDisplayName = "Next";
        private const string PreviousPortDefaultDisplayName = "Previous";
        
        /// <summary>
        /// Defines a basic input port with an arrowhead connector.
        /// </summary>
        /// <remarks>
        /// - Minimal helper for creating a single input port by name.  
        /// - The port has no custom display name unless explicitly set.  
        /// </remarks>
        public static void DefineBasicInputPort(Node.IPortDefinitionContext context, string portName)
        {
            context.AddInputPort(portName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        /// <summary>
        /// Defines a basic input port with an arrowhead connector and a custom display name.
        /// </summary>
        public static void DefineBasicInputPort(Node.IPortDefinitionContext context, string portName, string displayName)
        {
            context.AddInputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        /// <summary>
        /// Defines the standard "previous" input port for dialogue nodes.
        /// </summary>
        /// <remarks>
        /// Uses the constant <c>PreviousPortName</c> and the default display name <c>PreviousPortDefaultDisplayName</c>.
        /// </remarks>
        public static void DefineNodeInputPort(Node.IPortDefinitionContext context)
        {
            DefineBasicInputPort(context, PreviousPortName, PreviousPortDefaultDisplayName);
        }
        
        /// <summary>
        /// Defines the standard "previous" input port with a custom display name.
        /// </summary>
        public static void DefineNodeInputPort(Node.IPortDefinitionContext context, string displayName)
        {
            DefineBasicInputPort(context, PreviousPortName, displayName);
        }
        
        /// <summary>
        /// Defines a basic output port with an arrowhead connector.
        /// </summary>
        /// <remarks>
        /// - Minimal helper for creating a single output port by name.  
        /// - The port has no custom display name unless explicitly set.  
        /// </remarks>
        public static void DefineBasicOutputPort(Node.IPortDefinitionContext context, string portName)
        {
            context.AddOutputPort(portName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        /// <summary>
        /// Defines a basic output port with an arrowhead connector and a custom display name.
        /// </summary>
        public static void DefineBasicOutputPort(Node.IPortDefinitionContext context, string portName, string displayName)
        {
            context.AddOutputPort(portName)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        
        /// <summary>
        /// Defines the standard "next" output port for dialogue nodes.
        /// </summary>
        /// <remarks>
        /// Uses the constant <c>NextPortName</c> and the default display name <c>NextPortDefaultDisplayName</c>.
        /// </remarks>
        public static void DefineNodeOutputPort(Node.IPortDefinitionContext context)
        {
            DefineBasicOutputPort(context, NextPortName, NextPortDefaultDisplayName);
        }
        
        /// <summary>
        /// Defines the standard "next" output port with a custom display name.
        /// </summary>
        public static void DefineNodeOutputPort(Node.IPortDefinitionContext context, string displayName)
        {
            DefineBasicOutputPort(context, NextPortName, displayName);
        }
        
        /// <summary>
        /// Defines a dedicated data input port for <c>IDataNode</c> connections.
        /// </summary>
        /// <remarks>
        /// - Uses a circular connector UI to differentiate it from standard dialogue flow ports.  
        /// - The display name is hidden (empty string).  
        /// </remarks>
        public static void DefineDataInputPort(Node.IPortDefinitionContext context)
        {
            context.AddInputPort(DataPortName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .WithDisplayName("")
                .Build();
        }
        
        /// <summary>
        /// Safely retrieves an input port by name without throwing if it doesn't exist.
        /// </summary>
        /// <remarks>
        /// - Wraps <c>INode.GetInputPortByName</c> in a <c>try/catch</c>.  
        /// - Returns <c>null</c> instead of propagating <c>KeyNotFoundException</c>.  
        /// </remarks>
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
        
        /// <summary>
        /// Safely retrieves an output port by name without throwing if it doesn't exist.
        /// </summary>
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

        /// <summary>
        /// Retrieves the standard "next" dialogue port from a node.
        /// </summary>
        /// <returns>The "next" output port, or <c>null</c> if it does not exist.</returns>
        public static IPort GetNextPortOrNull(INode node)
        {
            return GetOutputPortByName(node, NextPortName);
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