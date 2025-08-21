using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Values;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Generic Base Class for Custom ValueSetterNodes.
    /// Used for assigning value options which aren't natively supported by Graph Toolkit / <see cref="ValueSetterNode"/> 
    /// </summary>
    /// <typeparam name="T">Type that the node will assign values to</typeparam>
    public abstract class CustomValueSetterNode<T> : Node, IDataNode<ValueEditor>
    {
        protected sealed override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<ValueSetter<T>>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var valueEntry = DialogueGraphUtility.AssignFromFieldOptions<ValueSetter<T>>(this);
            return valueEntry;
        }
    }
}