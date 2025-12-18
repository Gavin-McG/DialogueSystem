using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    [Serializable]
    public class Variable
    {
        [SerializeField] private VariableType type;
        [SerializeField] private string _stringValue;
        [SerializeField] private float _floatValue;
        [SerializeField] private int _intValue;
        [SerializeField] private bool _boolValue;
        
        //----------------------------------
        //          Constructors
        //----------------------------------
        
        public Variable(string stringValue)
        {
            type = VariableType.String;
            _stringValue = stringValue;
        }

        public Variable(float floatValue)
        {
            type = VariableType.Float;
            _floatValue = floatValue;
        }

        public Variable(int intValue)
        {
            type = VariableType.Int;
            _intValue = intValue;
        }

        public Variable(bool boolValue)
        {
            type = VariableType.Bool;
            _boolValue = boolValue;
        }

        public Variable(Variable variable)
        {
            type = variable.type;
            _stringValue = variable._stringValue;
            _floatValue = variable._floatValue;
            _intValue = variable._intValue;
            _boolValue = variable._boolValue;
        }
        
        //----------------------------------
        //          Operators
        //----------------------------------

        public static bool operator==(Variable variable1, Variable variable2)
        {
            if (variable1 is null || variable2 is null) return false;
            if (variable1.type != variable2.type) return false;

            return variable1.type switch
            {
                VariableType.String => variable1._stringValue == variable2._stringValue,
                VariableType.Float => variable1._floatValue == variable2._floatValue,
                VariableType.Int => variable1._intValue == variable2._intValue,
                VariableType.Bool => variable1._boolValue == variable2._boolValue,
                _ => false
            };
        }
        
        public static bool operator!=(Variable variable1, Variable variable2)
        {
            if (variable1 is null || variable2 is null) return true;
            if (variable1.type != variable2.type) return true;

            return variable1.type switch
            {
                VariableType.String => variable1._stringValue != variable2._stringValue,
                VariableType.Float => variable1._floatValue != variable2._floatValue,
                VariableType.Int => variable1._intValue != variable2._intValue,
                VariableType.Bool => variable1._boolValue != variable2._boolValue,
                _ => false
            };
        }

        public bool Equals(Variable other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return this == other;
        }

        public override bool Equals(object other) =>
            other is Variable variable && Equals(variable);
        
        public override int GetHashCode()
        {
            return type switch
            {
                VariableType.String => HashCode.Combine(type, _stringValue),
                VariableType.Float  => HashCode.Combine(type, _floatValue),
                VariableType.Int    => HashCode.Combine(type, _intValue),
                VariableType.Bool   => HashCode.Combine(type, _boolValue),
                _                   => type.GetHashCode()
            };
        }

        public override string ToString() => type switch
        {
            VariableType.String => _stringValue,
            VariableType.Float => _floatValue.ToString(),
            VariableType.Int => _intValue.ToString(),
            VariableType.Bool => _boolValue.ToString(),
        };
        
        //----------------------------------
        //          Getters
        //----------------------------------
        
        public VariableType Type => type;

        public string GetString() => type == VariableType.String ? _stringValue : null;
        public float GetFloat() => type == VariableType.Float ? _floatValue : 0f;
        public int GetInt() => type == VariableType.Int ? _intValue : 0;
        public bool GetBool() => type == VariableType.Bool && _boolValue;
        
        //----------------------------------
        //          Setters
        //----------------------------------

        public void SetValue(string stringValue)
        {
            type = VariableType.String;
            _stringValue = stringValue;
        }

        public void SetValue(float floatValue)
        {
            type = VariableType.Float;
            _floatValue = floatValue;
        }

        public void SetValue(int intValue)
        {
            type = VariableType.Int;
            _intValue = intValue;
        }

        public void SetValue(bool boolValue)
        {
            type = VariableType.Bool;
            _boolValue = boolValue;
        }
    }
}
