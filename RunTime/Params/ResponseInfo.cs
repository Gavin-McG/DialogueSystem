using System;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    public class ResponseInfo
    {
        [SerializeField] public string text;
        [SerializeField] private readonly ResponseParameters _responseParams;

        internal ResponseInfo(string text, ResponseParameters responseParams)
        {
            this.text = text;
            this._responseParams = responseParams;
        }

        /// <summary>
        /// Returns the Choice Parameters of the dialogue as type T
        /// </summary>
        public T GetResponseParams<T>() where T : ResponseParameters
        {
            if (_responseParams is T tResponseParams) return tResponseParams;
            return null;
        }

        public Type GetResponseParamsType() => _responseParams.GetType();
    }
}
