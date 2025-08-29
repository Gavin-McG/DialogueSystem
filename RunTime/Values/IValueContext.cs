namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Interface for classes which should represent a local scope of values
    /// </summary>
    public interface IValueContext
    {
        public string ContextName { get; }
    }
}