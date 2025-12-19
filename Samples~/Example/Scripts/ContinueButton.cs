using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Continue Button for skipping Text Scrolling and Advancing Text Dialogue
    /// </summary>
    public class ContinueButton : MonoBehaviour
    {
        [HideInInspector] public UnityEvent ButtonPressed = new();

        [SerializeField] private Button continueButton;
        [SerializeField] private InputActionReference continueAction;
        
        private void OnEnable()
        {
            continueButton.onClick.AddListener(ClickButton);
            if (continueAction != null)
                continueAction.action.performed += ClickButton;
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(ClickButton);
            if (continueAction != null)
                continueAction.action.performed -= ClickButton;
        }
        
        
        private void ClickButton()
        {
            ButtonPressed.Invoke();
        }
        
        private void ClickButton(InputAction.CallbackContext context) => ClickButton();
    }
}