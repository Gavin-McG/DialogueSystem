using System;
using System.Collections;
using DialogueSystem.Runtime;
using UnityEngine;

public class ExampleDialogueDispatch : MonoBehaviour
{
    [SerializeField] private DialogueManager manager;
    [SerializeField] private DialogueAsset asset;
    [SerializeField] private DSEvent dialogueEvent;
    [SerializeField] private DSEventString dialogueIntEvent;
    [SerializeField] private TestClass testClass;

    [Serializable]
    public class TestClass
    {
        public int value;
        public TestClass instance;
    }
    
    private void OnEnable()
    {
        StartCoroutine(DispatchRoutine());
        dialogueEvent.AddListener(() => Debug.Log("You Win!"));
        dialogueIntEvent.AddListener((string v) => Debug.Log(v));
    }

    IEnumerator DispatchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        manager.beginDialogue.Invoke(asset);
    }
}
