using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    public class TimeLimitUI : MonoBehaviour
    {
        [SerializeField] GameObject timeLimitUI;
        [SerializeField] RectTransform barBackground;
        [SerializeField] RectTransform bar;

        [HideInInspector] public UnityEvent timeLimitExpired;
        
        private Coroutine timeLimitCoroutine;

        public void StartTimer(float timeLimit)
        {
            timeLimitUI.SetActive(true);
            StopTimerRoutine();
            timeLimitCoroutine = StartCoroutine(TimeLimitRoutine(timeLimit));
        }

        public void Disable()
        {
            timeLimitUI.SetActive(false);
            StopTimerRoutine();
        }

        private void StopTimerRoutine()
        {
            if (timeLimitCoroutine != null)
            {
                StopCoroutine(timeLimitCoroutine);
                timeLimitCoroutine = null;
            }
        }

        public void SetBarPercentage(float percentage)
        {
            percentage = Mathf.Clamp01(percentage);
            bar.anchorMax = new Vector2(percentage, 0.5f);
        }
        
        IEnumerator TimeLimitRoutine(float timeLimitDuration)
        {
            float time = 0;
            while (time < timeLimitDuration)
            {
                float t = time / timeLimitDuration;

                SetBarPercentage(t);

                time += Time.deltaTime;
                yield return null;
            }

            timeLimitExpired.Invoke();
            timeLimitCoroutine = null;
        }
    }
}