---
_layout: landing
---

# Dialogue System

The Dialogue System is a flexible Unity dialogue framework for creating branching conversations, player choices, events, variables, and custom runtime data.

## What this system does

- Defines dialogue as a graph asset (`.dialogue`) using Unity's Graph Toolkit.
- Separates runtime dialogue state from UI, so developers can implement their own frontend.
- Supports linear text, choice-based dialogue, conditional redirects, event invocation, and variable-driven content.
- Enables custom parameter data for dialogue, text, choices, responses, and option conditions.

## Main concepts

### Dialogue Graph

A `DialogueGraph` is the authoring asset for a dialogue interaction. Graphs are built from nodes and connected ports, representing the flow of a conversation.

Common nodes:
- `StartNode` — begins a dialogue interaction and optionally uses a named start point.
- `TextNode` — displays narrative or spoken text.
- `ChoiceNode` — presents player responses and branches based on the selected option.
- `OptionNode` — attached to `ChoiceNode`s or redirect nodes to represent each available outcome.
- `EventNode` — invokes `DSEvent` objects and optionally stalls dialogue traversal.
- `SequentialRedirectNode` — evaluates options in order and chooses the first valid path.
- `RandomRedirectNode` — evaluates all options and chooses one using weighted randomness.
- `SetVariableNode` — updates a graph variable at runtime without producing dialogue output.

### DialogueManager

The `DialogueManager` is the runtime controller for a single active dialogue interaction. It:
- begins dialogue on a `DialogueGraph`
- advances through nodes using `AdvanceDialogue`
- refreshes the current dialogue output with `RefreshDialogue`
- ends dialogue with `EndDialogue`
- stores local variables and exposes them via a Unity-like API
- provides access to dialogue-wide `DialogueParameters`

### DialogueInfo

`DialogueInfo` is the runtime result returned by the `DialogueManager` when advancing or refreshing dialogue.

It contains:
- `dialogueType` (`Basic`, `Choice`, `Stall`)
- `text` with support for `{variable}` substitution
- a list of `responses` for choice dialogue
- parameter data for text, choice, and response nodes

### Variables

Dialogue graphs support named variables of type `Bool`, `Int`, `Float`, and `String`.

- Variables are defined in the graph blackboard.
- Values can be changed at runtime via `DialogueManager` or `SetVariableNode`.
- Variable values can be embedded in displayed text with `{VarName}`.
- Option conditions and other custom logic can read variables when evaluating paths.

### Events

Events are fired through `EventNode`s using `DSEvent` and typed `DSEvent<T>` assets.

- Default event types include `DSEvent`, `DSEventString`, `DSEventBool`, `DSEventFloat`, `DSEventInt`, and `DSEventAudioClip`.
- Custom event types can be added by subclassing `DSEvent<T>` and closing the generic.
- `EventNode` can optionally stall dialogue until the next `AdvanceDialogue` call.

## Customization and extension

The system is designed to be extended through custom serialized classes:

- `DialogueParameters` — data for the whole dialogue interaction.
- `TextParameters` — runtime text display metadata.
- `ChoiceParameters` — configuration for choice dialogs.
- `ResponseParameters` — metadata for individual choice responses.
- `OptionType` — conditional logic used by `OptionNode`s, redirects, and choice availability.

Custom parameter classes must be marked `[Serializable]` and optionally use `[TypeOption]` for labeling.

## Typical workflow

1. Create a new `DialogueGraph` asset.
2. Add a `StartNode` and define start names if needed.
3. Add `TextNode`, `ChoiceNode`, `OptionNode`, `EventNode`, `RedirectNode`, and `SetVariableNode` nodes.
4. Create graph variables in the blackboard.
5. Connect node ports to define dialogue flow.
6. Build a runtime UI that uses `DialogueManager.AdvanceDialogue` and renders `DialogueInfo`.
7. Use custom parameters, events, and option conditions to support game-specific behavior.

## Why use this system

- Author rich branching dialogue without hardcoding conversation flow.
- Keep runtime dialogue logic separate from visual UI implementation.
- Use reusable events and conditional logic to build interactive game systems.
- Add custom data and behavior that integrates cleanly with dialogue nodes.

## What to explore next

- `DialogueGraphs.md` — how to create and edit dialogue graphs
- `DialogueManager.md` — runtime API and dialogue lifecycle
- `DialogueInfo.md` — how dialogue output is represented
- `DialogueEvents.md` — creating and invoking events from dialogue
- `CustomParameters.md` — defining custom data for dialogue nodes
- `RedirectNodes.md` — conditional and random branching behavior
- `SetVariableNode.md` — modifying variables during dialogue
