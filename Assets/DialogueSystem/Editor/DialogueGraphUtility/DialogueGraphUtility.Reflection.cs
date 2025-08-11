using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        
        public static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetOptionFieldsRecursive(
            Type type, 
            string parentPath = "", 
            HashSet<Type> ancestorTypes = null)
        {
            ancestorTypes ??= new HashSet<Type>();
    
            // Add current type to ancestor chain
            if (!ancestorTypes.Add(type))
            {
                // Type already encountered in this chain — avoid infinite recursion
                yield break;
            }

            var fields = type.GetFields(FieldFlags);

            foreach (var field in fields)
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null ||
                                field.GetCustomAttribute<HideInDialogueGraphAttribute>() != null;
                bool isPort = typeof(ScriptableObject).IsAssignableFrom(field.FieldType);

                if (!(isPublic || isSerializedPrivate) || isHidden || isPort)
                    continue;

                string fullPath = string.IsNullOrEmpty(parentPath) ? field.Name : $"{parentPath}{FieldSeperator}{field.Name}";

                yield return (fullPath, field, type);

                var fieldType = field.FieldType;
                if (!IsBasicSupportedType(fieldType) &&
                    !fieldType.IsPrimitive &&
                    !typeof(UnityEngine.Object).IsAssignableFrom(fieldType) &&
                    !fieldType.IsArray)
                {
                    // Pass a copy of ancestorTypes for recursion, 
                    // or use the same set if you remove type after recursion (see below)
                    foreach (var subField in GetOptionFieldsRecursive(fieldType, fullPath, ancestorTypes))
                        yield return subField;
                }
            }

            // Remove current type to allow sibling branches to explore it
            ancestorTypes.Remove(type);
        }
        
        public static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetOptionFieldsRecursive<T>()
        {
            return GetOptionFieldsRecursive(typeof(T));
        }
        
        public static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetPortFieldsRecursive(
            Type type, 
            string parentPath = "", 
            HashSet<Type> ancestorTypes = null)
        {
            ancestorTypes ??= new HashSet<Type>();
    
            // Add current type to ancestor chain
            if (!ancestorTypes.Add(type))
            {
                // Type already encountered in this chain — avoid infinite recursion
                yield break;
            }

            var fields = type.GetFields(FieldFlags);

            foreach (var field in fields)
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null ||
                                field.GetCustomAttribute<HideInDialogueGraphAttribute>() != null;
                bool isPort = typeof(ScriptableObject).IsAssignableFrom(field.FieldType);
                
                string fullPath = string.IsNullOrEmpty(parentPath) ? field.Name : $"{parentPath}{FieldSeperator}{field.Name}";

                if ((isPublic || isSerializedPrivate) && !isHidden && isPort)
                {
                    yield return (fullPath, field, type);
                    continue;
                }

                var fieldType = field.FieldType;
                if (!IsBasicSupportedType(fieldType) &&
                    !fieldType.IsPrimitive &&
                    !typeof(UnityEngine.Object).IsAssignableFrom(fieldType) &&
                    !fieldType.IsArray)
                {
                    // Pass a copy of ancestorTypes for recursion, 
                    // or use the same set if you remove type after recursion (see below)
                    foreach (var subField in GetPortFieldsRecursive(fieldType, fullPath, ancestorTypes))
                        yield return subField;
                }
            }

            // Remove current type to allow sibling branches to explore it
            ancestorTypes.Remove(type);
        }
        
        public static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetPortFieldsRecursive<T>()
        {
            return GetPortFieldsRecursive(typeof(T));
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

                var attributes = field.GetCustomAttributes().ToArray();
                var defaultValue = field.GetCustomAttribute<DefaultValueAttribute>()?.Value;

                context.AddNodeOption(path, field.FieldType, displayName, tooltip,
                    attributes: attributes, defaultValue: defaultValue);
            }
        }

        public static void DefineFieldPorts<T>(Node.IPortDefinitionContext context)
        {
            var type = typeof(T);
            
            var fields = GetPortFieldsRecursive(type);

            foreach (var (path, field, _) in fields)
            {
                var displayName = FieldNameToDisplayName(path);

                var builder = context.AddInputPort(path)
                    .WithDataType(field.FieldType)
                    .WithDisplayName(displayName);
                    
                var defaultValueAttribute = field.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttribute != null)
                {
                    builder.WithDefaultValue(defaultValueAttribute.Value);
                    Debug.Log(defaultValueAttribute.Value?.ToString());
                }

                builder.Build();
            }
        }
        
        private static void SetNestedFieldValue(object root, string[] pathParts, object value)
        {
            object current = root;
            var stack = new Stack<(object parent, FieldInfo field)>();

            // Traverse down to the target field
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                var field = current.GetType().GetField(pathParts[i], FieldFlags);
                var fieldValue = field.GetValue(current);

                if (fieldValue == null)
                {
                    fieldValue = Activator.CreateInstance(field.FieldType);
                    field.SetValue(current, fieldValue);
                }

                stack.Push((current, field));
                current = fieldValue;
            }

            // Set the final field
            var finalField = current.GetType().GetField(pathParts[^1], FieldFlags);
            finalField?.SetValue(current, value);

            // Reassign structs back up the chain
            while (stack.Count > 0)
            {
                var (parent, field) = stack.Pop();
                field.SetValue(parent, current);
                current = parent;
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

            obj = Activator.CreateInstance(typeof(T));

            var fields = GetOptionFieldsRecursive<T>();
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

                var valueToAssign = success
                    ? parameters[0]
                    : fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null;

                SetNestedFieldValue(obj, path.Split(FieldSeperator), valueToAssign);
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

            var fields = GetOptionFieldsRecursive<T>();
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

                var valueToAssign = success
                    ? parameters[0]
                    : fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null;

                SetNestedFieldValue(obj, path.Split(FieldSeperator), valueToAssign);
            }
        }

        public static T AssignFromFieldPorts<T>(Node node, 
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict, ref T obj)
        {
            var fields = GetPortFieldsRecursive<T>();
            foreach (var (path, field, _) in fields)
            {
                var port = node.GetInputPortByName(path);
                if (port == null)
                    continue;
                
                var fieldType = field.FieldType;
                var getDialogueObjectMethod = typeof(DialogueGraphUtility)
                    .GetMethod(nameof(GetDialogueObjectValueOrNull))?
                    .MakeGenericMethod(fieldType);
                
                object[] parameters = { node, path, dialogueDict };
                object valueToAssign = getDialogueObjectMethod?.Invoke(port, parameters);

                SetNestedFieldValue(obj, path.Split(FieldSeperator), valueToAssign);
            }
            
            return obj;
        }
        
        
    }
}