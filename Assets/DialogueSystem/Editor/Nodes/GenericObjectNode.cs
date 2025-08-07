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
    
    public abstract class GenericObjectNode<T> : Node, IDialogueObjectNode where T : DialogueObject
    {
        public virtual DialogueObject CreateDialogueObject()
        {
            var obj = ScriptableObject.CreateInstance<T>();

            foreach (var field in GetSerializableFields())
            {
                var option = GetNodeOptionByName(field.Name);
                if (option == null)
                    continue;

                var fieldType = field.FieldType;

                var tryGetValueMethod = typeof(INodeOption).GetMethod(nameof(INodeOption.TryGetValue))?
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

            return obj;
        }

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            foreach (var field in GetSerializableFields())
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

        private static IEnumerable<FieldInfo> GetSerializableFields()
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return typeof(T).GetFields(flags).Where(field =>
            {
                bool isPublic = field.IsPublic && field.GetCustomAttribute<NonSerializedAttribute>() == null;
                bool isSerializedPrivate = !field.IsPublic && field.GetCustomAttribute<SerializeField>() != null;
                bool isHidden = field.GetCustomAttribute<HideInInspector>() != null;

                return (isPublic || isSerializedPrivate) && !isHidden;
            });
        }
    }

}