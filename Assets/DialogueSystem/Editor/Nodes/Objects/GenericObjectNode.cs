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
        private static string OutputPortName => typeof(T).Name;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<T>(context);
            
            context.AddOutputPort<T>(nameof(T))
                .WithDisplayName(OutputPortName)
                .Build();
        }

        public ScriptableObject CreateDialogueObject()
        {
            var obj = ScriptableObject.CreateInstance<T>();

            DialogueGraphUtility.AssignFromFieldOptions(this, ref obj);
            obj.name = OutputPortName;

            return obj;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var obj = DialogueGraphUtility.GetObjectFromNode<T>(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref obj);
        }
    }

}