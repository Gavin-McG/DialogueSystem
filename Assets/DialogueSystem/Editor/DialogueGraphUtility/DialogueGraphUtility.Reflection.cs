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
    public static partial class DialogueGraphUtility
    {
        private const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        private static IEnumerable<FieldInfo> GetOptionFields<T>()
        {
            return typeof(T).GetFields(FieldFlags).Where(field =>
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;
                bool isPort = field.GetCustomAttribute<DialoguePortAttribute>() != null && typeof(DialogueObject).IsAssignableFrom(field.FieldType);

                return (isPublic || isSerializedPrivate) && !isHidden && !isPort;
            });
        }
        
        private static IEnumerable<FieldInfo> GetPortFields<T>()
        {
            return typeof(T).GetFields(FieldFlags).Where(field =>
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;
                bool isPort = field.GetCustomAttribute<DialoguePortAttribute>() != null && typeof(DialogueObject).IsAssignableFrom(field.FieldType);

                return (isPublic || isSerializedPrivate) && !isHidden && isPort;
            });
        }

        private static string FieldNameToDisplayName(string fieldName)
        {
            var displayName = Regex.Replace(fieldName, "(?<!^)([A-Z])", " $1");
            displayName = char.ToUpper(displayName[0]) + displayName.Substring(1);
            return displayName;
        }

        public static void DefineFieldOptions<T>(INodeOptionDefinition context)
        {
            var fields = DialogueGraphUtility.GetOptionFields<T>();
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                var fieldType = field.FieldType;
                var displayName = FieldNameToDisplayName(fieldName);

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
                var displayName = FieldNameToDisplayName(fieldName);

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
    }
}