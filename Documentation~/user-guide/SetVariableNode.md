## Set Variable Node

The `SetVariableNode` is another node which doesn't produce any output for the dialogue. It is used to change the value of a `Variable` at runtime. This node only affects the value associated with a `Variable` for the `DialogueManager` that is running the dialogue. The graph's assigned default value for the `Variable` will remain unchanged. The default values for variables are static and can only be changed manually within the Graph editor. 

> The variable which the node sets is defined using the name of the variable. If a name not defined within the graph is used then a warning will be present on the node.