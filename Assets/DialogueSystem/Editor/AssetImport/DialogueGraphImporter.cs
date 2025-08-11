using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [ScriptedImporter(1, DialogueGraph.AssetExtension)]
    public class DialogueGraphImporter : ScriptedImporter
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

            
            //get list of all IDialogueObject nodes (including block nodes)
            var graphNodes = graph.GetNodes().OfType<IDialogueObjectNode>().ToList();
            var dialogueObjectNodes = new List<IDialogueObjectNode>();
            foreach (var node in graphNodes)
            {
                dialogueObjectNodes.Add(node);
                if (node is not ContextNode contextNode) continue;
                
                var blocks = contextNode.blockNodes.OfType<IDialogueObjectNode>();
                dialogueObjectNodes.AddRange(blocks);
                
            }
            
            //Create objects for each node
            var nodeDict = new Dictionary<IDialogueObjectNode, ScriptableObject>();
            int nonMainAssetCount = 0;
            foreach (var objectNode in dialogueObjectNodes) 
            {
                var dialogueObject = objectNode.CreateDialogueObject();
                nodeDict.Add(objectNode, dialogueObject);
                
                if (dialogueObject is DialogueAsset asset)
                {
                    ctx.AddObjectToAsset("Main", asset, assetTexture);
                    ctx.SetMainObject(asset);
                }
                else 
                {
                    nonMainAssetCount++;
                    ctx.AddObjectToAsset(nonMainAssetCount.ToString(), dialogueObject);
                }
            }
            
            //assign references between nodes
            var dialogueReferencNodes = dialogueObjectNodes.OfType<IDialogueReferenceNode>();
            foreach (var node in dialogueReferencNodes)
            {
                node.AssignObjectReferences(nodeDict);
            }
        }
    }
    
}
