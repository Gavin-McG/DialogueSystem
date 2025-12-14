using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Graph Toolkit class for the Dialogue Graph interface.
    /// Handles errors/warning pertaining to the graph as a whole
    /// </summary>

    [Graph(AssetExtension)]
    [Serializable]
    public sealed class DialogueGraph : Graph
    {
        public const string AssetExtension = "dialogue";

        [MenuItem("Assets/Create/Dialogue System/Dialogue Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
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
    }

}
