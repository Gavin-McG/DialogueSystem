using System;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Choice Dialogue Nodes. 
    /// </summary>
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    public class ChoiceNode : ContextNode, IDialogueNode
    {
        private ChoiceObject _asset;
        private INodeOption _textOption;
        private INodeOption _paramOption;
        private IPort _nextPort;
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = context.AddOption<ChoiceTextHolder>("text").Build();
            _paramOption = context.AddOption<ValueHolder<TextParameters, ChoiceParameters>>("params").Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context, "Default");
        }
  
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<ChoiceObject>();
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.defaultDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Assign text
            _textOption.TryGetValue(out TextHolder text);
            _asset.text = text.text;
            
            //Get Parameters
            _paramOption.TryGetValue(out ValueHolder<TextParameters, ChoiceParameters> parameters);
            _asset.textParams = parameters.value1;
            _asset.choiceParams = parameters.value2;
            
            //Get Options
            _asset.options = blockNodes
                .OfType<OptionNode>()
                .Select(n => n.GetData())
                .OfType<OptionObject>()
                .ToList();
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }

}
