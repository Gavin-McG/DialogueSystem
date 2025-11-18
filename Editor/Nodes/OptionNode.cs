using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// class for options attached to choice dialogue nodes or redirects.
    /// Handles logic for displaying node options from redirect weights,
    /// option parameters, and option fields.
    /// </summary>
    [Serializable, UseWithContext(typeof(OptionContext))]
    public class OptionNode : InitializeNode, IDialogueTraceNode, IInputDataNode<Option>
    {
        private INodeOption _dataOption;
        private INodeOption _weightOption;
        private IPort _nextPort;
        private readonly List<IPort> _valuePorts = new();
        private Option _asset;
        
        
        // ───────────────────────────────────────────────
        // Node Definition
        // ───────────────────────────────────────────────
        
        protected sealed override void OnDefineInitializedOptions(IOptionDefinitionContext context)
        {
            // Conditional options depending on node type
            if (contextNode is OptionContext optionContext)
                DefineWeightOptions(optionContext, context);

            // Options from the Option's fields
            // DialogueGraphUtility.AddTypeOptions<T>(context);
        }

        protected sealed override void OnDefineInitializedPorts(IPortDefinitionContext context)
        {
            _nextPort = DialogueGraphUtility.AddNextPort(context);
            
            // Define ports for optionParams
            if (contextNode is not RedirectNode)
                DefineChoicePorts(context);

            // Ports from the Option's fields
            // DialogueGraphUtility.AddTypePorts<T>(context);

            // Create input ports for each {bracket} in the text
            // if (_textOption?.TryGetValue(out string text) ?? false)
            // {
            //     int index = 0;
            //     foreach (var placeholder in TextData.ExtractBracketContents(text))
            //     {
            //         _valuePorts.Add(
            //             context.AddInputPort<DSValue>($"value {index++}")
            //                 .WithDisplayName(placeholder)
            //                 .Build()
            //         );
            //     }
            // }
        }
        
        
        // ───────────────────────────────────────────────
        // Options & Ports
        // ───────────────────────────────────────────────
        
        private void DefineWeightOptions(OptionContext optionContext, IOptionDefinitionContext context)
        {
            // Redirect nodes may require a weight option
            if (optionContext.UsesWeight)
            {
                _weightOption = DialogueGraphUtility.AddNodeOption(context, "Weight", 
                    typeof(float), defaultValue: 0.5f,
                    tooltip: "Percent probability for this option to be chosen during evaluation"
                );
            }
            
            if (contextNode is not RedirectNode)
                DefineChoiceOptions(context);
        }

        private void DefineChoiceOptions(IOptionDefinitionContext context)
        {
            _dataOption = context.AddOption<TabGroup<TextData, OptionData, ResponseData>>("Data")
                .WithDisplayName("")
                .Build();

            // if (TryGetOptionParamType(out Type optionParamsType))
            // {
            //     DialogueGraphUtility.AddTypeOptions(context, optionParamsType);
            // }
        }

        private void DefineChoicePorts(IPortDefinitionContext context)
        {
            // if (TryGetOptionParamType(out Type optionParamsType))
            // {
            //     DialogueGraphUtility.AddTypePorts(context, optionParamsType);
            // }
        }
        
        
        // ───────────────────────────────────────────────
        // Option Param Type Utilities
        // ───────────────────────────────────────────────
        
        /// <summary>
        /// Tries to retrieve the <c>OptionParams</c> type
        /// from the parent <see cref="ChoiceNode{TChoice,TOption,TOptionParams}"/>.
        /// </summary>
        // private bool TryGetOptionParamType(out Type optionParamType)
        // {
        //     Type current = contextNode.GetType();
        //
        //     while (current != null && current != typeof(object))
        //     {
        //         if (current.IsGenericType &&
        //             current.GetGenericTypeDefinition() == typeof(ChoiceNode<,,>))
        //         {
        //             optionParamType = current.GetGenericArguments()[2]; // TOptionParams
        //             return true;
        //         }
        //
        //         current = current.BaseType;
        //     }
        //
        //     optionParamType = null!;
        //     return false;
        // }
        
        
        // ───────────────────────────────────────────────
        // Assignment Helpers
        // ───────────────────────────────────────────────

        private void AssignChoiceFields()
        {
            // if (TryGetOptionParamType(out Type optionParamsType))
            // {
            //     object optionParams = null;
            //     DialogueGraphUtility.AssignFromNodeNonGeneric(this, optionParamsType, ref optionParams);
            //     if (_option == null) Debug.Log(2);
            //     _option.optionParams = (OptionData)optionParams;
            // }
            //
            // // Assign text parameter
            // _textOption.TryGetValue(out _option.optionParams.text);
        }
        
        
        // ───────────────────────────────────────────────
        // Dialogue Object Creation
        // ───────────────────────────────────────────────
        
        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<Option>();
            _asset.name = "OptionSO";
            return _asset;
        }

        public void AssignObjectReferences()
        {
            _dataOption.TryGetValue(out TabGroup<TextData, OptionData, ResponseData> data);
            _asset.textData = data.first;
            _asset.optionData = data.second;
            _asset.responseData = data.third;
            
            // Set next dialogue
            //_option.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            // Assign from option fields
            //DialogueGraphUtility.AssignFromNode(this, ref _option);

            // Assign choice/redirect specific parameters
            if (contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
                _weightOption.TryGetValue(out _asset.weight);
            else if (contextNode is not RedirectNode)
                AssignChoiceFields();

            // Assign Events and Values
            DialogueGraphUtility.AssignDialogueData(_asset.data, _nextPort);

            // Assign values for each {bracket} in the text
            // foreach (var valuePort in _valuePorts)
            // {
            //     _option.optionParams.values.Add(
            //         DialogueGraphUtility.GetPortValueOrDefault<DSValue>(this, valuePort.name)
            //     );
            // }
        }

        public Option GetInputData()
        {
            return _asset;
        }
    }
}