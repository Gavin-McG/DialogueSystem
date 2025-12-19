using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Manages Continue and Choice Buttons
    /// </summary>
    public class ChoiceUI : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> ChoicePressed = new();
        [HideInInspector] public UnityEvent ContinuePressed = new();
        
        [SerializeField] private GameObject choiceContainer;
        [SerializeField] private List<ChoiceButton> choices = new();
        [SerializeField] private ContinueButton continueButton;

        private void OnEnable()
        {
            continueButton.ButtonPressed.AddListener(Continue);
            foreach (ChoiceButton choice in choices)
            {
                choice.ButtonPressed.AddListener(SelectChoice);
            }
        }

        private void OnDisable()
        {
            continueButton.ButtonPressed.RemoveListener(Continue);
            foreach (ChoiceButton choice in choices)
            {
                choice.ButtonPressed.RemoveListener(SelectChoice);
            }
        }

        public void SetChoices(List<ResponseInfo> responses)
        {
            continueButton.gameObject.SetActive(false);

            for (int i = 0; i < choices.Count; i++)
            {
                choices[i].gameObject.SetActive(false);
                if (i < responses.Count)
                {
                    choices[i].gameObject.SetActive(true);
                    choices[i].UpdateText(responses[i]);
                }
            }
        }

        public void SetContinue()
        {
            continueButton.gameObject.SetActive(true);
            
            for (int i = 0; i < choices.Count; i++)
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        private void SelectChoice(int index) => ChoicePressed.Invoke(index);
        private void Continue() => ContinuePressed.Invoke();
    }
}