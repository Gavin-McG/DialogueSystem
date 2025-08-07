using System;
using System.Collections;
using DialogueSystem.Runtime;
using UnityEngine;

public class ExampleDialogueDispatch : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueAsset asset;
    [SerializeField] private DialogueEvent dialogueEvent;
    
    private void OnEnable()
    {
        StartCoroutine(DispatchRoutine());
        dialogueEvent.dialogueEvent.AddListener(() => Debug.Log("You Win!"));
    }

    IEnumerator DispatchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        manager.beginDialogue.Invoke(asset);
    }
}
