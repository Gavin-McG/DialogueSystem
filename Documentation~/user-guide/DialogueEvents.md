# Dialogue Events

Invoking events from within dialogue is accomplished through the use of the `EventNode` along with `DSEvent` objects.

## Creating an Event Object

Creating a new event requires creating a new `DSEvent` object. This can be done through the create menu by navigating to `Create -> Dialogue System -> Events` and selecting the type of event that you wish to create. While custom event types can be created, there are several types of events that are included by default due to being frequently needed. Those types are:

- `DSEvent` - Non-generic type used for basic event that require no data.
- `DSEvent (string)` - Defined as `DSEventString : DSEvent<string>`
- `DSEvent (bool)` - Defined as `DSEventBool : DSEvent<bool>` 
- `DSEvent (float)` - Defined as `DSEventFloat : DSEvent<float>` 
- `DSEvent (int)` - Defined as `DSEventInt : DSEvent<int>` 
- `DSEvent (AudioClip)` - Defined as `DSEventAudioClip : DSEvent<AudioClip>` 

It is also important to note, that when storing references to a specific type of event that you use the closed generic form. For example, use `DSEventInt` rather than `DSEvent<int>`. Unity doesn't handle generic SpriptableObject fields well, and will show all events in the dropdown rather than just those of the correct type. Using the closed generic prevents this issue.

## Events inside a Graph

Using an event within a `DialogueGraph` requires creating a `EventNode`. The `EventNode` can be placed anywhere within your dialogue sequence and will Invoke the assigned event when traversed by a `DialogueManager`. The `EventNode` supports assigning both typed and untyped events. In the case that a typed event is used, the `EventNode` will provide a field to enter the value you'd like the event to be called with for that specific invocation.

### Event Stalls

The `EventNode` has an additional option called 'Stall'. If the stall option for an `EventNode` is enabled it will prompt the Dialogue manager to stop traversing the dialogue and output a `DialogueInfo` with `dialogueType=DialogueType.Stall` and no other data. The `DialogueManager` will resume traversal the next time `AdvanceDialogue` is called. This is most often used for cases where you would like to pause the dialogue and later resume in response to some event. A common example of this might be to create a `WaitForSecond` event which will cause the `AdvanceDialogue` method to be called again after a delay.

## Using an Event

The API of the `DSEvent` and `DSEvent<T>` are designed to be identical nearly identical to that of the `UnityEvent` and `UnityEvent<T>` classes respectively in order to improve familiarity. It uses the standard Pub/Sub architecture for events. `DSEvent`s that are attached to `EventNode`s will be invoked automatically, but they can also be invoked manually through code.

```
void DSEvent.Invoke()
void DSEvent.AddListener(UnityAction call)
void DSEvent.RemoveListener(UnityAction call)

void DSEvent<T>.Invoke(T value)
void DSEvent<T>.AddListener(UnityAction<T> call)
void DSEvent<T>.RemoveListener(UnityAction<T> call)
```

Notably, events exists as `ScriptableObject`s, which means they can be Invokes from multiple `DialogueManager`s without any way of distinguishing which was the source. For this purpose, an additional set of methods are defined which can add and remove listeners which only listen to events called from a specific `DialogueManager`.

```
void DSEvent.Invoke(DialogueManager manager)
void DSEvent.AddListener(DialogueManager manager, UnityAction call)
void DSEvent.RemoveListener(DialogueManager manager, UnityAction call)

void DSEvent<T>.Invoke(DialogueManager manager, T value)
void DSEvent<T>.AddListener(DialogueManager manager, UnityAction<T> call)
void DSEvent<T>.RemoveListener(DialogueManager manager, UnityAction<T> call)
```

Because these events exist as `ScriptableObjects` their data will persists between scenes. This makes is especially important to clean up their listeners properly, otherwise they may continue to grow in size. It is advised to call `RemoveAllListeners` when appropriate to reduce the risk of this occuring.

```
void DSEvent.RemoveAllListeners()
void DSEvent<T>.RemoveAllListeners()
```

## Defining Event Types

While the default event types support a large number of use cases, you may need other types to be supported for specific use cases. While the generic type `DSEvent<T>` exists, `ScriptableObject`s do not support open generics. To make use of this class you must define a child class which closes the generic. You also must use the `[CreateAssetMenu]` attribute to make it available within the Create menu.

```
[CreateAssetMenu(fileName = "DSEvent", menuName = "Dialogue System/Events/Dialogue Event (Custom)")]
class DSEventCustom : DSEvent<MyCustomClass> {}
```

Unless you'd like to implement your own functionality for an event type, no definitions are required inside the graph. The child class only needs to exists as to close the generic. Your custom DSEvent type will be automatically supported by the `EventNode` in dialogue graphs, so long as the generic type used is properly serilizeable.
