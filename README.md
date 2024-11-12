# What is this project about?  
This project is a Unity demo that displays the possible use cases for eye tracking using the Meta Quest Pro, Meta XR All-in-One SDK, and Meta Movement.  

# What is in each folder?  
All the files associated with eye tracking are in the **_Eye Folder_**.    

**Materials** - Materials that are used in coloring the eye models  
**Models** - Contains the base imported eye mesh, and the final assembled eye model  
**Prefabs** - Pre-assembled eyes with all the required scripts and interactable eye objects  
**Scripts** - Scripts associated with eye tracking and interactable eye objects
 
# Documentation
_All scripts are well documented, and this is supplemental to their comments_  


### EyeRaycast - Enables ray casts to be shot from the eyes
**Variables**
 * rayMaterial - Determines the color of the raycast
 * isRayVisible - Determines if the raycast is visible to the user
 * maxHitDist - Maximum distance in which the raycast will look for an object to hit

**Methods**
* Start - Initializes the lineRenderer
* Update - Shoots ray casts each frame from each object which has this script
    * The raycast may hit an object, but if it does not have a script with type "**_RayInterface_**" then it will ignore the hit
* enableRay - Renders the ray from the eyeball to the target if the input is True
  * Important Information
    * This script is ONLY intended for the eye objects
    
---
### RayInterface - Empty interface that allows for eye raycast interactions
**Methods**
* isHit - Executes on the initial hit of the object
* isSelected - Each contiguous hit after isHit executes this function
* isUnselected - Executes whenever the object is no longer being hit

**Important Information**
* The logic which implements when these methods are executed is in EyeRaycast

---
### BaseEyeInteractable - Base interactable that implements simple interactions given raycast hit info
**Methods**
* isHit - Logs to the console that the object has been hit by a raycast
* isSelected - Logs to the console that the object is still being hit by a raycast
* isUnselected - Logs to the console that the object has stopped being hit by a raycast

**Important Information**
* This script inherits from **_RayInterface_**, so if an object has this script and is hit, then it will be recognized as a valid hit

---
### ExampleEyeInteractable - Slightly more useful example of an eye interactable
**Variables**
* matHit - The object will change to this material when hit
* matSelected - The object will change to this material while selected
* matUnselected - The object will change to this material when unselected
* timeTillSelected - The length in seconds that the user needs to hit an object to register it as selected

**Methods**
* isHit - Changes the objects material to **__matHit__**
* isSelected - Changes the objects material to **__matSelected__** after the desired **__timeTillSelected__**
* isUnselected - Changes the objects material to **__matUnselected__**
* executeSelect - Given the input time, delay the object from being selected for that much time
* changeMaterialTo - Change the object material to the input material 

**Important Information**
* This script inherits from **_BaseEyeInteractable_**, so if an object has this script and is hit, then it will be recognized as a valid hit
