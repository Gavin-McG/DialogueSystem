using System;
using System.Collections;
using WolverineSoft.DialogueSystem.Default;
using WolverineSoft.DialogueSystem;
using UnityEngine;

public class ExampleDialogueDispatch : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueAsset asset;
    [SerializeField] private DSEvent dialogueEvent;
    
    private void OnEnable()
    {
        StartCoroutine(DispatchRoutine());
    }

    IEnumerator DispatchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        manager.BeginDialogue(asset);
    }
}
