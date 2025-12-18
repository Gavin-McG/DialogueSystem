namespace WolverineSoft.DialogueSystem
{
    public interface IVariableContext
    {
        public bool IsReadOnly { get; }
        
        public bool TryGetVariable(string name, out Variable variable);
        public void SetVariable(string name, Variable variable);
        
        //Setters
        public void SetString(string name, string value);
        public void SetFloat(string name, float value);
        public void SetInt(string name, int value);
        public void SetBool(string name, bool value);
        
        //Getters
        public string GetString(string name);
        public float GetFloat(string name);
        public int GetInt(string name);
        public bool GetBool(string name);
    }
}