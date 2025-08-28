using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    public class MainTextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private float textDelay = 0.05f;
        
        [HideInInspector] public UnityEvent completedText = new UnityEvent();
        
        public enum TextState { Scrolling, Completed }

        private string currentText;
        private Coroutine displayTextCoroutine;
        public TextState textState { get; set; } = TextState.Completed;

        public void SetText(string text)
        {
            currentText = text;
            textState = TextState.Scrolling;
            
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
            textState = TextState.Completed;
            
            if (displayTextCoroutine != null)
            {
                StopCoroutine(displayTextCoroutine);
                displayTextCoroutine = null;
            }
            
            completedText.Invoke();
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