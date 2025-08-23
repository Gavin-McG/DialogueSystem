using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

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

        /// <summary>
        /// Checks if a type is directly supported by the Graph Toolkit inspector 
        /// (primitives, UnityEngine types, or enums).
        /// </summary>
        private static bool IsBasicSupportedType(Type type)
        {
            return SupportedNodeOptionTypes.Contains(type)
                   || type.IsEnum;
        }

        /// <summary>
        /// Converts a field path (with separators) into a human-readable display name.
        /// Example: "stats/maxHealth" → "Stats.Max Health"
        /// </summary>
        private static string FieldNameToDisplayName(string fieldName)
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

        /// <summary>
        /// Recursively retrieves all fields of <paramref name="type"/> that should be
        /// exposed as node *options* (non-ScriptableObject fields).
        /// </summary>
        /// <remarks>
        /// - Public fields are included unless marked [NonSerialized].
        /// - Private fields are included if marked [SerializeField].
        /// - Fields marked [HideInInspector] or [HideInDialogueGraph] are skipped.
        /// - ScriptableObject fields are skipped (they are handled as ports instead).
        /// - Recursion stops if a type repeats in the ancestry chain (avoids infinite loops).
        /// </remarks>
        private static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetOptionFieldsRecursive(
            Type type,
            string parentPath = "",
            HashSet<Type> ancestorTypes = null)
        {
            ancestorTypes ??= new HashSet<Type>();

            // If type has already been visited in this recursion branch, stop (prevents infinite recursion on cyclic references)
            if (!ancestorTypes.Add(type))
                yield break;

            var fields = type.GetFields(FieldFlags);

            foreach (var field in fields)
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null ||
                                field.GetCustomAttribute<HideInDialogueGraphAttribute>() != null;
                bool isPort = typeof(ScriptableObject).IsAssignableFrom(field.FieldType);

                // Skip anything that shouldn't be an option
                if (!(isPublic || isSerializedPrivate) || isHidden || isPort)
                    continue;

                // Construct full field path (parent/child/...)
                string fullPath = string.IsNullOrEmpty(parentPath)
                    ? field.Name
                    : $"{parentPath}{FieldSeparator}{field.Name}";

                yield return (fullPath, field, type);

                // Recurse into nested complex types
                var fieldType = field.FieldType;
                if (!IsBasicSupportedType(fieldType) &&
                    !fieldType.IsPrimitive &&
                    !typeof(UnityEngine.Object).IsAssignableFrom(fieldType) &&
                    !fieldType.IsArray)
                {
                    foreach (var subField in GetOptionFieldsRecursive(fieldType, fullPath, ancestorTypes))
                        yield return subField;
                }
            }

            // Remove current type so sibling branches can explore it again
            ancestorTypes.Remove(type);
        }

        // (T overload just forwards to above method)
        private static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetOptionFieldsRecursive<T>()
        {
            return GetOptionFieldsRecursive(typeof(T));
        }

        /// <summary>
        /// Recursively retrieves all fields of <paramref name="type"/> that should be
        /// exposed as node *ports* (ScriptableObject fields).
        /// </summary>
        /// <remarks>
        /// Logic is nearly identical to <see cref="GetOptionFieldsRecursive"/>, except:
        /// - Only ScriptableObject fields are included.
        /// - Other complex fields are still traversed in case they contain nested ScriptableObjects.
        /// </remarks>
        private static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetPortFieldsRecursive(
            Type type,
            string parentPath = "",
            HashSet<Type> ancestorTypes = null)
        {
            ancestorTypes ??= new HashSet<Type>();

            if (!ancestorTypes.Add(type))
                yield break;

            var fields = type.GetFields(FieldFlags);

            foreach (var field in fields)
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null ||
                                field.GetCustomAttribute<HideInDialogueGraphAttribute>() != null;
                bool isPort = typeof(ScriptableObject).IsAssignableFrom(field.FieldType);

                string fullPath = string.IsNullOrEmpty(parentPath)
                    ? field.Name
                    : $"{parentPath}{FieldSeparator}{field.Name}";

                // Ports are only ScriptableObject fields
                if ((isPublic || isSerializedPrivate) && !isHidden && isPort)
                {
                    yield return (fullPath, field, type);
                    continue;
                }

                // Traverse into nested types to find inner ScriptableObject fields
                var fieldType = field.FieldType;
                if (!IsBasicSupportedType(fieldType) &&
                    !fieldType.IsPrimitive &&
                    !typeof(UnityEngine.Object).IsAssignableFrom(fieldType) &&
                    !fieldType.IsArray)
                {
                    foreach (var subField in GetPortFieldsRecursive(fieldType, fullPath, ancestorTypes))
                        yield return subField;
                }
            }

            ancestorTypes.Remove(type);
        }

        // (T overload just forwards to above method)
        private static IEnumerable<(string path, FieldInfo fieldInfo, Type parentType)> GetPortFieldsRecursive<T>()
        {
            return GetPortFieldsRecursive(typeof(T));
        }

        /// <summary>
        /// Traverses the object hierarchy along <paramref name="pathParts"/> 
        /// and assigns the final field with <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// - Intermediate null objects are auto-created using <see cref="Activator.CreateInstance"/>.
        /// - If the hierarchy contains structs, they are reassigned upwards 
        ///   after modification to ensure the value changes stick.
        /// </remarks>
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

        /// <summary>
        /// Define node *options* for all eligible fields of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// - Simple supported types get a single "value" option.  
        /// - Complex types are expanded into multiple options (one per field).  
        /// - Tooltip, default value, and custom attributes are forwarded if present.  
        /// </remarks>
        public static void DefineFieldOptions<T>(Node.IOptionDefinitionContext context)
        {
            var type = typeof(T);

            if (IsBasicSupportedType(type))
            {
                context.AddOption("value", type)
                    .WithDisplayName(FieldNameToDisplayName("value"))
                    .Build();
                return;
            }

            var fields = GetOptionFieldsRecursive<T>();

            foreach (var (path, field, _) in fields)
            {
                var tooltipAttribute = field.GetCustomAttribute<TooltipAttribute>();
                var tooltip = tooltipAttribute?.tooltip;
                var displayName = FieldNameToDisplayName(path);

                var defaultValue = field.GetCustomAttribute<DefaultValueAttribute>()?.Value;

                // Each field becomes an editable option in the inspector
                context.AddOption(path, field.FieldType)
                    .WithDisplayName(displayName)
                    .WithTooltip(tooltip)
                    .WithDefaultValue(defaultValue);
            }
        }

        /// <summary>
        /// Define node *ports* for all eligible ScriptableObject fields of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// - Ports are input connections, not simple editable values.  
        /// - Each ScriptableObject field creates a new port.  
        /// - If a field has a [DefaultValue] attribute, it is assigned to the port.  
        /// </remarks>
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

                    // Helpful for debugging, but could be removed in production
                    Debug.Log(defaultValueAttribute.Value?.ToString());
                }

                builder.Build();
            }
        }

        /// <summary>
        /// Creates a new instance of <typeparamref name="T"/> and assigns values
        /// from a node's defined options.
        /// </summary>
        /// <remarks>
        /// - If <typeparamref name="T"/> is a simple supported type, a single "value" option is used.  
        /// - Otherwise, a new instance of <typeparamref name="T"/> is created via reflection, 
        ///   and each option is assigned to the corresponding field (nested fields included).  
        /// - Reflection is used to call <c>INodeOption.TryGetValue&lt;T&gt;</c> dynamically.  
        /// </remarks>
        public static T AssignFromFieldOptions<T>(Node node)
        {
            var type = typeof(T);
            object obj = default(T);

            if (IsBasicSupportedType(type))
            {
                var option = node.GetNodeOptionByName("value");
                if (option == null)
                    return (T)(obj ?? default(T));

                // Dynamically call INodeOption.TryGetValue<T>
                var tryGetValue = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))
                    ?.MakeGenericMethod(type);

                object[] parameters = { null };
                if ((bool)(tryGetValue?.Invoke(option, parameters) ?? false))
                {
                    return (T)parameters[0];
                }

                // If no value, fall back to default instance
                return (T)(type.IsValueType ? Activator.CreateInstance(type) : null);
            }

            // Create new instance for complex type
            obj = Activator.CreateInstance(typeof(T));

            var fields = GetOptionFieldsRecursive<T>();
            foreach (var (path, field, _) in fields)
            {
                var option = node.GetNodeOptionByName(path);
                if (option == null)
                    continue;

                var fieldType = field.FieldType;

                // Reflection call to TryGetValue<fieldType>
                var tryGetValue = typeof(INodeOption)
                    .GetMethod(nameof(INodeOption.TryGetValue))
                    ?.MakeGenericMethod(fieldType);

                object[] parameters = { null };
                bool success = (bool)(tryGetValue?.Invoke(option, parameters) ?? false);

                var valueToAssign = success
                    ? parameters[0]
                    : fieldType.IsValueType
                        ? Activator.CreateInstance(fieldType)
                        : null;

                SetNestedFieldValue(obj, path.Split(FieldSeparator), valueToAssign);
            }

            return (T)obj;
        }

        /// <summary>
        /// Updates an existing ScriptableObject instance of type <typeparamref name="T"/>
        /// using node options.
        /// </summary>
        /// <remarks>
        /// - If <typeparamref name="T"/> is a simple supported type, only the "value" option is used.  
        /// - If no object exists yet, a new ScriptableObject is created.  
        /// - Reflection is again used to safely extract values from node options.  
        /// - Fields are set recursively using <see cref="SetNestedFieldValue"/>.  
        /// </remarks>
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
                    : fieldType.IsValueType
                        ? Activator.CreateInstance(fieldType)
                        : null;

                SetNestedFieldValue(obj, path.Split(FieldSeparator), valueToAssign);
            }
        }

        /// <summary>
        /// Updates an existing ScriptableObject instance of type <typeparamref name="T"/>
        /// using connected *ports* (instead of options).
        /// </summary>
        /// <remarks>
        /// - Ports represent ScriptableObject references, not raw values.  
        /// - Each port is looked up via <see cref="GetDialogueObjectValueOrNull"/>.  
        /// - The retrieved object is then assigned into the corresponding field.  
        /// - Fields are set recursively using <see cref="SetNestedFieldValue"/>.  
        /// </remarks>
        public static void AssignFromFieldPorts<T>(Node node,
            Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict, ref T obj)
        {
            var fields = GetPortFieldsRecursive<T>();
            foreach (var (path, field, _) in fields)
            {
                var port = node.GetInputPortByName(path);
                if (port == null)
                    continue;

                var fieldType = field.FieldType;

                // Dynamically call GetDialogueObjectValueOrNull<fieldType>
                var getDialogueObjectMethod = typeof(DialogueGraphUtility)
                    .GetMethod(nameof(GetDialogueObjectValueOrNull))?
                    .MakeGenericMethod(fieldType);

                object[] parameters = { node, path, dialogueDict };
                object valueToAssign = getDialogueObjectMethod?.Invoke(port, parameters);

                SetNestedFieldValue(obj, path.Split(FieldSeparator), valueToAssign);
            }
        }
    }
}