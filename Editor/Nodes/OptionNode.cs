using System;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// class for options attached to choice dialogue nodes or redirects.
    /// Handles logic for displaying node options from redirect weights,
    /// option parameters, and option fields.
    /// </summary>
    [Serializable]
    [UseWithContext(typeof(OptionContextNode))]
    public class OptionNode : BlockNode, IDialogueNode
    {
        private OptionObject _asset;
        private INodeOption _textOption;
        private INodeOption _weightOption;
        private INodeOption _paramOption;
        private IPort _nextPort;
        
        OptionContextNode OptionContext => contextNode as OptionContextNode;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            if (contextNode is null)
            {
                context.AddOption<bool>("initButton").WithDisplayName("Press This -->").Build();
                return;
            }
            
            _textOption = OptionContext.UseText ? context.AddOption<OptionTextHolder>("text").Build() : null;
            _weightOption = OptionContext.UseWeight ? context.AddOption<float>("Weight").Build() : null;

            _paramOption = OptionContext.UseResponseParameters
                ? context.AddOption<GenericValueHolder<OptionType, ResponseParameters>>("params").Build()
                : context.AddOption<GenericValueHolder<OptionType>>("params").Build();
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            _nextPort = DialogueGraphUtility.AddNextPort(context);
        }
  
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<OptionObject>();
            _asset.name = "Option";
            return _asset;
        }

        public OptionType GetOptionType()
        {
            //Get The Option type depending on the Context Node
            OptionType optionType = null;
            if (_paramOption.TryGetValue(out GenericValueHolder<OptionType, ResponseParameters> choiceParams))
                optionType = choiceParams.value1;
            else if (_paramOption.TryGetValue(out GenericValueHolder<OptionType> redirectParams))
                optionType = redirectParams.value1;
            
            return optionType;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Assign text/weight
            TextHolder text = null;
            _textOption?.TryGetValue(out text);
            _weightOption?.TryGetValue(out _asset.weight);
            _asset.text = text?.text ?? string.Empty;
            
            //Get Parameters
            _paramOption.TryGetValue(out GenericValueHolder<OptionType, ResponseParameters> parameters);
            _asset.optionType = GetOptionType();
            _asset.responseParams = parameters?.value2;
        }

        public DialogueObject GetData()
        {
            return _asset;
        }

        public void CheckErrors(GraphLogger logger, IVariableContext variables)
        {
            //Check any required Variables on OptionType for existing Default value
            if (_paramOption != null)
            {
                var optionType = GetOptionType();
                
                //Ensure all used Variables by Option type have defined default value
                var requiredVariables = optionType?.CheckVariables;
                if (requiredVariables != null)
                    foreach (var variableName in requiredVariables)
                        if (!variables.TryGetVariable(variableName, out Variable variable))
                        {
                            logger.LogWarning("Variable \"" + variableName + "\" has no default value", this);
                        }
            }
        }
    }
}