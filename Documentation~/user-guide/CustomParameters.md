# Custom Manager

The Dialogue System is designed to be as flexible as possible to support a wide range of use-cases. This is primary achieved through the use of custom parameters. There are several different types of parameters associated with different aspects of dialogue. 

A given node can only hold one of each type of parameter which it supports. If multiple of a given type are defined then you will be given the option to select which a node should hold. The nodes which support custom parameters are as follows:

- `StartNode` - Dialogue Parameters
- `TextNode` - Text Parameters
- `ChoiceNode` - Text Parameters, Choice Parameters
- `OptionNode` - OptionType, Response Parameters (On Choice Only)

> All Custom parameter classes should be defined with the `[Serializeable]` Attribute. Additionally, if you have multiple of a type of parameter defined, you can use the `[TypeOption]` Attribute to define its name and order within the dropdown. 

## Dialogue Parameters

Dialogue Parameters are defined by creating a class derived from `DialogueParameters`. Dialogue parameters are parameters that are associated with the entire dialogue interaction as a whole. Havin at least one child class of `DialogueParameters` will make a 'Dialogue' tab available on all `StartNode`s

Dialogue parameters might often include fields about interaction types, UI details, background music, or any other information your dialogue UI/frontend can use that would be redundant to specify on many different nodes.

```
[Serializable, TypeOption("Sample")]
public class MyDialogueParameters : DialogueParameters
{
    [SerializeField] public AudioClip bgm;
}
```

## Text Parameters

Text Parameters are defined by creating a class derived from `TextParameters`. Text parameters are parameters that are associated with any dialogue element with text to be displayed, which includes both Regular and Choice dialogue. Having at least one child class of `TextParameters` will make a 'Text' tab available on all appropriate nodes.

Text parameters might often include fields about who is speaking, color of the text, font size, audio, text scroll speed, or any other information your dialogue UI/frontend might use in displaying your dialogue.

```
[Serializable, TypeOption("Sample")]
public class MyTextParameters : TextParameters
{
    [SerializeField] public string speakerName;
}
```

## Choice Parameters

Choice Parameters are defined by creating a class derived from `ChoiceParameters`. Choice parameters are parameters that are associated with choice dialogue. Having at least one child class of `ChoiceParameters` will make a 'Choice' tab available on all Choice Dialogue Nodes.

Choice parameters might often include fields about time limits, arrangement of responses, or any other information your dialogue UI.frontend might use in displaying choice dialogue.

```
[Serializable, TypeOption("Sample")]
public class MyChoiceParameters : ChoiceParameters
{
    [SerializeField] public bool isTimed;
    [SerializeField, Min(0)] public float timeLimit;
}
```

## Response Parameters

Response parameters are defined by creating a class derived from `ResponseParameters`. Response parameters are parameters that are associated which the response options of a `ChoiceNode`. Having at lease one child class of `ResponseParameters` will make a 'Response' tab available on all `OptionNodes` that are attached to a `ChoiceNode`.

Response parameters might often include fields about requirements for selecting a response, indicators about the effects of responses, text color, font size, hover audio, or any other information your dialogue UI/frontend might use in displaying your choice responses.

```
[Serializable, TypeOption("Sample")]
public class MyResponseParameters : ResponseParameters
{
    [SerializeField] public Sprite indicator;
}
```

## Option Type

While they function differently from the other parameter variants, option types are defined and are selected within the graph in much the same way. Option types are defined by creating a class derived from `OptionType`. Option types serve the purpose of defining a condition associated with a `OptionNode`. The exact purpose of ths condition varies by what node the `OptionNode` is attached to.

1. `ChoiceNode` - The OptionType controls whether the response will be available. Unavailable responses are not returned and the frontend is not made aware of them. If you'd like to have a response visible but unable to be selected, this should be achieved using Response parameters and a custom frontend impkenentation.
2. `SequentialRedirectNode` - The `OptionNode`s attached to this node are each evaluated based on their `OptionType`. The `DialogueManager` will traverse through the first node whose condition passes, or through the 'Default' port if no condition passes.
3. `RandomRedirectNode` - Functions very similarly to `SequentialRedirectNode`, but all OptionNodes always have their `OptionType` evaluated. Of all passing conditions, a weighted random option will be selected for the `DialogueManager`. Again, if no conditions pass the output of the 'Default' port will be used instead.

Unlike the other parameter Types, the `OptionType` class does require a function implementation. The `OptionType.EvaluateCondition` is the method used by the aformentioned nodes to determine if the OptionNode's condition passed, in which case this method should return true. The `AdvanceContext` is the same one provided to the call to DialogueManager.AdvanceDialogue which triggered the traversal. The `IVariableContext` provides an interface to access the variables currently held by the `DialogueManager`.

Additionally, if the `OptionType` you are defining uses fields that represent the name of a variable, you should configure your `OptionType` so that a warning can be shown if the variable name isn't defined within the graph. This can be done by overwriting `the CheckVariables` property, which expects a `IEnumerable<string>`

```
[TypeOption("Variable Equals")]
public class VariableEqualsOption : OptionType
{
    [Tooltip("Name of the variable to be compared")]
    [SerializeField] private string variableName;
    
    [Tooltip("value to compare the variable with")]
    [SerializeField] private Variable value;
    
    public override bool EvaluateCondition(AdvanceContext advanceContext, IVariableContext variables)
    {
        if (variables.TryGetVariable(variableName, out var variable))
            return variable == value;
        return false;
    }
    
    public override IEnumerable<string> CheckVariables => new List<string> { variableName };
}
```

There are several OptionTypes which are provided by default, as they are extremely commonly used or needed:

1. Basic Option - Always returns true, usually used on ChoiceNode responses that should always appear.
2. Variable Equals - Returns true if the variable specified by name matches a specified value.
3. Random Chance - Returns true with specified probability