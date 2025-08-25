using System;
using Unity.GraphToolkit.Editor;
using WolverineSoft.DialogueSystem.Default;
using WolverineSoft.DialogueSystem.Editor;

namespace WolverineSoft.DialogueSystem.Default.Editor
{
    [Serializable, UseWithGraph(typeof(DialogueGraph))]
    public class ProfileObjectNode : CustomObjectNode<DialogueProfile> {}
    
}