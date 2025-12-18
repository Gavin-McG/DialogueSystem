using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Non-choice dialogue.
    /// </summary>
    /// <typeparam name="TBaseParams">Type of <see cref="BaseParams"/> to be used by the node</typeparam>
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    public class SetVariableNode : Node, IDialogueNode
    {
        private SetVariableObject _asset;
        private INodeOption _variableNameOption;
        private INodeOption _variableOption;
        private IPort _nextPort;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _variableNameOption = context.AddOption<string>("name").Build();
            _variableOption = context.AddOption<Variable>("variable").Build();
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context);
        }
  
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<SetVariableObject>();
            _asset.name = "SetVariable";
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Assign values
            _variableNameOption.TryGetValue(out _asset.variableName);
            _variableOption.TryGetValue(out _asset.variable);
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }

}
