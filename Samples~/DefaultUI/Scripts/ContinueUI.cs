using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    public class ContinueUI : MonoBehaviour
    {
        [SerializeField] private GameObject continueUI;
        [SerializeField] private Button continueButton;
        [SerializeField] private InputActionReference continueAction;

        public void Disable()
        {
            continueUI.SetActive(false);
            
            if (continueAction != null)
                continueAction.action.started -= TriggerButton;
        }

        public void Enable()
        {
            continueUI.SetActive(true);
            
            if (continueAction != null)
                continueAction.action.started += TriggerButton;
        }
        
        
        public void AddListener(UnityAction call)
        {
            continueButton.onClick.AddListener(call);
        }
        
        public void RemoveListener(UnityAction call)
        {
            continueButton.onClick.RemoveListener(call);
        }

        public void RemoveAllListeners()
        {
            continueButton.onClick.RemoveAllListeners();
        }

        private void TriggerButton(InputAction.CallbackContext context)
        {
            continueButton.onClick.Invoke();
        }
    }
}