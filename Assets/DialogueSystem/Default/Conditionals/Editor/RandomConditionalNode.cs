using System;
using DialogueSystem.Editor;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

    
[Serializable]
[UseWithContext(typeof(RedirectNode))]
public class RandomConditionalNode : ConditionalNode<RandomConditionalOption> {}
    

