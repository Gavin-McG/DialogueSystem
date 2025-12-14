using System;
using System.Collections.Generic;
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
    }
}