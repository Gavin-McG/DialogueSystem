using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    
    [Serializable]
    [UseWithContext(typeof(SequentialRedirectNode), typeof(RandomRedirectNode))]
    public class RandomConditionalNode : ConditionalNode
    {
        private const string ChanceOptionName = "chance";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            base.OnDefineOptions(context);
            
            context.AddNodeOption<float>(ChanceOptionName, "Chance",
                tooltip: "Percent probablility for this option to be chosen on evaluation",
                defaultValue: 0.5f);
        }

        public override DialogueObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<RandomConditionalOption>();
            option.name = "Random Conditional Option";
            option.weight = GetOptionValueOrDefault<float>(ChanceOptionName);
            option.chance = GetOptionValueOrDefault<float>(ChanceOptionName); 
            
            return option;
        }
    }
    
}
