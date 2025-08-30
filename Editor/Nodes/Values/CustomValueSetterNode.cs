using Unity.GraphToolkit.Editor;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Custom ValueSetterNodes.
    /// Used for assigning value options which aren't natively supported by Graph Toolkit / <see cref="ValueSetterNode"/> 
    /// </summary>
    /// <typeparam name="T">Type that the node will assign values to</typeparam>
    public abstract class CustomValueSetterNode<T> : Node, IInputDataNode<ValueEditor>
    {
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.AddTypeOptions<ValueSetter<T>>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddDataInputPort(context);
            
            DialogueGraphUtility.AddTypePorts<ValueSetter<T>>(context);
        }

        public ValueEditor GetInputData()
        {
            ValueSetter<T> valueEntry = null;
            DialogueGraphUtility.AssignFromNode<ValueSetter<T>>(this, ref valueEntry);
            return valueEntry;
        }
    }
}