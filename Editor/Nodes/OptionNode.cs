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
    [UseWithContext(typeof(ChoiceNode))]
    public class OptionNode : BlockNode, IDialogueNode
    {
        private OptionObject _asset;
        private INodeOption _textOption;
        private INodeOption _paramOption;
        private IPort _nextPort;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = context.AddOption<OptionTextHolder>("text").Build();
            _paramOption = context.AddOption<ValueHolder<OptionType, ResponseParameters>>("params").Build();
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
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Assign text
            _textOption.TryGetValue(out TextHolder text);
            _asset.text = text.text;
            
            //Get Parameters
            _paramOption.TryGetValue(out ValueHolder<OptionType, ResponseParameters> parameters);
            _asset.optionType = parameters.value1;
            _asset.responseParams = parameters.value2;
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }
}