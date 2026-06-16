# Dialogue Info

`DialogueInfo` contains all the information pertaining to a specific dialogue output. It is returned by the `DialogueManager` when advancing or refreshing dialogue via `DialogueManager.AdvanceDialogue` or `DialogueManager.RefreshDialogue`. If the returned DialogueInfo is null then the dialogue has ended.

There are several public fields of the dialoguInfo. The first is the `dialogueType`, which is an enumerator representing what type of dialogue output this is:

- `DialogueType.Basic` - Regular text dialogue
- `DialogueType.Choice` - Choice dialogue containing response info
- `DialogueType.Stall` - Encountered Event with stall, containing no other info

## Text

The `DialogueInfo.text` field contains the string output of the dialogue to be displayed as long as the type is either `DialogueType.Basic` or `DialogueType.Choice`. Text inside of brackets `{}` will be replaced with the current value of a variable. For instance, if the `Variable` 'myVar' were set to 5, "{myVar}" would be replaced by "5".

## ResponseInfo

The `DialogueInfo.responses` is a List of `ResponseInfo` instances. These responses contain the info for each individual response. The field will be null unless the type is `DialogueType.Choice`.  `ResponseInfo` has a `ResponseInfo.text` field which contains the string for that response. Brackets are also used to embed variable values in the same way as the `DialogueInfo.text`. 

The number of responses available is equal to the length of the list. When advancing the dialogue from a `DialogueType.Choice` output, the next `AdvanceContext` should have its `AdvanceContext.choice` field set to the response selected by the user. The choice should be the response's index in the `DialogueInfo.responses` list. If `AdvanceContext.choice` is set to -1, then the 'Default' output of the ChoiceNode will be used instead of any of the options.

## Paramaters

If the `DialogueInfo` has any parameters assigned they can be accessed using the following methods:

```
// Methods for Text parameters
Type DialogueInfo.GetBaseParamsType()
T DialogueInfo.GetTextParams<T>() where T : TextParameters

// Methods for Choice parameters
Type GetChoiceParamsType()
T GetChoiceParams<T>() where T : ChoiceParameters
```

These methods will return null if there is not a corresponding parameter type assigned. Getting the type of the parameter is useful for checking whether the parameter type is present and, if so, what type it is. This can help make the code a bit more clean compared to retrieving the parameters as the base class and attempting to cast to supported derived types.

`ResponseInfo` also has corresponding methods for getting each response's response parameters:

```
// Methods for Response parameters
Type ResponseInfo.GetResponseParamsType()
T ResponseInfo.GetResponseParams<T>() where T : ResponseParameters
```