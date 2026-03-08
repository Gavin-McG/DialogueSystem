using System;
using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    [Serializable]
    public class VariableContainer : IVariableContext
    {
        [SerializeField] private bool isReadOnly;
        [SerializeField] private SerializableDictionary<string, Variable> variables;

        public bool IsReadOnly => isReadOnly;

        // -----------------------------
        // Constructors
        // -----------------------------

        public VariableContainer(
            IEnumerable<KeyValuePair<string, Variable>> entries,
            bool isReadOnly = false)
        {
            this.isReadOnly = isReadOnly;
            variables = new SerializableDictionary<string, Variable>();

            if (entries == null)
                return;

            foreach (var entry in entries)
            {
                if (!variables.ContainsKey(entry.Key))
                    variables.Add(entry.Key, entry.Value);
            }
        }

        public VariableContainer(bool isReadOnly = false)
        {
            this.isReadOnly = isReadOnly;
            variables = new SerializableDictionary<string, Variable>();
        }

        // -----------------------------
        // Core queries
        // -----------------------------

        public bool TryGetVariable(string name, out Variable variable) =>
            variables.TryGetValue(name, out variable);

        public void SetVariable(string name, Variable variable)
        {
            EnsureWritable();
            variables[name] = variable;
        }

        // -----------------------------
        // Set helpers
        // -----------------------------

        private void EnsureWritable()
        {
            if (isReadOnly)
                throw new InvalidOperationException("VariableContainer is read-only.");
        }

        // -----------------------------
        // Setters
        // -----------------------------

        public void SetString(string name, string value)
        {
            EnsureWritable();
            variables[name] = new Variable(value);
        }

        public void SetInt(string name, int value)
        {
            EnsureWritable();
            variables[name] = new Variable(value);
        }

        public void SetFloat(string name, float value)
        {
            EnsureWritable();
            variables[name] = new Variable(value);
        }

        public void SetBool(string name, bool value)
        {
            EnsureWritable();
            variables[name] = new Variable(value);
        }

        // -----------------------------
        // Getters
        // -----------------------------

        public string GetString(string name)
        {
            if (!TryGetVariable(name, out Variable variable))
            {
                Debug.LogWarning($"No Variable found with name {name}");
                return null;
            }

            if (variable.Type != VariableType.String)
            {
                Debug.LogWarning($"Variable '{name}' is of type {variable.Type}, not {VariableType.String}.");
                return null;
            }
            
            return variable.GetString();
        }

        public int GetInt(string name)
        {
            if (!TryGetVariable(name, out Variable variable))
            {
                Debug.LogWarning($"No Variable found with name {name}");
                return 0;
            }

            if (variable.Type != VariableType.Int)
            {
                Debug.LogWarning($"Variable '{name}' is of type {variable.Type}, not {VariableType.Int}.");
                return 0;
            }
            
            return variable.GetInt();
        }

        public float GetFloat(string name)
        {
            if (!TryGetVariable(name, out Variable variable))
            {
                Debug.LogWarning($"No Variable found with name {name}");
                return 0f;
            }

            if (variable.Type != VariableType.Float)
            {
                Debug.LogWarning($"Variable '{name}' is of type {variable.Type}, not {VariableType.Float}.");
                return 0f;
            }
            
            return variable.GetFloat();
        }

        public bool GetBool(string name)
        {
            if (!TryGetVariable(name, out Variable variable))
            {
                Debug.LogWarning($"No Variable found with name {name}");
                return false;
            }

            if (variable.Type != VariableType.Bool)
            {
                Debug.LogWarning($"Variable '{name}' is of type {variable.Type}, not {VariableType.Bool}.");
                return false;
            }
            
            return variable.GetBool();
        }
    }
}