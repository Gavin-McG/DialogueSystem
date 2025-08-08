using System;
using System.Collections;
using DialogueSystem.Runtime;
using UnityEngine;

public class ExampleDialogueDispatch : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueAsset asset;
    [SerializeField] private DSEvent dialogueEvent;
    [SerializeField] private DSEvent<int> dialogueIntEvent;
    
    private void OnEnable()
    {
        StartCoroutine(DispatchRoutine());
        dialogueEvent.AddListener(() => Debug.Log("You Win!"));
        dialogueIntEvent.AddListener((int v) => Debug.Log(v));
    }

    IEnumerator DispatchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        manager.beginDialogue.Invoke(asset);
    }
}
