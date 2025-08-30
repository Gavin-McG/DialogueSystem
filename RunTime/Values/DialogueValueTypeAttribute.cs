using System;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Attribute that is used to make a Type assignable from the <see cref="DSValue"/> instepctor.
    /// Does not apply to the Graph ValueSetterNode inspector, as that only works with compile-time enums
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DialogueValueTypeAttribute : Attribute { }
}