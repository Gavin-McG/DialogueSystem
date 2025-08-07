using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{

    public static class DialogueGraphUtility
    {
        public const string NextPortName = "next";
        public const string PreviousPortName = "previous";
        public const string ProfilePortName = "profile";
        public const string EventPortName = "event";

        public const string NextPortDefaultDisplayName = "Next";
        public const string PreviousPortDefaultDisplayName = "Previous";
        public const string ProfilePortDefaultDisplayName = "Profile";
        
        public static DialogueObject GetObject(IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogueObject = dialogueDict.GetValueOrDefault(dialogueObjectNode);
            return dialogueObject;
        }

        public static T GetObject<T>(IDialogueObjectNode dialogueObjectNode,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            var dialogueObject = GetObject(dialogueObjectNode, dialogueDict);
            return (dialogueObject is T objectAsT) ? objectAsT : null;
        }
        
        public static DialogueObject GetObjectFromNode(INode node,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return  GetObject(dialogueObjectNode, dialogueDict);
        }
        
        public static T GetObjectFromNode<T>(INode node,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            if (node is not IDialogueObjectNode dialogueObjectNode) return null;
            return  GetObject<T>(dialogueObjectNode, dialogueDict);
        }

        public static DialogueObject GetConnectedObject(INode node, string portName,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogueObjectNode = GetConnectedObjectNode(node, portName);
            if (dialogueObjectNode == null) return null;
            return GetObject(dialogueObjectNode, dialogueDict);
        }

        public static T GetConnectedObject<T>(INode node, string portName,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            var dialogueObject = GetConnectedObject(node, portName, dialogueDict);
            return (dialogueObject is T objectAsT) ? objectAsT : null;
        }

        private static IDialogueObjectNode GetConnectedObjectNode(INode node, string portName)
        {
            var port = node.GetOutputPortByName(portName);
            var connectedPort = port?.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            if (connectedNode is IDialogueObjectNode dialogueObjectNode)
            {
                return dialogueObjectNode;
            }
            
            return null;
        }

        public static DialogueTrace GetConnectedDialogue(INode node, 
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogueObject = GetConnectedObject(node, NextPortName, dialogueDict);
            return dialogueObject is DialogueTrace dialogueTrace ? dialogueTrace : null;
        }

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

        public static void DefineProfileInputPort(Node.IPortDefinitionContext context)
        {
            context.AddInputPort<DialogueProfile>(ProfilePortName)
                .WithDisplayName(ProfilePortDefaultDisplayName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }
        
        public static void DefineProfileOutputPort(Node.IPortDefinitionContext context)
        {
            context.AddOutputPort<DialogueProfile>(ProfilePortName)
                .WithDisplayName(ProfilePortDefaultDisplayName)
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }

        public static DialogueProfile GetProfileValueOrNull(INode node,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            //get port for profile
            var port = GetInputPortOrNull(node, ProfilePortName);
            if (port == null) return null;
            
            //try to get profile value from connected variable
            var connectedPort = port?.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    //try to get value assigned directly if no connected node
                    return port.TryGetValue(out DialogueProfile attachedProfile) ? attachedProfile : null;
                case IVariableNode variableNode:
                    //return value assigned from variable node
                    return variableNode.variable
                        .TryGetDefaultValue(out DialogueProfile varProfile) ? varProfile : null;
                case DialogueProfileNode profileNode:
                    //return created profile object from profile node
                    return GetObjectFromNode<DialogueProfile>(profileNode, dialogueDict);
                default:
                    return null;
            }
        }

        public static T GetOptionValueOrDefault<T>(Node node, string optionName)
        {
            return node.GetNodeOptionByName(optionName)
                .TryGetValue<T>(out var value) ? value : default(T);
        }
        
        public static IPort GetInputPortOrNull(INode node, string portName)
        {
            var ports = node.GetInputPorts()
                .Where(port => port.name == portName)
                .ToList();
            return (ports.Count == 1) ? ports[0] : null;
        }

        public static IPort GetOutputPortOrNull(INode node, string portName)
        {
            var ports = node.GetOutputPorts()
                .Where(port => port.name == portName)
                .ToList();
            return (ports.Count == 1) ? ports[0] : null;
        }

        public static IPort GetNextPortOrNull(INode node)
        {
            return GetOutputPortOrNull(node, NextPortName);
        }
        
        public static IPort GetEventPortOrNull(INode node)
        {
            return GetOutputPortOrNull(node, EventPortName);
        }

        public static List<DialogueEvent> GetEvents(INode node)
        {
            var eventPort = node.GetOutputPortByName(EventPortName);

            var connectedEventPorts = new List<IPort>();
            eventPort.GetConnectedPorts(connectedEventPorts);
            var eventNodes = connectedEventPorts.Select(port => port.GetNode());

            var events = new List<DialogueEvent>();
            foreach (var eventNode in eventNodes)
            {
                if (eventNode is DialogueEventNode dialogueEventNode && dialogueEventNode.GetEvent())
                {
                    events.Add(dialogueEventNode.GetEvent());
                }
            }
            return events;
        }
    }

}
