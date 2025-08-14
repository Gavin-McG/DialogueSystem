using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

[Serializable]
[UseWithContext(typeof(RedirectNode))]
public class ResponseTimeConditionalNode : ConditionalNode<ResponseTimeConditionalOption> {}
