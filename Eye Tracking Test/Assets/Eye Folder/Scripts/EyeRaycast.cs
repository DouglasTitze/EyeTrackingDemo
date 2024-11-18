using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EyeRaycast : MonoBehaviour
{
    // Determines if the raycast is visible to the user
    [SerializeField] public bool isRayVisible = true;

    // When enabled the ray can interact with more than one interactable at once
    // If disabled, then it can only interact with one interactable at a time
    static private bool multiHitEnabled = true;

    // Maximum distance in which the raycast will look for an object to hit
    static public float maxHitDist = 100f;

    private LineRenderer lineRenderer;

    // Holds the last frames interactables (if mulitHit = False, then it only ever contains one element)
    private HashSet<EyeRayInterface> prevTargetsSet = new HashSet<EyeRayInterface>();

    private void Start() { initializeLineRenderer(); }

    private void Update()
    {
        if (multiHitEnabled)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxHitDist);

            processHits(hits);
        }
        else
        {
            RaycastHit hit;

            Physics.Raycast(transform.position, transform.forward, out hit, maxHitDist);

            processHit(hit);
        }
    }

    /// <summary>
    /// For each hit along the ray, process each hit and update prevTargets
    /// </summary>
    private void processHits(RaycastHit[] hits)
    {
        // If none of the hits are valid, then exit the function
        if (validHitList(hits) == false) { return; }

        RaycastHit lastHit = default;
        HashSet<EyeRayInterface> targetsSet = new HashSet<EyeRayInterface>();
        foreach (var hit in hits)
        {
            // Get all interactable scripts from the hit object
            EyeRayInterface[] targets = hit.collider.GetComponents<EyeRayInterface>();

            // Process all the targets, and store the valid targets in the temp set
            HashSet<EyeRayInterface> tempSet = processTargets(targets, hit); 

            // Update lastHit and the targetsSet
            if (tempSet.Count != 0) { lastHit = hit; }

            targetsSet.UnionWith(tempSet);
            
        }

        // If any object was hit then enable the ray
        if (lastHit.collider != null) { enableRay(lastHit); }
        else { lineRenderer.enabled = false; }

        // Unselect all the targets that are no longer being selected and update set
        updatePrevTargets(targetsSet);
    }

    /// <summary>
    /// Process the first interactable hit
    /// </summary>
    private void processHit(RaycastHit hit)
    {
        // Exit the function if the hit is not valid
        if (validHitList(new RaycastHit[] { hit }) == false) { return; }


        EyeRayInterface target = hit.collider.GetComponent<EyeRayInterface>();
        // If an event was triggered, then the target was valid
        if (executeTargetEvent(target, hit)) 
        {
            // Update prevTargets, enableRay, and exit the function
            updatePrevTargets(new HashSet<EyeRayInterface> { target });
            enableRay(hit);
        }
    }

    /// <summary>
    /// Removes all targets absent from the input set that were previously in prevTargets
    /// Also, updates prevTargets to the input set
    /// </summary>
    private void updatePrevTargets(HashSet<EyeRayInterface> targets)
    {
        foreach (var oldTarget in prevTargetsSet.Except(targets))
        {
            oldTarget.isUnselected();
        }

        prevTargetsSet = targets;
    }

    /// <summary>
    /// Execute Hit or Selected on the input target
    /// </summary>
    /// <returns>
    /// Return True if the input was valid and an event was triggered
    /// </returns>
    private bool executeTargetEvent(EyeRayInterface target, RaycastHit hit)
    {
        // A target passed in may not always be a valid target
        if (target == null) { return false; }

        // If target is not the prevTarget, then hit target
        if (prevTargetsSet.Contains(target) == false)
        {
            // Hit the target
            target.isHit(hit);
        }

        // Otherwise, select it
        else 
        { 
            target.isSelected(hit); 
        }

        return true;
    }

    /// <summary>
    /// Processes all the input targets and executes their events
    /// </summary>
    /// <returns>
    /// Returns a set of all the valid targets that were executed
    /// </returns>
    private HashSet<EyeRayInterface> processTargets(EyeRayInterface[] targets, RaycastHit hit)
    {
        HashSet<EyeRayInterface> validTargets = new HashSet<EyeRayInterface>();

        // Otherwise execute all target events
        foreach (EyeRayInterface target in targets)
        {
            // If it was a valid target, add it to the set
            if(executeTargetEvent(target, hit)) { validTargets.Add(target); }
        }

        return validTargets;
    }

    /// <summary>
    /// Validates the input hitlist as valid or invlaid
    /// </summary>
    /// <returns>
    /// Returns true if the list is valid
    /// </returns>
    private bool validHitList(RaycastHit[] hits)
    {
        // If htis is empty or if the first hit collider is null, then INVALID
        if (hits.Length == 0 || hits[0].collider == null)
        {
            updatePrevTargets(new HashSet<EyeRayInterface>());
            lineRenderer.enabled = false;
            return false;
        }
        else { return true; }
    }


    /********************** RAY SECTION **********************/

    /// <summary>
    /// Shows the ray from the eyeball to the target if the user enabled this functionallity
    /// </summary>
    /// <param name="hit"> Hit information to help display the Ray </param>
    private void enableRay(RaycastHit hit)
    {
        if (isRayVisible)
        {
            // Set the start and end points of the LineRenderer
            lineRenderer.SetPosition(0, transform.position);          // Start at the object's position
            lineRenderer.SetPosition(1, hit.point);                   // End at the hit point

            // Enable the LineRenderer
            lineRenderer.enabled = true;

            return;
        }

        else { return; }
    }

    /// <summary>
    /// Initializes line renderer that represents the raycast
    /// </summary>
    private void initializeLineRenderer()
    {
        // Initialize the LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f; // Set the starting width of the line
        lineRenderer.endWidth = 0.01f;   // Set the ending width of the line
    }
}