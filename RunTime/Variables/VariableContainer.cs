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

        public bool TryGetString(string name, out string value)
        {
            if (!TryGetVariable(name, out Variable variable) || variable.Type == VariableType.String)
            {
                value = null;
                return false;
            }
            
            value = variable.GetString();
            return true;
        }

        public bool TryGetInt(string name, out int value)
        {
            if (!TryGetVariable(name, out Variable variable) || variable.Type == VariableType.String)
            {
                value = 0;
                return false;
            }
            
            value = variable.GetInt();
            return true;
        }

        public bool TryGetFloat(string name, out float value)
        {
            if (!TryGetVariable(name, out Variable variable) || variable.Type == VariableType.String)
            {
                value = 0f;
                return false;
            }
            
            value = variable.GetFloat();
            return true;
        }

        public bool TryGetBool(string name, out bool value)
        {
            if (!TryGetVariable(name, out Variable variable) || variable.Type == VariableType.String)
            {
                value = false;
                return false;
            }
            
            value = variable.GetBool();
            return true;
        }
    }
}