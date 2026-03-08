using UnityEngine;
using UnityEngine.Serialization;
using WolverineSoft.DialogueSystem;

public class OptionObject : DialogueObject
{
    [SerializeField] public DialogueObject nextDialogue;
    [SerializeField] public string text;
    [SerializeField] public float weight = 1;
    [SerializeReference] public OptionType optionType;
    [SerializeReference] public ResponseParameters responseParams;
    
    public override DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager)
    {
        return nextDialogue;
    }
    
    public bool EvaluateCondition(AdvanceContext advanceContext, DialogueManager manager) =>
        optionType.EvaluateCondition(advanceContext, manager);
}