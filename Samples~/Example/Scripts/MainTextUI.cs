using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Manage Displayed Text of Dialogue
    /// </summary>
    public class MainTextUI : MonoBehaviour
    {
        [HideInInspector] public UnityEvent CompletedText = new UnityEvent();

        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private float textDelay = 0.05f;
        
        private string currentText;
        private Coroutine displayTextCoroutine;

        public void UpdateText(string text)
        {
            currentText = text;
            
            if (displayTextCoroutine != null)
            {
                StopCoroutine(displayTextCoroutine);
                displayTextCoroutine = null;
            }
            displayTextCoroutine = StartCoroutine(DisplayTextRoutine());
        }
        
        public void CompleteText()
        {
            mainText.text = currentText;
            
            if (displayTextCoroutine != null)
            {
                StopCoroutine(displayTextCoroutine);
                displayTextCoroutine = null;
            }
            
            CompletedText.Invoke();
        }

        IEnumerator DisplayTextRoutine()
        {
            int letterCount = 0;
            while (letterCount <= currentText.Length)
            {
                string substring = currentText.Substring(0, letterCount);
                mainText.text = substring;
                
                yield return new WaitForSeconds(textDelay);
                letterCount++;
            }
            
            CompleteText();
        }
    }
}