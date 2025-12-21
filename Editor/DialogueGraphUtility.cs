using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    internal static partial class DialogueGraphUtility
    {
        private const string NextPortName = "Next";
        private const string PreviousPortName = "Previous";
        
        //---------Port Adders-------------

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
        
        //--------Port Getters------------
        
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
        
        //--------Variables-------------
        
        private const string VariablePattern = @"\{(.*?)\}";
        
        public static List<string> GetVariableNames(string text)
        {
            List<string> result = new List<string>();
            if (text==null) return result;
            
            foreach (Match match in Regex.Matches(text, VariablePattern))
            {
                result.Add(match.Groups[1].Value);
            }
            return result.Distinct().ToList();
        }
        
        //----------Events---------------
        
        private static Type GetDSEventValueType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DSEvent<>))
                    return type.GetGenericArguments()[0];

                type = type.BaseType;
            }

            return null;
        }

        private static Type GetEventType(INodeOption eventOption)
        {
            if (eventOption == null) return null;
            eventOption.TryGetValue(out DSEventBase currentEvent);
            
            //DSEvent has no value type
            if (currentEvent is DSEvent)
                return null;
            
            //Get DSEvent<T> template type
            return GetDSEventValueType(currentEvent?.GetType());
        }

        public static INodeOption DefineEventOption(Node.IOptionDefinitionContext context)
        {
            return context.AddOption<DSEventBase>("Event").WithDisplayName("").Build();
        }

        public static INodeOption DefineEventValueOption(Node.IOptionDefinitionContext context, INodeOption eventOption)
        {
            Type valueType = GetEventType(eventOption);
            if (valueType != null)
                return context.AddOption("value", valueType).Build();
            return null;
        }

        public static EventReference GetEventCaller(INodeOption eventOption, INodeOption valueOption)
        {
            var valueType = GetEventType(eventOption);
            if (valueType != null)
            {
                // Create EventCaller<T>
                Type callerType = typeof(EventCaller<>).MakeGenericType(valueType);
                var caller = (EventReference)Activator.CreateInstance(callerType);

                // Assign dialogueEvent
                eventOption.TryGetValue(out DSEventBase dialogueEvent);
                callerType
                    .GetField("dialogueEvent")
                    ?.SetValue(caller, dialogueEvent);

                // Assign value
                if (valueOption != null)
                {
                    valueOption.TryGetValue(out object value);
                    callerType
                        .GetField("value")
                        ?.SetValue(caller, value);
                }

                return caller;
            }
            else
            {
                var caller = new EventCaller();
                eventOption.TryGetValue(out caller.dialogueEvent);
                return caller;
            }
        }
    }
}