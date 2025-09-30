using System;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable, UseWithContext(typeof(OptionContext))]
    public class ValueCompOptionNode : OptionNode<ValueCompOption> {}
}