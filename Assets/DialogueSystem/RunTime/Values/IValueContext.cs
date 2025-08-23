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
        /// Defines (or redefines) a value in the specified scope.
        /// </summary>
        public void DefineValue(ValueSO valueSO, object value, ValueScope scope = ValueScope.Manager);

        /// <summary>
        /// Removes a value definition in the specified scope.
        /// </summary>
        public void UndefineValue(ValueSO valueSO, ValueScope scope = ValueScope.Manager);

        /// <summary>
        /// Checks if a value with the defined in any scope.
        /// </summary>
        public bool IsValueDefined(ValueSO valueSO);

        /// <summary>
        /// Retrieves the stored value as an object.
        /// </summary>
        public object GetValue(ValueSO valueSO);

        /// <summary>
        /// Retrieves the stored value, cast to the specified type.
        /// </summary>
        public T GetValue<T>(ValueSO valueSO);

        /// <summary>
        /// Retrieves the stored value by name
        /// </summary>
        public object GetValue(string valueName);
        
        /// <summary>
        /// Retrieves the stored value by name, cast to the specified type
        /// </summary>
        public T GetValue<T>(string valueName);

        /// <summary>
        /// Gets the scope in which the given value name is defined.
        /// </summary>
        public ValueScope GetValueScope(ValueSO valueSO);

        /// <summary>
        /// Clears all values defined at or below the given scope.
        /// </summary>
        public void ClearValues(ValueScope scope);
    }
}