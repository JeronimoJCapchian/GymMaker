# Interactiva.Core
##### Interactiva.Core represents all the core functionality that is likely to be used across our projects. This repository contains scripts to handle navigation (players), POIs (Points of Interest which can be anything the user might want to interact with), UI and utilities.

#### Dependencies
_You will need to install these from the package manager (under Unity Registry) for this package to work:_
- Cinemachine
- UnityEngine.InputSystem (new input system package)
- TextMeshPro
- DOTween (can be gotten from the Unity Asset Store [here](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676 "here"))

### Interactiva.Core.POIs

This is the namespace of all POI functionality. In essence, a POI is something you can interact with, therefore it has an "Interact" method that must be implemented. It also has methods to enable and disable object visual highlights so as to let the user know they can interact with the POI (How you implement these is up to you, an outline effect is included to use).

##### POIManager
Similar to many other Interactiva systems, the POI system follows a centralized manager pattern (similar to the *mediator design pattern*), by using the POIManager component. To set up POIs, simply drag this script to an empty GameObject in your scene that you will remember.

The POIManager class keeps track of the POIs we can currently focus towards. This works by using individual Collider Triggers in each POI; when they detect the player, they will send a message to update the focusable POI list to the manager.

##### POI

The POI class is an abstract base class for all objects the player can interact with. This class implements a listener for OnTriggerEnter that will fire when a Navigator enters it, so as to determine if the player can focus on it. To make a new POI, simply inherit from this class.

You will want to implement the following Properties:
- **CanInteract**: returns whether the player can interact with this POI. You can set up a condition for it to have moments where it can't be interacted with, or just return true if the player can always interact with it.
- **PromptText**: this returns the string which will be displayed to the player whent hey can interact with this POI. Note that the FocusComponent holds the initial part of the string "*Press key to *" so you will want to return just the action part, like "*play video*", "*focus*", "*interact*", etc. 
- **HidePromptOnInteract**: return whether the prompt will be hidden as soon as the player interacts.

To extend the functionality of POI there are two other  interfaces you might want to implement:
##### IPOIToggle:
IPOI Toggle is an interface that lets the player toggle the functionality of this POI every time they interact with it. You will want to implement this interface if your POI has two states (e.g. focused / unfocused , playing / not playing, etc). This will communicate to the FocusComponent how to treat this POI when its interacted with.
You will want to implement the following properties and methods:
- **AltPromptText**: since this POI has two states, this string will hold the prompt text to display when this POI has been toggled (e.g. interacted with). 
- **HideNavigator**: returns whether the navigator should be hidden when this POI is toggled true. Currently, the Navigator is hidden by toggling 
- IPOIFocus



###


* IFocusable is an interface which you will want to implement in conjunction with POI so that the POI can take control of the camera from the Navigator (for example when zooming in on objects or other interactive POIs that require camera control).
* IPlayable is an interface which you will want to implement if the activity is something that can be won or lost (like mini games).
* POIFocus is a class that lets us focus in on objects, and either view them from one angle, or rotate around them (similar to Peoria Art Fair pieces).
* POIVideo is a class that lest us play video from a VideoPlayer component on interact.
* FPFocusWrapper holds all the dependencies with the Navigation system. If not using Navigation system, you can safely delete this file. It acts as a wrapper; but also handles the Interact Input, camera transition functionality between IFocusable and the Navigator, and executing the highlighting methods in POI.

