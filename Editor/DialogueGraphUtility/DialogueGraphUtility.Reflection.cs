using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Editor
{
    internal static partial class DialogueGraphUtility
    {
        // BindingFlags to use when reflecting fields for node option/port generation
        private const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // Character to separate nested field names when building paths
        private const string FieldSeparator = "/";

        // Types that the Graph Toolkit inspector natively supports as editable options
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
                   || type.IsEnum
                   || type.IsPrimitive 
                   || typeof(UnityEngine.Object).IsAssignableFrom(type);
        }

        
        public static string FieldNameToDisplayName(string fieldName)
        {
            // Replace path separators with dots for readability
            var displayName = fieldName.Replace(FieldSeparator, ".");

            // Insert spaces before uppercase letters for camelCase → "camel Case"
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
        
        
        private static bool IsUnitySerializableType(Type t)
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(t))
                return true;
            if (t.IsPrimitive || t == typeof(string))
                return true;
            if (t.IsEnum)
                return true;
            if (t.IsValueType && t.IsDefined(typeof(SerializableAttribute), false))
                return true;
            if (t.IsClass && t.IsDefined(typeof(SerializableAttribute), false))
                return true;

            return false;
        }

        private static IEnumerable<FieldInfo> GetTypeFields(Type type)
        {
            return type.GetFields(FieldFlags)
                .Where(field =>
                {
                    bool isPublic = field.IsPublic;
                    bool hasSerializeField   = field.IsDefined(typeof(SerializeField), false);
                    bool hasNonSerialized    = field.IsDefined(typeof(NonSerializedAttribute), false);
                    bool hasHideInInspector  = field.IsDefined(typeof(HideInInspector), false);
                    bool hasHideInGraph      = field.IsDefined(typeof(HideInDialogueGraphAttribute), false);

                    // Unity-style serializable type check (not just .NET serializable)
                    bool isUnitySerializable = IsUnitySerializableType(field.FieldType);

                    bool allowField = (isPublic || hasSerializeField) && isUnitySerializable;
                    bool disAllowField = hasNonSerialized || hasHideInInspector || hasHideInGraph;

                    return allowField && !disAllowField;
                });
        }


        private static IEnumerable<(string path, FieldInfo fieldInfo)> GetTypeFieldsRecursive(
            Type type,
            string parentPath = "",
            HashSet<Type> ancestorTypes = null)
        {
            ancestorTypes ??= new HashSet<Type>();

            // If type has already been visited in this recursion branch, stop (prevents infinite recursion on cyclic references)
            if (!ancestorTypes.Add(type))
                yield break;
            
            //Get all the fields of this type to be shown in DialogueGraph inspector
            var fields = GetTypeFields(type);

            foreach (var field in fields)
            {
                string fullPath = string.IsNullOrEmpty(parentPath)
                    ? field.Name
                    : $"{parentPath}{FieldSeparator}{field.Name}";

                if (IsBasicSupportedType(field.FieldType))
                {
                    yield return (fullPath, field);
                }
                else
                {
                    foreach (var subfield in GetTypeFieldsRecursive(field.FieldType, fullPath, ancestorTypes))
                    {
                        yield return subfield;
                    }
                }
            }
            
            // Remove current type so sibling branches can explore it again
            ancestorTypes.Remove(type);
        }
        
        public static IEnumerable<(string path, FieldInfo fieldInfo)> GetOptionFields(Type type) =>
            GetTypeFieldsRecursive(type).Where(entry => 
                !entry.fieldInfo.IsDefined(typeof(DialoguePortAttribute), false)
            );
        
        public static IEnumerable<(string path, FieldInfo fieldInfo)> GetPortFields(Type type) =>
            GetTypeFieldsRecursive(type).Where(entry => 
                entry.fieldInfo.IsDefined(typeof(DialoguePortAttribute), false)
            );
        

        
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

            // If structs were encountered, push the modified value back up the chain
            while (stack.Count > 0)
            {
                var (parent, field) = stack.Pop();
                field.SetValue(parent, current);
                current = parent;
            }
        }
        
        public static void AddTypeOptions(Node.IOptionDefinitionContext context, Type type)
        {
            if (IsBasicSupportedType(type) && !typeof(ScriptableObject).IsAssignableFrom(type))
            {
                context.AddOption("value", type)
                    .WithDisplayName(FieldNameToDisplayName("value"))
                    .Build();
                return;
            }

            var fields = GetOptionFields(type);

            foreach (var (path, field) in fields)
            {
                var displayName = FieldNameToDisplayName(path);
                var defaultValue = field.GetCustomAttribute<DefaultValueAttribute>()?.Value;
                var tooltipAttribute = field.GetCustomAttribute<TooltipAttribute>();
                var tooltip = tooltipAttribute?.tooltip;

                // Each field becomes an editable option in the inspector
                AddNodeOption(context, path, field.FieldType, displayName, defaultValue, tooltip);
            }
        }
        
        public static void AddTypeOptions<T>(Node.IOptionDefinitionContext context) => 
            AddTypeOptions(context, typeof(T));
        
        
        
        public static void AddTypePorts(Node.IPortDefinitionContext context, Type type)
        {
            var fields = GetPortFields(type);

            foreach (var (path, field) in fields)
            {
                var displayName = FieldNameToDisplayName(path);
                var defaultValue = field.GetCustomAttribute<DefaultValueAttribute>()?.Value;

                AddInputPort(context, path, field.FieldType, displayName, defaultValue: defaultValue);
            }
        }
        
        public static void AddTypePorts<T>(Node.IPortDefinitionContext context) =>
            AddTypePorts(context, typeof(T));
        
        
        private static void AssignFromFieldOptions<T>(Node node, ref T value)
        {
            Type type = typeof(T);
            
            //Assign from single value option if necessary
            if (IsBasicSupportedType(type) && !typeof(ScriptableObject).IsAssignableFrom(type))
            {
                var option = node.GetNodeOptionByName("value");
                if (option == null) return;

                option.TryGetValue(out value);
                return;
            }
            
            var fields = GetOptionFields(type);
            foreach (var (path, field) in fields)
            {
                var option = node.GetNodeOptionByName(path);
                if (option == null) continue;
                
                option.TryGetValue(out object optionValue);
                
                SetNestedFieldValue(value, path.Split(FieldSeparator), optionValue);
            }
        }
        

        private static void AssignFromFieldPorts<T>(Node node, ref T value)
        {
            Type type = typeof(T);
        
            var fields = GetPortFields(type);
            foreach (var (path, field) in fields)
            {
                var port = node.GetInputPortByName(path);
                if (port == null) continue;
                
                MethodInfo getPortValueMethod = typeof(DialogueGraphUtility)
                    .GetMethod(nameof(GetPortValue), BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(field.FieldType);
                
                object[] parameters = new object[] { port };
                
                var optionValue = getPortValueMethod.Invoke(null, parameters);
                
                SetNestedFieldValue(value, path.Split(FieldSeparator), optionValue);
            }
        }

        public static void AssignFromNode<T>(Node node, ref T value)
        {
            //assign value if necessary
            Type type = typeof(T);
            if (typeof(UnityEngine.Object).IsAssignableFrom(type) && value == null)
            {
                Debug.LogError("Cannot assign values of a null UnityEngine.Object");
                return;
            }
            value ??= Activator.CreateInstance<T>();
            
            //assign from options and ports
            AssignFromFieldOptions(node, ref value);
            AssignFromFieldPorts(node, ref value);
        }
        
        public static void AssignFromNodeNonGeneric(Node node, Type type, ref object value)
        {
            MethodInfo genericMethod = typeof(DialogueGraphUtility)
                .GetMethod(nameof(AssignFromNode), BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(type);

            // Parameters for Invoke must be in object[]
            object[] parameters = new object[] { node, value };

            // Call generic method
            genericMethod.Invoke(null, parameters);

            // Because 'value' is a ref parameter, reflection updates the array element
            value = parameters[1];
        }

    }
}