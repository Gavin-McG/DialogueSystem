using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    public class ChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameObject choiceUI;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private Button choiceButton;
        [SerializeField] private InputActionReference choiceAction;

        public void Disable()
        {
            choiceUI.SetActive(false);
            choiceText.text = "";
            
            if (choiceAction != null)
                choiceAction.action.started -= TriggerButton;
        }

        public void SetText(string text)
        {
            choiceUI.SetActive(true);
            choiceText.text = text;
            
            if (choiceAction != null)
                choiceAction.action.started += TriggerButton;
        }

        public void AddListener(UnityAction call)
        {
            choiceButton.onClick.AddListener(call);
        }
        
        public void RemoveListener(UnityAction call)
        {
            choiceButton.onClick.RemoveListener(call);
        }

        public void RemoveAllListeners()
        {
            choiceButton.onClick.RemoveAllListeners();
        }
        
        private void TriggerButton(InputAction.CallbackContext context)
        {
            choiceButton.onClick.Invoke();
        }
    }
}