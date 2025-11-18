using UnityEngine;
using WolverineSoft.DialogueSystem;

public class Option : DialogueTrace
{
    [SerializeField] public DialogueTrace nextDialogue;
    [SerializeField] public TextData textData;
    [SerializeField] public ResponseData responseData;
    [SerializeField] public OptionData optionData;
    public float weight = 1;
    
    protected override DialogueTrace GetNextDialogue(AdvanceParams advanceParams, DialogueManager manager)
    {
        return nextDialogue;
    }
    
    public bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager) =>
        optionData.data.EvaluateCondition(advanceParams, manager);
}