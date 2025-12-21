using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem.Example
{
    public class TimeLimitUI : MonoBehaviour
    {
        [SerializeField] RectTransform barBackground;
        [SerializeField] RectTransform bar;

        [HideInInspector] public UnityEvent TimeLimitExpired;
        
        private Coroutine timeLimitCoroutine;

        private void OnEnable()
        {
            SetBarPercentage(0);
        }

        public void StartTimer(float timeLimit)
        {
            StopTimer();
            timeLimitCoroutine = StartCoroutine(TimeLimitRoutine(timeLimit));
        }

        public void StopTimer()
        {
            if (timeLimitCoroutine != null)
            {
                StopCoroutine(timeLimitCoroutine);
                timeLimitCoroutine = null;
            }
            SetBarPercentage(0);
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

            TimeLimitExpired.Invoke();
            StopTimer();
        }
    }
}