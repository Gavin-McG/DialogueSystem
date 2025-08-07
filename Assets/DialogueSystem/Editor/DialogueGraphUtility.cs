using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{

    public static class DialogueGraphUtility
    {
        public const string NextPortName = "next";
        public const string PreviousPortName = "previous";
        public const string EventPortName = "event";

        public const string NextPortDefaultDisplayName = "Next";
        public const string PreviousPortDefaultDisplayName = "Previous";
        
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
        

        public static T GetDialogueObjectValueOrNull<T>(INode node, string portName,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict) where T : DialogueObject
        {
            //get port for object
            var port = GetInputPortByName(node, portName);
            if (port == null) return null;
            
            //try to get profile value from connected variable
            var connectedPort = port?.firstConnectedPort;
            var connectedNode = connectedPort?.GetNode();

            switch (connectedNode)
            {
                case null:
                    //try to get value assigned directly if no connected node
                    return port.TryGetValue(out T attachedObject) ? attachedObject : null;
                case IVariableNode variableNode:
                    //return value assigned from variable node
                    return variableNode.variable
                        .TryGetDefaultValue(out T varObject) ? varObject : null;
                case GenericObjectNode<T> objectNode:
                    //return created profile object from profile node
                    return GetObjectFromNode<T>(objectNode, dialogueDict);
                default:
                    return null;
            }
        }

        public static T GetOptionValueOrDefault<T>(Node node, string optionName)
        {
            return node.GetNodeOptionByName(optionName)
                .TryGetValue<T>(out var value) ? value : default(T);
        }

        public static IPort GetInputPortByName(INode node, string portName)
        {
            try
            {
                return node.GetInputPortByName(portName);
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }
        
        public static IPort GetOutputPortByName(INode node, string portName)
        {
            try
            {
                return node.GetOutputPortByName(portName);
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        public static IPort GetNextPortOrNull(INode node)
        {
           return GetOutputPortByName(node, NextPortName);
        }
        
        public static IPort GetEventPortOrNull(INode node)
        {
            return GetOutputPortByName(node, EventPortName);
        }

        public static List<DialogueEvent> GetEvents(INode node)
        {
            var eventPort = GetEventPortOrNull(node);

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
        
        public static IEnumerable<FieldInfo> GetOptionFields<T>()
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return typeof(T).GetFields(flags).Where(field =>
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;
                bool isPort = field.GetCustomAttribute<DialoguePortAttribute>() != null && typeof(DialogueObject).IsAssignableFrom(field.FieldType);

                return (isPublic || isSerializedPrivate) && !isHidden && !isPort;
            });
        }
        
        public static IEnumerable<FieldInfo> GetPortFields<T>()
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return typeof(T).GetFields(flags).Where(field =>
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;
                bool isPort = field.GetCustomAttribute<DialoguePortAttribute>() != null && typeof(DialogueObject).IsAssignableFrom(field.FieldType);

                return (isPublic || isSerializedPrivate) && !isHidden && isPort;
            });
        }

        public static void DefineFieldOptions<T>(INodeOptionDefinition context)
        {
            var fields = DialogueGraphUtility.GetOptionFields<T>();
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                var fieldType = field.FieldType;
                var displayName = Regex.Replace(field.Name, "(?<!^)([A-Z])", " $1");
                displayName = char.ToUpper(displayName[0]) + displayName.Substring(1);

                var tooltipAttribute = field.GetCustomAttribute<TooltipAttribute>();
                var tooltip = tooltipAttribute?.tooltip;

                context.AddNodeOption(fieldName, fieldType, displayName, tooltip);
            }
        }

        public static void DefineFieldPorts<T>(Node.IPortDefinitionContext context)
        {
            var fields = DialogueGraphUtility.GetPortFields<T>();
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                var fieldType = field.FieldType;
                var displayName = Regex.Replace(field.Name, "(?<!^)([A-Z])", " $1");
                displayName = char.ToUpper(displayName[0]) + displayName.Substring(1);

                context.AddInputPort(fieldName)
                    .WithDataType(fieldType)
                    .WithDisplayName(displayName)
                    .Build();
            }
        }

        public static void AssignFromFieldOptions<T>(Node node, ref T obj) where T : class
        {
            var fields = DialogueGraphUtility.GetOptionFields<T>();
            foreach (var field in fields)
            {
                var option = node.GetNodeOptionByName(field.Name);
                if (option == null)
                    continue;

                var fieldType = field.FieldType;

                var tryGetValueMethod = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))?
                    .MakeGenericMethod(fieldType);

                object[] parameters = { null };
                bool success = (bool)tryGetValueMethod.Invoke(option, parameters);

                if (success)
                {
                    field.SetValue(obj, parameters[0]);
                }
                else
                {
                    field.SetValue(obj, fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null);
                }
            }
        }

        public static void AssignFromFieldPorts<T>(Node node, 
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict, ref T obj) where T : class
        {
            var fields = DialogueGraphUtility.GetPortFields<T>();
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                
                var getDialogueObjectMethod = typeof(DialogueGraphUtility)
                    .GetMethod(nameof(GetDialogueObjectValueOrNull))?
                    .MakeGenericMethod(fieldType);
                
                object[] parameters = { node, field.Name, dialogueDict };
                
                var value = getDialogueObjectMethod?.Invoke(null, parameters);
                
                field.SetValue(obj, value);
            }
        }

        public static void AssignFromNode<T>(Node node,
            Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict, ref T obj) where T : class
        {
            AssignFromFieldOptions(node, ref obj);
            AssignFromFieldPorts(node, dialogueDict, ref obj);
        }
    }

}
