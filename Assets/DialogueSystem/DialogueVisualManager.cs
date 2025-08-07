using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueVisualManager : MonoBehaviour
{
    [Serializable]
    public class OptionButton
    {
        [SerializeField] public Button button;
        [SerializeField] public TextMeshProUGUI text;
    }
    
    [SerializeField] private DialogueManager dialogueManager;
    [Header("UI References")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private GameObject continueUI;
    [SerializeField] private GameObject choicesUI;
    [SerializeField] private List<OptionButton> optionButtons;

    private bool dialogueEnabled = false;
    private Coroutine timeLimitCoroutine;
    private float timeStarted;

    private void OnEnable()
    {
        dialogueManager.displayDialogue.AddListener(DisplayDialogue);
        dialogueManager.endDialogue.AddListener(HideDialogue);
    }
    
    private void OnDisable()
    {
        dialogueManager.displayDialogue.RemoveListener(DisplayDialogue);
        dialogueManager.endDialogue.RemoveListener(HideDialogue);
    }

    private void DisplayDialogue(DialogueDetails dialogueDetails)
    {
        dialogueEnabled = true;
        dialogueUI.SetActive(true);
        
        mainText.text = dialogueDetails.text;
        timeStarted = Time.time;

        if (dialogueDetails.isChoice)
        {
            continueUI.SetActive(false);
            choicesUI.SetActive(true);
            
            var choiceCount = Mathf.Min(dialogueDetails.choicePrompts.Count, optionButtons.Count);
            for (var i = 0; i < choiceCount; i++)
            {
                optionButtons[i].button.gameObject.SetActive(true);
                optionButtons[i].text.text = dialogueDetails.choicePrompts[i];
            }

            for (var i = choiceCount; i < optionButtons.Count; i++)
            {
                optionButtons[i].button.gameObject.SetActive(false);
            }
        }
        else
        {
            continueUI.SetActive(true);
            choicesUI.SetActive(false);
        }
        
        EndTimeLimit();
        if (dialogueDetails.hasTimeLimit)
        {
            timeLimitCoroutine = StartCoroutine(TimeLimitRoutine(dialogueDetails.timeLimitDuration));
        }
    }

    private void HideDialogue()
    {
        dialogueEnabled = false;
        dialogueUI.SetActive(false);
    }

    public void SelectChoice(int index)
    {
        if (!dialogueEnabled) return;
        
        EndTimeLimit();

        dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
        { 
            choice = index,
            inputDelay = Time.time - timeStarted,
            timedOut = false,
        });
    }

    private void UpdateTimeLimit(float t)
    {
        Debug.Log(t.ToString() + " Completed");
    }

    IEnumerator TimeLimitRoutine(float timeLimitDuration)
    {
        float time = 0;
        while (time < timeLimitDuration)
        {
            float t = time / timeLimitDuration;
            
            UpdateTimeLimit(t);
            
            time += Time.deltaTime;
            yield return null;
        }

        dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
        {
            choice = 0,
            inputDelay = timeLimitDuration,
            timedOut = true,
        });
    }

    private void EndTimeLimit()
    {
        if (timeLimitCoroutine != null)
        {
            StopCoroutine(timeLimitCoroutine);
            timeLimitCoroutine = null;
        }
    }
}
