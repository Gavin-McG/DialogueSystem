
# 2.1.0

- Fixed a bug when variable-related warning were incorrectly logged
- Fixed invocation of AdvancedDialogue event
- EndedDialogue event is only invoked on EndDialogue call when dialogue is playing
- Changed Variable-getters to a more Dictionary like API
    - TryGetBool, TryGetString, TryGetInt, TryGetFloat
- Getting an assigned variaable of the wrong type will now attempt to retrieve default value from the current dialogue instead.
- Added the option to specify a new AdvanceContext when refreshing Dialogue

# 2.0.2

- Fixed EventNode UI when using a non-types DSEvent

# 2.0.1

- Added new Events invokes by DialogueManager

# 2.0.0

- Removed the old DialogueParams class and replaced with DialogueInfo
- Complete rehaul of value system, renamed to variables
    - Functions similarly to Animator system, rather than storing values by asset
    - More intuitive and removed scope confusion
- Custom property drawers for nodes.
    - Allows larger text boxes, tabs for selecting paarameter types and storing polymorphic data
    - Removed the need to define custom nodes for new parameter types. Using the Dialogue System no longer requires the definition of custom node types
- Added option to stall Dialogue events

# 1.1.0

- Updated to GTK 0.4.0-exp.2
    - Allows Block nodes to be Used with Child Context Nodes
- Create OptionContext class as base class for Choice and Redirect Nodes
- Changed all OptionNode’s UseWithContext attribute to inherit from only OptionContext
- Moved Value and Basic option to base package from Sample

# 1.0.0

Initial Release