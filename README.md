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

- isRayVisible - Determines if the raycast is visible to the user, if the ray hits an interactable object
- multiHitEnabled - Allows the ray to hit multiple objects at once
- maxHitDist - Maximum distance in which the raycast will look for an object to hit

**Methods**

- Update - Shoots ray casts each frame from each object which has this script
  - The raycast may hit an object, but if it does not have a script with type "**_EyeRayInterface.cs_**" then it will ignore the hit
- processHits - Updates the state of **ALL** interactable scripts associated with the input hits.
- updatePrevTargets - Unselects the interatables that do not appear in the input set, and updates prevTargets to the input set
- executeTargetEvent - Executes the isSelected or isHit function of the input target (interactable script)
- validHitList - Checks if the list is empty, if so then update line renderer and prevTargets
- getClosestHit - Returns the closest hit to the eyes
- enableRay - Renders the ray from the eyeball to the target if **_isRayVisible_** is true
- initializeLineRenderer - Initializes the lineRenderer

**Important Information**

- This script is **ONLY** intended for the eye objects
- Even if "**_multiHitEnabled_**" is disabled, it does not disable an object from executing multiple EyeInteractables (targets)

---

### EyeRayInterface - Empty interface that allows for eye raycast interactions

**Methods**

- isHit - Executes on the initial hit of the object
- isSelected - Each contiguous hit after isHit executes this function
- isUnselected - Executes whenever the object is no longer being hit

**Important Information**

- The logic which executes each method above is implemented by "**_EyeRaycast.cs_**"

---

### LogEyeInteractable - Simple interactable that logs when each method is executed

**Methods**

- isHit - Logs to the console that the object has been hit by a raycast
- isSelected - Logs to the console that the object is still being hit by a raycast
- isUnselected - Logs to the console that the object has stopped being hit by a raycast

---

### ChangeColorEyeInteractable - Changes the material of the object depending on its interaction state

**Variables**

- matHit - The object will change to this material when hit
- matSelected - The object will change to this material while selected
- matUnselected - The object will change to this material when unselected
- timeTillSelected - Time delay in seconds between isHit and isSelected

**Methods**

- isHit - Changes the objects material to matHit
- isSelected - Changes the objects material to matSelected after the desired timeTillSelected
- isUnselected - Changes the objects material to matUnselected
- executeSelect - Given the input time, delay the object from being selected
- changeMaterialTo - Changes the object material to the input material

---

### CanvasEyeInteractable - Records data on how long and how many times the user looks at the canvas

_This script is tailored to the use case of the tutorial, but can be used as an example of how to interact with canvas's_

**Variables**

- liveUpdateEnabled - When enabled the tutorial continuously updates the times and total time looked at the canvas
- isSectionEnabled - When enabled, the section is currently on the correct section to begin displaying text
- allInteractions - A dictionary to contain all the unique eye interactions, and their respective durations

**Methods**

- isHit - Record the start time of the interaction
- isSelected - If liveUpdate is enabled, then continuously update the dictionary to the new total elapsed time of the interaction
- isUnselected - Update the dictionary to the new total elapsed time of the interaction
- getTotalTimeStr - Collect all the data from the dictionary and return it as a formatted string ("total_seconds s total_remaining_milliseconds ms")
- updateBodyText - Display the total time the canvas has been looked at and the total number of unique interactions the user has performed on the canvas.

**Important Information**

- The canvas MUST have a box collider which covers its entire panel to work
- isSectionEnabled is triggered by the **_CanvasTextManger.cs_** script, but it could be extrapolated to do it independetally by scanning the canvas's text box until the desired text is inside of the canvas
- The displayed amount of unique interactions is _usually_ double of the actual number, since both eyes are usually interacting with the canvas. **Despite this the recorded time is still acurate.**
