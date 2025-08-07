using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
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
            var nodeDict = new Dictionary<IDialogueObjectNode, DialogueObject>();
            int nonMainAssetCount = 0;
            foreach (var objectNode in dialogueObjectNodes) 
            {
                var dialogueObject = objectNode.CreateDialogueObject();
                nodeDict.Add(objectNode, dialogueObject);
                
                if (dialogueObject is DialogueAsset asset)
                {
                    ctx.AddObjectToAsset("Main", asset);
                    ctx.SetMainObject(asset);
                }
                else 
                {
                    nonMainAssetCount++;
                    ctx.AddObjectToAsset(nonMainAssetCount.ToString(), dialogueObject);
                }
            }
            
            //assign references between nodes
            var dialogueTraceNodes = dialogueObjectNodes.OfType<IDialogueTraceNode>();
            foreach (var node in dialogueTraceNodes)
            {
                node.AssignObjectReferences(nodeDict);
            }
        }
    }
    
}
