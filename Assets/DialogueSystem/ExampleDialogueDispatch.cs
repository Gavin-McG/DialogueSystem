using System;
using System.Collections;
using DialogueSystem.Default.Runtime;
using DialogueSystem.Runtime;
using UnityEngine;

public class ExampleDialogueDispatch : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueAsset asset;
    [SerializeField] private DSEvent dialogueEvent;
    
    private void OnEnable()
    {
        StartCoroutine(DispatchRoutine());
        dialogueEvent.AddListener(() => Debug.Log(manager.GetValue("MyValue2")));
    }

    IEnumerator DispatchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        manager.BeginDialogue(asset);
    }
}
