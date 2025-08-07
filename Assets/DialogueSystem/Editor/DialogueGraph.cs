using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{

    [Graph(AssetExtension)]
    [Serializable]
    public class DialogueGraph : Graph
    {
        public const string AssetExtension = "dialogue";

        [MenuItem("Assets/Create/Dialogue System/Dialogue Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
        }
        
        public override void OnGraphChanged(GraphLogger infos)
        {
            CheckGraphErrors(infos);
        }

        private void CheckGraphErrors(GraphLogger infos)
        {
            MultipleBeginCheck(infos);
            MultipleOutputCheck(infos);
            VerifyContextTypes(infos);
        }

        private bool MultipleBeginCheck(GraphLogger infos)
        {
            var passedCheck = true;
            
            var beginDialogueNodes = GetNodes().OfType<BeginDialogueNode>().ToList();
            switch (beginDialogueNodes.Count)
            {
                case 0:
                    infos.LogError("Add a BeginDialogueNode in your Dialogue graph.");
                    passedCheck = false;
                    break;
                case > 1:
                    foreach (var beginDialogueNode in beginDialogueNodes.Skip(1))
                    {
                        infos.LogWarning($"DialogueGraph only supports one {nameof(BeginDialogueNode)} by graph. " +
                                         "Only the first created one will be used.", beginDialogueNode);
                    }
                    passedCheck = false;
                    break;
            }
            
            return passedCheck;
        }

        private bool MultipleOutputCheck(GraphLogger infos)
        {
            var passedCheck = true;
            
            var nodes = GetNodes().ToList();
            foreach (var node in nodes)
            {
                //Get Next port if it exists
                var nextPort = DialogueGraphUtility.GetNextPortOrNull(node);
                if (nextPort == null) continue;
                
                List<IPort> connectedPorts = new();
                nextPort.GetConnectedPorts(connectedPorts);

                var tracePorts = connectedPorts
                    .Select(port => port.GetNode())
                    .OfType<IDialogueTraceNode>().ToList();

                if (tracePorts.Count <= 1) continue;
                
                infos.LogError($"A Next Dialogue Port cannot exceed more than 1 trace Connection. ", node);
                passedCheck = false;
            }

            return passedCheck;
        }

        private bool VerifyContextTypes(GraphLogger infos)
        {
            var passedCheck = true;

            var choiceNodes = GetNodes().OfType<ChoiceDialogueNode>().ToList();
            foreach (var node in choiceNodes)
            {
                foreach (var block in node.blockNodes)
                {
                    if (block is ChoiceOptionNode) continue;
                    
                    infos.LogError($"Choice BlockNode '{block}' is not a {nameof(ChoiceOptionNode)}.", block);
                    passedCheck = false;
                }
            }
            
            var redirectNodes = GetNodes().OfType<SequentialRedirectNode>().ToList();
            foreach (var node in redirectNodes)
            {
                foreach (var block in node.blockNodes)
                {
                    if (block is ConditionalNode) continue;
                    
                    infos.LogError($"Conditional BlockNode '{block}' is not a {nameof(ConditionalNode)}.", block);
                    passedCheck = false;
                }
            }
            
            return passedCheck;
        }
    }

}
