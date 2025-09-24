# **Dialogue System Documentation**

This documentation describes how to install, set up, and extend the Dialogue System package. It is designed to guide developers from first-time setup through advanced customization. It is designed with the idea that implementation is divided into two-parts: the backend/editor logic and the frontend/UI display. This system mostly focuses on the backend, given the wide variety of potential requirements for the frontend. It is likely each project will require its own implementation of frontend to be built off this backend. 

This system is built using Unity’s Graph Toolkit. For any information about defining variables for using the Graph interface view the [official documentation](https://docs.unity3d.com/Packages/com.unity.graphtoolkit@0.3/manual/introduction.html). Currently uses Graph Toolkit version 0.3.0-exp.1.

## **Installation**

The Dialogue System is distributed as a Unity package. You can install it in one of two ways using the **Unity Package Manager**:

### **Option 1: Install from Local Zip**

1. Download the package contents as a `.zip` file.  
2. In Unity, go to **Window \> Package Manager**.  
3. Click the **\+** icon and choose **Install package from disk...**.  
4. Select the `.zip` file.

### **Option 2: Install from Git Repository**

1. In Unity, go to **Window \> Package Manager**.  
2. Click the **\+** icon and choose **Add package from Git URL...**.  
3. Enter the repository URL:  
    "com.company.dialoguesystem": "https://github.com/user/dialoguesystem.git"

This method makes it easier to pull updates.

## 

## **Package Samples**

The Dialogue System package ships with two optional **Samples** that can be imported through the Unity Package Manager:

### **1\. Default Definitions & Profiles**

This sample provides a set of **baseline assets** for immediate use:

* Default parameter definitions  
* Default node definitions  
* Common event types  
* Common option types  
* A preconfigured `DialogueProfile` ScriptableObject

Importing this sample is recommended if you want to quickly set up functional dialogues without writing custom nodes or parameters. Modifications can easily be made to add additional parameters to suit your needs.

### **2\. Default UI Implementation**

This sample builds on the first by including a **reference UI layer** that works directly with the Default Nodes and Profiles. It demonstrates:

* Rendering dialogue text to the screen  
* Displaying and selecting options in choice nodes  
* Driving the flow of dialogue via the `DialogueManager`

This sample is recommended as both a **learning tool** and a **starter template** for your own UI system. Begin using It by Dragging the DialogueCanvas into your scene and assign a DialogueManager to the DialogueUIManager. To create your own Dialogue UI you can modify the prefabs, scripts, and UI from this sample.

## **Programming Setup**

This  section is most relevant if you are opting not to use the “Default Definitions” Sample.

### **Defining Parameters**

Every dialogue system has unique requirements (e.g., text speed, choice timers, option colors). To support flexibility, the Dialogue System uses parameter classes. Define new parameters by creating classes that inherit from the base parameter types. If you don’t require any parameters beyond the defaults, then you can just use the base classes without defining a custom class:

* **Dialogue Settings** – Apply to the dialogue interaction as a whole (e.g., background changes, audio).  
  * Inherit from `DialogueSettings`.  
  * Default: `Description` field.  
  
* **Base Parameters** – Apply to all dialogues (regular and choice).

  * Inherit from `BaseParams`.  
  * Default: `Text` parameter.  

* **Choice Parameters** – Apply to choice dialogues only (e.g., time limits).

  * Inherit from `ChoiceParams`.  
  * Default: `hasTimeLimit` and `timeLimitDuration` parameters.  

* **Option Parameters** – Apply to each option of a choice dialogue.

  * Inherit from `OptionParams`.  
  * Default: `Text` parameter.  
 
* **Advance Parameters** – Supplies by the UI to be evaluated by options/choice dialogue  
    
  * Inherit from `AdvanceParams`  
  * Default: `choice` and `timedOut` parameters  
  * Custom Options can convert AdvanceParams to your custom class:  

### **Defining Nodes**

Each dialogue graph requires nodes that use your parameter classes. Define nodes as serializable classes with the `[UseWithGraph]` attribute, placed in an **Editor** folder or editor-only assembly.

* **Begin Dialogue Node** – Starting point of any dialogue. Holds `DialogueSettings`.  

* **Dialogue Node** – Regular dialogue. Holds `BaseParams`.  
  
* **Choice Dialogue Node** – Branching dialogue. Holds `BaseParams`, `ChoiceParams`, and `OptionParams`.  
  

### **Using Nodes**

1. Create a new Dialogue Graph via **Create \> Dialogue System \> Dialogue Graph**.  
2. Open the graph and place a **Begin Dialogue Node** (required, only one allowed).  
3. Add **Dialogue Nodes** or **Choice Nodes** as needed.  
 
4. Connect nodes by linking the `Next` port to the target node’s `Previous` port.  
5. Save the graph (**Ctrl+S**). A DialogueAsset is created with a Dialogue Graph icon and sub-assets.  


## **Defining Options**

To add options to a Choice Dialogue:

1. Define a class inheriting from `Option`. Implement `EvaluateCondition()` to determine visibility.  
   * Example: always return `true` for unconditional options.  
   * Example: Evaluate a Value  

2. Define an editor node class inheriting from `OptionNode<TOption>`. Options should apply a `[UseWithContext]` Attribute as follows, with `DefaultChoiceNode` replaced with your defined ChoiceNode
 
3. In the Dialogue Graph, add options using **Add a Block**. Options require the press of a button to be initialized properly.  

## **Dialogue Manager**

The **DialogueManager** is the runtime component that executes DialogueAssets and interfaces with your UI.

### **Starting Dialogue**

DialogueManager.BeginDialogue(myDialogueAsset);

* If a dialogue is already running, this call does nothing.  
* On start, the `DialogueManager.startedDialogue` event is invoked.

### **Advancing Dialogue**

DialogueParams next \= DialogueManager.AdvanceDialogue();  
DialogueParams next \= DialogueManager.AdvanceDialogue(myAdvanceparams);  
DialogueParams\<T1,T2,T3\> next \= DialogueManager.AdvanceDialogue\<T1,T2,T3\>();  
DialogueParams\<T1,T2,T3\> next \= DialogueManager.AdvanceDialogue\<T1,T2,T3\>(myAdvanceparams);

* Returns a `DialogueParams` /  `DialogueParams<T1,T2,T3>` object describing the current dialogue step.  
* Type parameters correspond to your chosen Base, Choice, and Option parameter classes. If the parameters of a given dialogue do not correspond to the provided types then the param values will be null.  
* The non-templated DialogueParams provides templated Getter functions for each parameter type as well as a getter function for the type of each.  
* For subsequent advances, pass an `AdvanceParams` with user interaction details.  
* Returns `null` when dialogue ends.

### **Other Methods**

* `RefreshDialogue()` – Re-evaluates options for the most recent dialogue.  
* `EndDialogue()` – Stops the current dialogue immediately (does not notify UI).

### **Retrieving Parameters**

When retrieving from a non-generic `DialogueParams` you must use the Getter method for your desired type. The getter function will return a null or empty list if the type is incorrect. This method of retrieving parameters should only be used when you are using a mix of different parameters within your Graph. If you have only one defined parameter/node set then use the generic `DialogueParams<T1,T2,T3>` methods  

* **Dialogue Settings:** `DialogueManager.GetSettings<T>()`  
* **Base Params:** `DialogueParams.GetBaseParams<T>()`  
* **Choice Params:** `DialogueParams.GetChoiceParams<T>()`  
* **Option Params:** `DialogueParams.GetOptions<T>()`

When using The generic , the parameters can be accessed directly through fields of your desired type. The template arguments are the BaseParams, ChoiceParams, and OptionParams respectively. (I recommend using type-alias for long type name)  

## **Redirects**

Redirect nodes allow branching without displaying dialogue.

* **Sequential Redirect Node:** Evaluates options top-to-bottom, takes the first that returns true.  
* **Random Redirect Node:** Evaluates all options and selects a weighted random from those returning true.  
* If no options evaluate to true, the `Default` port is followed.

## **Events**

### **Creating Events**

Dialogue Events are ScriptableObjects:

* Create via **Create \> Dialogue System \> Events \>** .  
* Supports typed (ex: `DSEvent<int>`) and untyped (`DSEvent`) variants. It’s recommended to use the non-generic type of events as the inspector doesn’t handle generics well.  
  *  ex: Use `DSEventInt` over `DSEvent<int>` where `DSEventInt` : `DSEvent<int>`  
  * ScriptableObjects cannot exist as an open generic. Must have a specific class such as `DSEventInt` 

### **Using Events in Graphs**

* Add an `EventNode` and assign your event.  
* Attach it to an output port of a node to trigger on completion.  
* The Begin Dialogue Node also has an **End Events** port.

### **Managing Listeners**

DSEvents have the same interface as UnityEvents. You can add or remove Listeners using AddListener and RemoveListener respectively or RemoveAllListeners. 

Listeners can also be scoped to individual DialogueManagers. This can be useful for:

* Split-screen multiplayer   
* When using multiple types of Dialogue interfaces

### **Custom Event Types**

Create a new class inheriting from `DSEvent<T>`:

## **Values**

**DSValues** allow setting, checking, and modifying values within dialogue. They are ScriptableObjects stored in a `DSValueHolder`.

### **Creating Values**

* Create via **Create \> Dialogue System \> Dialogue Value**.  
* Assign a unique name, default type, and value. Type and value correspond to the initial Global value.  
* Add `[DialogueValue]` to custom classes to include them in the type dropdown.

### **Setting Values in Dialogue**

* Use a **ValueSetter Node** on a node’s output.  
* Example: Set `MyValue` to 5.6 when advancing from Node A to Node B.

* Types not included in `ValueSetterNode` Default Type dropdown must be defined using separate Node inheriting from `CustomValueSetterNode<T>`

### **Value Scopes**

* **Global:** Applies to all managers, persists until app closes.  
* **Manager:** Applies per DialogueManager, persists until app closes.  
* **Dialogue:** Only valid for the current dialogue and applies per DialogueManager.

Lowest available scope is used (Dialogue → Manager → Global). DialogueManager is identified by the ManagerName field, so 2 Managers with the same name will be treated as the same. This is important when using multiple scenes and persisting references when saving/loading between sessions.

### **Using Values in Text**

Insert values into text using `{}`:

ex: Hello {playerName}\!

Each `{}` creates an input port for a DSValue. The string that in placed into the text is retrieved using the .ToString method of whatever value is assigned.

### **Using Values in Code**

DSValues provide several methods for getting/setting values with scope:

### **Saving and Restoring Values**

Two systems exist, a High-Level and a Low-Level. 

* **The High-Level** system returns and restores from strings which represent JSON that has been serialized using NewtonSoft. This system does not preserve any values which reference UnityEngine.Object (ScriptableObject, Textures, GameObjects, Prefabs, etc).  
* **The Low Level** returns the raw data classes for the DSValue and DSValueHolders respectively. Saving and restoring these values between sessions will require you to use your own serialization system. The purpose of this system is if you wish to create a system which can preserve UnityEngine.Object references.

| System | Easy? | Supports UnityEngine.Object? | Polymorphism? | Use Case |
| ----- | ----- | ----- | ----- | ----- |
| High-Level (JSON) | ✅ | ❌ | ✅ | Most projects |
| Low-Level (custom data) | ❌ | ✅ | ✅ | Custom serializers |

* **High-Level:** `GetSaveData()` / `RestoreFromSaveData()` → JSON string (uses Newtonsoft).  
* **Low-Level:** `GetData()` / `RestoreFromData()` → raw classes for custom serialization.

## **Quick Start Tutorial**

1. Create a Dialogue Graph.  
2. Add a Begin Dialogue Node and a Dialogue Node.  
3. Connect them.  
4. Save the graph.  
5. Attach a DialogueManager to a GameObject.  
6. Start the dialogue in code:  
7. DialogueManager.BeginDialogue(myGraph);  
8. In your UI, listen to `DialogueManager.startedDialogue` and show `DialogueParams.Text`.

## **Troubleshooting**

* **Error: “No Begin Dialogue Node found”** → Add one to your graph.  
* **Options not appearing** → Ensure `EvaluateCondition()` returns `true`.  
* **Graph doesn’t save** → Save manually with **Ctrl+S**.

## **Glossary**

* **Dialogue Graph:** Asset containing dialogue flow.  
* **DialogueAsset:** Asset created by Dialogue Graph.  
* **DialogueParams:** Runtime info about current dialogue step.  
* **DSValue:** ScriptableObject representing a dynamic variable.  
* **Redirect Node:** Node used for branching without output.  
* **DSEvent:** ScriptableObject event that can be raised from dialogue.

## **Developer Notes**

Some quirks exist within this Dialogue System, many of which are a result of using the Graph Toolkit which is still in the experimental stage. There are several features I’d like to change or have and intend to implement if they become available.

* **Node Display Names** \- Nodes having the exact name of their classes is ugly and unintuitive. If the option to create custom display names is added I’d like to incorporate this.  
* **Node Colors** \- Like Display Names, this would just be easier for visually understanding the graph and is unfortunately not an option within the Graph Toolkit as of 0.3.0-exp.1  
* **Option Initialization** \- Currently when creating a new Option it prompts with a button to initialize the Node. This is necessary because the parameters of an Option depend on the parent ContextNode. Unfortunately a current bug doesn’t allow the BlockNode to access its ContextNode on first initialization. Changing the boolean option prompts a second init. A bug report for this has already been submitted and can be found [Here](https://issuetracker.unity3d.com/issues/blocknode-dot-contextnode-is-not-defined-when-initializing-options-and-ports). If the bug has been fixed then the InitializeNode abstraction can be removed.  
* **Small text Fields** \- Currently the text fields are displayed as only regular string fields. While the developers of the Graph Toolkit have expressed intent to add support for MultiLine text, they have yet to do so. This means editing text within the graph may be somewhat inconvenient. When the ability to add MultiLine is implemented it will likely be added as a Builder Function on the Options Builder as other attributes have been. In this case, implementation for this could involve adding a new parameter to the `DialgoueGraphUtility.AddNodeOption` function and calling the corresponding builder function when the parameter is true.  
* **Parent Context Nodes** \- Currently all Options and Option Nodes are defined within the Samples. While some Options are practically fundamental and would be used in any implementation, they can not be added to the main package as they require the UseWithContext attribute to include the developers implemented ChoiceDialogueNode. If the UseWithContext attribute were changed to support parent ContextNodes then these options could be moved into the main Package using the base ChoiceDialogueNode in the attribute instead. An existing bug report pertaining to this issue can be found [Here](https://issuetracker.unity3d.com/issues/blocknodes-available-to-contextnode-are-not-properly-displayed-when-usewithcontext-is-set-to-a-parent-contextnode).  
* **Custom Value Type Setters** \- The Graph Toolkit has no means of creating dropdown other than using Enums, which of course can’t be modified at runtime. While the DialogueValueType attribute can add a type to the DSValue inspector, the same behavior can not be replicated for the Value Setter Node. This is why the custom Setter Node is required for this. In the case that the option for custom dropdowns is added, the CustomValueSetterNode can be removed and the ValueSetterNode be modified to populate its “Type” option in the same way as the SerializedValueBase’s PropertyDrawer  
* **Node hierarchy** \- Currently when creating a new node they are only grouped by Regular Nodes and Context Nodes. This is a bit inconvenient and I’d prefer to be able to define a custom creation hierarchy like can be done in the regular Create Menu, but unfortunately this cannot be done in the Graph Toolkit as of 0.3.0-exp.1  
* **Multiple Node Outputs** \- A Next Dialogue Port can currently be connected to multiple ports. As it is, this properly displays an error as a single Dialogue should not be connected to multiple, but if the option were added to do so I’d like to be able to disallow multiple connections completely.
