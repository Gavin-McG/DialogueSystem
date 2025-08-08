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

        private const string FieldSeperator = "/";
        
        private static readonly HashSet<Type> SupportedNodeOptionTypes = new()
        {
            typeof(long),
            typeof(int),
            typeof(bool),
            typeof(float),
            typeof(double),
            typeof(string),
            typeof(Color),
            typeof(GameObject),
            typeof(UnityEngine.Object),
            typeof(LayerMask),
            typeof(char),
            typeof(AnimationCurve),
            typeof(Bounds),
            typeof(Gradient),
            typeof(Vector2Int),
            typeof(Vector3Int),
            typeof(Vector4),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Rect),
            typeof(RectInt),
            typeof(BoundsInt)
        };
        
        private static bool IsBasicSupportedType(Type type)
        {
            return SupportedNodeOptionTypes.Contains(type)
                   || type.IsEnum;
        }
        
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
            // Replace separator with dot
            var displayName = fieldName.Replace(FieldSeperator, ".");

            // Add spaces before capital letters (camelCase to "camel Case")
            displayName = Regex.Replace(displayName, "(?<!^)([A-Z])", " $1");

            // Capitalize the first letter of each dot-separated section
            var parts = displayName.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }

            return string.Join(".", parts);
        }

        public static void DefineFieldOptions<T>(INodeOptionDefinition context)
        {
            var type = typeof(T);

            if (IsBasicSupportedType(type))
            {
                context.AddNodeOption("value", type, FieldNameToDisplayName("value"));
                return;
            }

            var fields = GetOptionFieldsRecursive<T>();

            foreach (var (path, field, _) in fields)
            {
                var tooltipAttribute = field.GetCustomAttribute<TooltipAttribute>();
                var tooltip = tooltipAttribute?.tooltip;
                var displayName = FieldNameToDisplayName(path);

                context.AddNodeOption(path, field.FieldType, displayName, tooltip);
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
                
                var dialoguePortAttribute = field.GetCustomAttribute<DialoguePortAttribute>();
                if (dialoguePortAttribute.DisplayName != null) 
                    displayName = dialoguePortAttribute.DisplayName;

                context.AddInputPort(fieldName)
                    .WithDataType(fieldType)
                    .WithDisplayName(displayName)
                    .Build();
            }
        }

        public static T AssignFromFieldOptions<T>(Node node)
        {
            var type = typeof(T);
            object obj = default(T);

            if (IsBasicSupportedType(type))
            {
                var option = node.GetNodeOptionByName("value");
                if (option == null)
                    return (T)(obj ?? default(T));

                var tryGetValue = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))
                    ?.MakeGenericMethod(type);

                object[] parameters = { null };
                if ((bool)(tryGetValue?.Invoke(option, parameters) ?? false))
                {
                    return (T)parameters[0];
                }

                return (T)(type.IsValueType ? Activator.CreateInstance(type) : null);
            }

            // Create new instance for both class and struct types
            obj = Activator.CreateInstance(typeof(T));

            var fields = DialogueGraphUtility.GetOptionFieldsRecursive<T>();
            foreach (var (path, field, _) in fields)
            {
                var option = node.GetNodeOptionByName(path);
                if (option == null)
                    continue;

                var fieldType = field.FieldType;
                var tryGetValue = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))
                    ?.MakeGenericMethod(fieldType);

                object[] parameters = { null };
                bool success = (bool)(tryGetValue?.Invoke(option, parameters) ?? false);

                // Traverse to target object (deep field support)
                object target = obj;
                string[] parts = path.Split(FieldSeperator);
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    var info = target.GetType().GetField(parts[i], FieldFlags);
                    var val = info.GetValue(target);
                    if (val == null)
                    {
                        val = Activator.CreateInstance(info.FieldType);
                        info.SetValue(target, val);
                    }
                    target = val;
                }

                var targetField = target.GetType().GetField(parts.Last(), FieldFlags);
                targetField?.SetValue(target, success
                    ? parameters[0]
                    : fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null);
            }

            return (T)obj;
        }
        
        public static void AssignFromFieldOptions<T>(Node node, ref T obj) where T : ScriptableObject
        {
            var type = typeof(T);

            if (IsBasicSupportedType(type))
            {
                var option = node.GetNodeOptionByName("value");
                if (option == null) return;

                var tryGetValue = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))
                    ?.MakeGenericMethod(type);

                object[] parameters = { null };
                if ((bool)(tryGetValue?.Invoke(option, parameters) ?? false))
                {
                    obj = parameters[0] as T;
                }
                else if (obj == null)
                {
                    obj = ScriptableObject.CreateInstance<T>();
                }

                return;
            }

            if (obj == null)
                obj = ScriptableObject.CreateInstance<T>();

            var fields = DialogueGraphUtility.GetOptionFieldsRecursive<T>();
            foreach (var (path, field, _) in fields)
            {
                var option = node.GetNodeOptionByName(path);
                if (option == null)
                    continue;

                var fieldType = field.FieldType;
                var tryGetValue = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))
                    ?.MakeGenericMethod(fieldType);

                object[] parameters = { null };
                bool success = (bool)(tryGetValue?.Invoke(option, parameters) ?? false);

                // Traverse to target object
                object target = obj;
                string[] parts = path.Split(FieldSeperator);
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    var info = target.GetType().GetField(parts[i], FieldFlags);
                    var val = info.GetValue(target);
                    if (val == null)
                    {
                        val = Activator.CreateInstance(info.FieldType);
                        info.SetValue(target, val);
                    }
                    target = val;
                }

                var targetField = target.GetType().GetField(parts.Last(), FieldFlags);
                targetField?.SetValue(target, success
                    ? parameters[0]
                    : fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null);
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
        
        
        public static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetOptionFieldsRecursive(Type type, string parentPath = "")
        {
            var fields = type.GetFields(FieldFlags);

            foreach (var field in fields)
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;
                bool isPort = field.GetCustomAttribute<DialoguePortAttribute>() != null && typeof(DialogueObject).IsAssignableFrom(field.FieldType);

                if (!(isPublic || isSerializedPrivate) || isHidden || isPort)
                    continue;

                string fullPath = string.IsNullOrEmpty(parentPath) ? field.Name : $"{parentPath}{FieldSeperator}{field.Name}";

                yield return (fullPath, field, type);

                if (!IsBasicSupportedType(field.FieldType) &&
                    !field.FieldType.IsPrimitive &&
                    !field.FieldType.IsEnum &&
                    !typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType) &&
                    !field.FieldType.IsArray)
                {
                    foreach (var subField in GetOptionFieldsRecursive(field.FieldType, fullPath))
                        yield return subField;
                }
            }
        }

        public static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetOptionFieldsRecursive<T>()
        {
            return GetOptionFieldsRecursive(typeof(T));
        }
    }
}