using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using WolverineSoft.DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Generic Base Class for Nodes which can create scriptableObject.
    /// </summary>
    /// <typeparam name="T">Type of scriptableObject to create</typeparam>
    public abstract class GenericObjectNode<T> : Node, IDialogueReferenceNode where T : ScriptableObject
    {
        private T _object;
        
        private static string ObjectName => typeof(T).Name;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<T>(context);
            
            context.AddOutputPort<T>(nameof(T))
                .WithDisplayName(ObjectName)
                .Build();
        }

        public ScriptableObject CreateDialogueObject()
        {
            _object = ScriptableObject.CreateInstance<T>();

            DialogueGraphUtility.AssignFromFieldOptions(this, ref _object);
            _object.name = ObjectName;

            return _object;
        }

        public void AssignObjectReferences()
        {
            DialogueGraphUtility.AssignFromFieldPorts(this, ref _object);
        }
    }

}