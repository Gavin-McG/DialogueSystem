using System;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// A single peice of saved info from a <see cref="DSValue"/>.
    /// Specific to context and scope.
    /// Global scope must have the context "Global"
    /// </summary>
    [Serializable]
    internal class ValueInstance
    {
        public SerializedValueBase value;
        public string contextName;
        public DSValue.ValueScope scope;
    }
}