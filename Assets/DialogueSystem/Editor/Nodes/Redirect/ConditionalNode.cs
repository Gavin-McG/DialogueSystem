using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class for conditional BlockNodes to be assigned onto Redirect nodes.
    /// </summary>
    /// <typeparam name="T">Type of conditional which the node will represent</typeparam>
    public abstract class ConditionalNode<T> : BlockNode, IDialogueTraceNode
    where T : ConditionalOption
    {
        private const string WeightOptionName = "Weight";
        
        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<T>(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);
        }

        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            if (contextNode==null || contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
            {
                DialogueGraphUtility.AddNodeOption(context, 
                    WeightOptionName, typeof(float), "Weight", 0.5f,
                    tooltip: "Percent probablility for this option to be chosen on evaluation");
            }
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<T>();
            option.name = "Conditional Option";
            
            DialogueGraphUtility.AssignFromFieldOptions(this, ref option);
        
            return option;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<T>(this, dialogueDict);
            var nextTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            option.nextDialogue = nextTrace;
            
            DialogueGraphUtility.AssignDialogueData(this, option.data, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option);
        }
    }

}
