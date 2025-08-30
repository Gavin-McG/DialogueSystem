namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Interface for classes which should represent a local scope of values
    /// </summary>
    public interface IValueContext
    {
        public string ContextName { get; }
    }
}