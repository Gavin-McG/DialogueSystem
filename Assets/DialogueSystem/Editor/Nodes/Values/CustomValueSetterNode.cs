using System;
using System.Collections.Generic;
using WolverineSoft.DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
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
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldOptions<ValueSetter<T>>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData()
        {
            var valueEntry = DialogueGraphUtility.AssignFromFieldOptions<ValueSetter<T>>(this);
            return valueEntry;
        }
    }
}