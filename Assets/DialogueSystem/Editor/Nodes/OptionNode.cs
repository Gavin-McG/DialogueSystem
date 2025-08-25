using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-23</date>
    
    /// <summary>
    /// Base class for options attached to choice dialogue nodes or redirects.
    /// Handles logic for displaying node options from redirect weights,
    /// option parameters, and option fields.
    /// </summary>
    public abstract class OptionNode<T> : InitializeNode, IDialogueTraceNode, IInputDataNode<Option>
        where T : Option
    {
        private INodeOption _textOption;
        private INodeOption _weightOption;
        private IPort _nextPort;
        private readonly List<IPort> _valuePorts = new();
        private T _option;
        
        
        // ───────────────────────────────────────────────
        // Node Definition
        // ───────────────────────────────────────────────
        
        protected sealed override void OnDefineInitializedOptions(IOptionDefinitionContext context)
        {
            // Conditional options depending on node type
            if (contextNode is RedirectNode redirectNode)
                DefineRedirectOptions(redirectNode, context);
            else
                DefineChoiceOptions(context);

            // Options from the Option's fields
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            _nextPort = DialogueGraphUtility.AddNextPort(context);
            
            // Define ports for optionParams
            if (contextNode is not RedirectNode)
                DefineChoicePorts(context);

            // Ports from the Option's fields
            DialogueGraphUtility.DefineFieldPorts<T>(context);

            // Create input ports for each {bracket} in the text
            if (_textOption?.TryGetValue(out string text) ?? false)
            {
                int index = 0;
                foreach (var placeholder in TextParams.ExtractBracketContents(text))
                {
                    _valuePorts.Add(
                        context.AddInputPort<ValueSO>($"value {index++}")
                            .WithDisplayName(placeholder)
                            .Build()
                    );
                }
            }
        }
        
        
        // ───────────────────────────────────────────────
        // Options & Ports
        // ───────────────────────────────────────────────
        
        private void DefineRedirectOptions(RedirectNode redirect, IOptionDefinitionContext context)
        {
            // Redirect nodes may require a weight option
            if (redirect.UsesWeight)
            {
                _weightOption = DialogueGraphUtility.AddNodeOption(context, "Weight", 
                    typeof(float), defaultValue: 0.5f,
                    tooltip: "Percent probability for this option to be chosen during evaluation"
                );
            }
        }

        private void DefineChoiceOptions(IOptionDefinitionContext context)
        {
            _textOption = DialogueGraphUtility.AddNodeOption(context, "Text", typeof(string));

            if (TryGetOptionParamType(out Type optionParamsType))
            {
                DialogueGraphUtility.DefineFieldOptions(context, optionParamsType);
            }
        }

        private void DefineChoicePorts(IPortDefinitionContext context)
        {
            if (TryGetOptionParamType(out Type optionParamsType))
            {
                DialogueGraphUtility.DefineFieldPorts(context, optionParamsType);
            }
        }
        
        
        // ───────────────────────────────────────────────
        // Option Param Type Utilities
        // ───────────────────────────────────────────────
        
        /// <summary>
        /// Tries to retrieve the <c>OptionParams</c> type
        /// from the parent <see cref="ChoiceNode{TChoice,TOption,TOptionParams}"/>.
        /// </summary>
        private bool TryGetOptionParamType(out Type optionParamType)
        {
            Type current = contextNode.GetType();

            while (current != null && current != typeof(object))
            {
                if (current.IsGenericType &&
                    current.GetGenericTypeDefinition() == typeof(ChoiceNode<,,>))
                {
                    optionParamType = current.GetGenericArguments()[2]; // TOptionParams
                    return true;
                }

                current = current.BaseType;
            }

            optionParamType = null!;
            return false;
        }
        
        
        // ───────────────────────────────────────────────
        // Assignment Helpers
        // ───────────────────────────────────────────────
        
        private void AssignFromChoiceOptions(ref T obj)
        {
            if (TryGetOptionParamType(out Type optionParamsType))
            {
                obj.optionParams = (OptionParams)DialogueGraphUtility.AssignFromFieldOptions(
                    this, optionParamsType
                );
            }
            
            // Assign text parameter
            _textOption.TryGetValue(out obj.optionParams.text);
        }

        private void AssignFromChoicePorts(ref T obj)
        {
            if (TryGetOptionParamType(out Type optionParamsType))
            {
                obj.optionParams = (OptionParams)DialogueGraphUtility.AssignFromFieldPorts(this, obj.optionParams, optionParamsType);
            }
        }
        
        
        // ───────────────────────────────────────────────
        // Dialogue Object Creation
        // ───────────────────────────────────────────────
        
        public ScriptableObject CreateDialogueObject()
        {
            _option = ScriptableObject.CreateInstance<T>();
            _option.name = DialogueGraphUtility.FieldNameToDisplayName(typeof(T).Name);

            // Redirect nodes may need weight assignment
            if (contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
                _weightOption.TryGetValue(out _option.weight);
            else if (contextNode is not RedirectNode)
                AssignFromChoiceOptions(ref _option);

            // Assign from option fields
            DialogueGraphUtility.AssignFromFieldOptions(this, ref _option);

            return _option;
        }

        public void AssignObjectReferences()
        {
            // Set next dialogue
            _option.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);

            if (contextNode is not RedirectNode)
                AssignFromChoicePorts(ref _option);

            DialogueGraphUtility.AssignDialogueData(_option.data, _nextPort);

            // Assign values for each {bracket} in the text
            foreach (var valuePort in _valuePorts)
            {
                _option.optionParams.values.Add(
                    DialogueGraphUtility.GetPortValueOrDefault<ValueSO>(this, valuePort.name)
                );
            }

            // Assign from option fields
            DialogueGraphUtility.AssignFromFieldPorts(this, ref _option);
        }

        public Option GetInputData()
        {
            return _option;
        }
    }
}