using System;
using Unity.GraphToolkit.Editor;
using WolverineSoft.DialogueSystem.Default;
using WolverineSoft.DialogueSystem.Editor;

namespace WolverineSoft.DialogueSystem.Default.Editor
{
    [Serializable, UseWithContext(typeof(OptionContext))]
    public class RandomOptionNode : OptionNode<RandomOption> {}
}
    

