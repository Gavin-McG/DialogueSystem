using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Button representing specific Response in a Choice Dialogue
    /// </summary>
    public class ChoiceButton : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<int> ButtonPressed = new();
        
        [SerializeField] private int index;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private Button choiceButton;
        [SerializeField] private InputActionReference choiceAction;
        
        private void OnEnable()
        {
            choiceButton.onClick.AddListener(ClickButton);
            if (choiceAction != null)
                choiceAction.action.performed += ClickButton;
        }

        private void OnDisable()
        {
            choiceButton.onClick.RemoveListener(ClickButton);
            if (choiceAction != null)
                choiceAction.action.performed -= ClickButton;
        }

        public void UpdateText(ResponseInfo response)
        {
            choiceText.text = response.text;
        }
        
        private void ClickButton()
        {
            ButtonPressed.Invoke(index);
        }
        
        private void ClickButton(InputAction.CallbackContext context) => ClickButton();
    }
}