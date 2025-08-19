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
    [SerializeField] private DSEventInt dialogueIntEvent;
    [SerializeField] private DSEventString dialogueStringEvent;
    
    private void OnEnable()
    {
        StartCoroutine(DispatchRoutine());
        dialogueEvent.AddListener(() => Debug.Log("You Win!"));
        dialogueIntEvent.AddListener((v) =>
        {
            Debug.Log(v);
            manager.DefineValue("choice", v);
        });
        dialogueStringEvent.AddListener((v) => Debug.Log(v));
        manager.DefineValue("score", 120);
    }

    IEnumerator DispatchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        manager.BeginDialogue(asset);
    }
}
