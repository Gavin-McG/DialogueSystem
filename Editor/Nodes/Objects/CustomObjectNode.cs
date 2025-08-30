using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Nodes which can create scriptableObject.
    /// </summary>
    /// <typeparam name="T">Type of scriptableObject to create</typeparam>
    public abstract class CustomObjectNode<T> : Node, IDialogueReferenceNode, IOutputDataNode<T>
        where T : ScriptableObject
    {
        private T _object;
        
        private static string ObjectName => typeof(T).Name;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.AddTypeOptions<T>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddTypePorts<T>(context);
            
            context.AddOutputPort<T>(nameof(T))
                .WithDisplayName(ObjectName)
                .Build();
        }

        public ScriptableObject CreateDialogueObject()
        {
            _object = ScriptableObject.CreateInstance<T>();
            _object.name = ObjectName;
            return _object;
        }

        public void AssignObjectReferences()
        {
            DialogueGraphUtility.AssignFromNode(this, ref _object);
        }

        public T GetOutputData() => _object;
    }

}