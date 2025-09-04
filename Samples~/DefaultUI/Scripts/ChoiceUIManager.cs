using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using WolverineSoft.DialogueSystem.Default;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    using MyParams = DialogueParams<DefaultBaseParams, DefaultChoiceParams, DefaultOptionParams>;
    
    public class ChoiceUIManager : MonoBehaviour
    {
        [SerializeField] GameObject choiceUI;
        [SerializeField] List<ChoiceUI> choices = new List<ChoiceUI>();
        [SerializeField] ContinueUI continueUI;
        
        private readonly Dictionary<UnityAction<int>, List<UnityAction>> storedChoiceListeners = new();

        
        public void SetContinueButton(MyParams dialogueParams)
        {
            DisableChoices();
            continueUI.Enable();
        }

        
        public void SetChoiceButtons(List<DefaultOptionParams> options)
        {
            choiceUI.SetActive(true);
            for (int i=0; i<choices.Count; i++)
            {
                var choice = choices[i];
                var optionParams = options.ElementAtOrDefault(i);
                
                if (optionParams == null) choice.Disable();
                else choice.SetText(optionParams.text);
            }

            if (options.Count > choices.Count)
            {
                Debug.LogWarning("Could not display all Dialogue Choice options - Too few ChoiceUI");
            }
            
            continueUI.Disable();
        }

        
        private void DisableChoices()
        {
            choiceUI.SetActive(false);
            foreach (var choice in choices)
            {
                choice.Disable();
            }
        }

        
        public void AddChoiceListener(UnityAction<int> call)
        {
            for (int i = 0; i < choices.Count; i++)
            {
                var index = i;
                var choice = choices[i];

                UnityAction action = () => call(index);

                if (!storedChoiceListeners.TryGetValue(call, out var list))
                {
                    list = new List<UnityAction>();
                    storedChoiceListeners[call] = list;
                }

                list.Add(action);
                choice.AddListener(action);
            }
        }

        
        public void RemoveChoiceListener(UnityAction<int> call)
        {
            if (!storedChoiceListeners.TryGetValue(call, out var list))
                return;
            
            for (int i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];
                choice.RemoveListener(list[i]);
            }
            
            storedChoiceListeners.Remove(call);
        }

        
        public void RemoveAllChoiceListeners()
        {
            foreach (var choice in choices)
            {
                choice.RemoveAllListeners();
            }
            storedChoiceListeners.Clear();
        }

        
        public void AddContinueListener(UnityAction call)
        {
            continueUI.AddListener(call);
        }
        

        public void RemoveContinueListener(UnityAction call)
        {
            continueUI.RemoveListener(call);
        }
        

        public void RemoveAllContinueListeners()
        {
            continueUI.RemoveAllListeners();
        }
    }
}