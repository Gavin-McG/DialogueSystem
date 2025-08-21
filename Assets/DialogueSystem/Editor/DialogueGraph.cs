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

            var errorNodes = GetNodes().OfType<IErrorNode>();
            foreach (var node in errorNodes)
            {
                node.DisplayErrors(infos);
            }
        }

        private IEnumerable<INode> GetAllNodes()
        {
            //return all nodes / contextNodes
            var nodes = GetNodes();
            foreach (var node in nodes)
            {
                yield return node;
            }
            
            //return all blockNodes
            var contextNodes = GetNodes().OfType<ContextNode>();
            foreach (var contextNode in contextNodes)
            {
                var blockNodes = contextNode.blockNodes;
                foreach (var blockNode in blockNodes)
                {
                    yield return blockNode;
                }
            }
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
    }

}
