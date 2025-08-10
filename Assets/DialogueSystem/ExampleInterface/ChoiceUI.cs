using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueSystem.ExampleInterface
{
    public class ChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameObject choiceUI;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private Button choiceButton;

        public void Disable()
        {
            choiceUI.SetActive(false);
            choiceText.text = "";
        }

        public void SetText(string text)
        {
            choiceUI.SetActive(true);
            choiceText.text = text;
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
    }
}