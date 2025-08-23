using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    public abstract class OptionNode<T> : InitializeNode, IDialogueTraceNode
    where T : Option
    {
        private const string WeightOptionName = "Weight";
        
        protected override void DefineFullOptions(IOptionDefinitionContext context)
        {
            if (contextNode is RedirectNode redirectNode)
                DefineConditionalOptions(redirectNode, context);
            else
                DefineChoiceOptions(context);
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            if (contextNode is not RedirectNode)
                DefineChoicePorts(context);
            
            DialogueGraphUtility.DefineFieldPorts<T>(context);
        }

        private static void DefineConditionalOptions(RedirectNode redirect, IOptionDefinitionContext context)
        {
            if (redirect.UsesWeight)
            {
                DialogueGraphUtility.AddNodeOption(context,
                    WeightOptionName, typeof(float), "Weight", 0.5f,
                    tooltip: "Percent probablility for this option to be chosen on evaluation");
            }
        }

        private bool TryGetOptionParamType(out Type optionParamType)
        {
            Type current = contextNode?.GetType();

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
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                DialogueGraphUtility.DefineFieldOptions(context, tOptionParams);
            }
        }

        private void DefineChoicePorts(IPortDefinitionContext context)
        {
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                DialogueGraphUtility.DefineFieldPorts(context, tOptionParams);
            }
        }

        private void AssignFromChoiceOptions(ref T obj)
        {
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                obj.optionParams = (OptionParams)DialogueGraphUtility.AssignFromFieldOptions(this, tOptionParams);
            }
        }

        private void AssignFromChoicePorts(ref T obj, Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            if (TryGetOptionParamType(out Type tOptionParams))
            {
                obj.optionParams = (OptionParams)DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, obj.optionParams, tOptionParams);
            }
        }

        public ScriptableObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<T>();
            option.name = typeof(T).Name;

            if (contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
                option.weight = DialogueGraphUtility.GetOptionValueOrDefault<float>(this, WeightOptionName);
            else
                AssignFromChoiceOptions(ref option);
            
            DialogueGraphUtility.AssignFromFieldOptions(this, ref option);
        
            return option;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<T>(this, dialogueDict);
            var nextTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            option.nextDialogue = nextTrace;
            
            if (contextNode is not RedirectNode)
                AssignFromChoicePorts(ref option, dialogueDict);
            
            DialogueGraphUtility.AssignDialogueData(this, option.data, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option);
        }
    }
}