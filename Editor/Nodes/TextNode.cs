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
    public class TextNode : Node, IDialogueNode
    {
        private TextObject _asset;
        private INodeOption _textOption;
        private INodeOption _paramOption;
        private IPort _nextPort;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = context.AddOption<TextTextHolder>("text").Build();
            _paramOption = context.AddOption<ValueHolder<TextParameters>>("params").Build();
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
            _asset = ScriptableObject.CreateInstance<TextObject>();
            _asset.name = "Text";
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
            _paramOption.TryGetValue(out ValueHolder<TextParameters> parameters);
            _asset.textParams = parameters.value1;
        }

        public DialogueObject GetData()
        {
            return _asset;
        }

        public void CheckErrors(GraphLogger logger, IVariableContext variables)
        {
            //Check that all used variable in text have Default value
            if (_textOption != null)
            {
                _textOption.TryGetValue(out TextHolder text);
                var variableNames = DialogueGraphUtility.GetVariableNames(text.text);
                
                foreach (var variableName in variableNames)
                    if (!variables.TryGetVariable(variableName, out Variable variable))
                    {
                        logger.LogWarning("Variable \"" + variableName + "\" has no default value", this);
                    }
            }
        }
    }

}
