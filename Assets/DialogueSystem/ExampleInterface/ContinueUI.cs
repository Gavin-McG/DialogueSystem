using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueSystem.ExampleInterface
{
    public class ContinueUI : MonoBehaviour
    {
        [SerializeField] private GameObject continueUI;
        [SerializeField] private Button continueButton;

        public void Disable()
        {
            continueUI.SetActive(false);
        }

        public void Enable()
        {
            continueUI.SetActive(true);
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
    }
}