# Redirect Nodes

Redirect nodes are nodes which do not produce any output, but can be used to affect the control flow of the graph. `OptionNode`s can be added to the redirect nodes in the same way as the `ChoiceNode`. The option that is selected, rather than using user input, is chosen by evaluaing the `OptionType` of each `OptionNode`. How these `OptionType`s are evaluated depands on the type of redirect node. `OptionNode`s that are on a redirect node will not have the text field that they have when on `ChoiceNode`s.

## Sequential Redirect Node

The `SequentialRedirectNode` evaluates the condition of each `OptionNode`'s `OptionType` in the order that they are listed. It will iterate through until the first option whose condition evaluates to true, and will select that option. If no Options are present or none evaluate to true, then the 'Default' output will be used.

## Random Redirect Node

The `RandomRedirectNode` evaluates the condition of all `OptionNode`'s `OptionType`. Any `OptionNode` that is placed on a `RandomRedirectNode` will have an additional 'weight' paraeter. All of the options which evaluate to true will then be chosen at random via a weighted random selection using the Options' weights. If no Options are present or none evaluate to true, then the 'Default' output will be used.