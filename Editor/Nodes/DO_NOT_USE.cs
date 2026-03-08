using System;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// This class only exists so that the Graph Toolkit will allow Variables of "Variable" type to be created.
    /// GTK populates available types by observed ports on a tested node creation
    /// If a later update to the GTK allows for the available variable types to be explicitly defined, then this class can be removed.
    /// </summary>
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    internal class DO_NOT_USE : Node
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<Variable>("DO_NOT_USE");
        }
    }
}