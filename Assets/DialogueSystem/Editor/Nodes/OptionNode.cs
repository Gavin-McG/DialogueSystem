using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-23</date>
    
    /// <summary>
    /// Base class for options which are attached to choice dialogue and redirects.
    /// Handles logic for displaying node options from redirect weight, option params, and option fields
    /// </summary>
    public abstract class OptionNode<T> : InitializeNode, IDialogueTraceNode
    where T : Option
    {
        private const string WeightOptionName = "Weight";
        
        //display options when properly initialized
        protected sealed override void DefineFullOptions(IOptionDefinitionContext context)
        {
            if (contextNode is RedirectNode redirectNode)
                DefineConditionalOptions(redirectNode, context);
            else
                DefineChoiceOptions(context);
            
            //define the option fields' options
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            if (contextNode is not RedirectNode)
                DefineChoicePorts(context);
            
            //define the option fields' ports
            DialogueGraphUtility.DefineFieldPorts<T>(context);
        }
        
        private static void DefineConditionalOptions(RedirectNode redirect, IOptionDefinitionContext context)
        {
            //Create option for weight for redirect nodes which require it
            if (redirect.UsesWeight)
            {
                DialogueGraphUtility.AddNodeOption(context,
                    WeightOptionName, typeof(float), "Weight", 0.5f,
                    tooltip: "Percent probablility for this option to be chosen on evaluation");
            }
        }

        /// <summary>
        /// Gets the type of Option param used by the parent Choice contextNode
        /// </summary>
        private bool TryGetOptionParamType(out Type optionParamType)
        {
            Type current = contextNode?.GetType();
            
            //step through inheritance to find ChoiceNode
            while (current != null && current != typeof(object))
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(ChoiceNode<,,>))
                {
                    //TOptionParams is third type parameter in ChoiceNode
                    optionParamType = current.GetGenericArguments()[2]; 
                    return true;
                }

                current = current.BaseType;
            }

            optionParamType = null!;
            return false;
        }

        private void DefineChoiceOptions(IOptionDefinitionContext context)
        {
            //create options for the OptionParams of the choice node
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                DialogueGraphUtility.DefineFieldOptions(context, tOptionParams);
            }
        }

        private void DefineChoicePorts(IPortDefinitionContext context)
        {
            //create ports for the OptionParams of the choice node
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                DialogueGraphUtility.DefineFieldPorts(context, tOptionParams);
            }
        }

        private void AssignFromChoiceOptions(ref T obj)
        {
            //Assign from the options of the OptionParams
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                obj.optionParams = (OptionParams)DialogueGraphUtility.AssignFromFieldOptions(this, tOptionParams);
            }
        }

        private void AssignFromChoicePorts(ref T obj, Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            //Assign from the ports of the OptionParams
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                obj.optionParams = (OptionParams)DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, obj.optionParams, tOptionParams);
            }
        }

        public ScriptableObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<T>();
            option.name = DialogueGraphUtility.FieldNameToDisplayName(typeof(T).Name);

            //assign from the respective redirect/choice options
            if (contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
                option.weight = DialogueGraphUtility.GetOptionValueOrDefault<float>(this, WeightOptionName);
            else
                AssignFromChoiceOptions(ref option);
            
            //assign from option fields
            DialogueGraphUtility.AssignFromFieldOptions(this, ref option);
        
            return option;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<T>(this, dialogueDict);
            var nextTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            option.nextDialogue = nextTrace;
            
            //redirect options don't have extra ports
            if (contextNode is not RedirectNode)
                AssignFromChoicePorts(ref option, dialogueDict);
            
            DialogueGraphUtility.AssignDialogueData(this, option.data, dialogueDict);
            
            //assign from option fields
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option);
        }
    }
}