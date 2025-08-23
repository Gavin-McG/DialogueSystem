namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for classes which can store and manage values
    /// </summary>
    public interface IValueContext
    {
        /// <summary>
        /// Defines (or redefines) a value with the given name in the specified scope.
        /// </summary>
        public void DefineValue(string valueName, object value, ValueScope scope = ValueScope.Manager);

        /// <summary>
        /// Removes a value definition for the given name in the specified scope.
        /// </summary>
        public void UndefineValue(string valueName, ValueScope scope = ValueScope.Manager);

        /// <summary>
        /// Checks if a value with the given name is currently defined in any scope.
        /// </summary>
        public bool IsValueDefined(string valueName);

        /// <summary>
        /// Retrieves the stored value by name as an object.
        /// </summary>
        public object GetValue(string valueName);

        /// <summary>
        /// Retrieves the stored value by name, cast to the specified type.
        /// </summary>
        public T GetValue<T>(string valueName);

        /// <summary>
        /// Gets the scope in which the given value name is defined.
        /// </summary>
        public ValueScope GetValueScope(string valueName);

        /// <summary>
        /// Clears all values defined at or below the given scope.
        /// </summary>
        public void ClearValues(ValueScope scope);
    }
}