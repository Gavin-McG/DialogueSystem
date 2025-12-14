using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Custom Importer for the DialogueGraph, expected by the Graph Toolkit
    /// </summary>
    [ScriptedImporter(1, DialogueGraph.AssetExtension)]
    internal class DialogueGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
            if (graph == null)
            {
                Debug.LogError($"Failed to load dialogue graph object: {ctx.assetPath}");
                return;
            }

            //Get texture for asset
            var assetTexture = Resources.Load<Texture2D>("DialogueGraphTexture");

            
            //get list of all IDialogueNode (including block nodes)
            var graphNodes = graph.GetNodes().OfType<IDialogueNode>().ToList();
            var dialogueNodes = new List<IDialogueNode>();
            foreach (var node in graphNodes)
            {
                dialogueNodes.Add(node);
                if (node is not ContextNode contextNode) continue;
                
                var blocks = contextNode.blockNodes.OfType<IDialogueNode>();
                dialogueNodes.AddRange(blocks);
            }
            
            //Create DialogueAsset
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            ctx.AddObjectToAsset("DialogueAsset", asset, assetTexture);
            ctx.SetMainObject(asset);
            
            //Create objects for each node
            int nonMainAssetCount = 0;
            foreach (var objectNode in dialogueNodes)
            {
                var dialogueObject = objectNode.CreateDialogueObject();
                ctx.AddObjectToAsset(nonMainAssetCount.ToString(), dialogueObject);
                nonMainAssetCount++;
            }
            
            //Assign start points
            asset.startPoints = graph
                .GetNodes()
                .OfType<StartNode>()
                .Select(n => n.GetData())
                .ToList();
            
            //assign references between nodes
            foreach (var node in dialogueNodes)
            {
                node.AssignObjectReferences();
            }
        }
    }
    
}
