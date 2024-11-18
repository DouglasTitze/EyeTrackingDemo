using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EyeRaycast : MonoBehaviour
{
    // Determines if the raycast is visible to the user
    [SerializeField] public bool isRayVisible = true;

    // When enabled the ray can interact with more than one object at once
    // If disabled, then it can only interact with one obejct (if that one object has multiple interactables, that will still work)
    static private bool multiHitEnabled = true;

    // Maximum distance in which the raycast will look for an object to hit
    static public float maxHitDist = 100f;

    // Holds the last frames interactables
    private HashSet<EyeRayInterface> prevTargetsSet = new HashSet<EyeRayInterface>();

    private LineRenderer lineRenderer;

    private void Start() { initializeLineRenderer(); }

    private void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxHitDist);

        // Do NOT continue, if invalid hit list
        // If it is invalid it will also update the prevTargetsSet
        if (validHitList(hits) == false) { return; }

        if (multiHitEnabled)
        {
            processHits(hits);
        }
        else
        {
            // Get the closest hit
            RaycastHit closestHit = getClosestHit(hits);

            // Only input the closestHit into the processHits function
            processHits(new RaycastHit[] { closestHit });
        }
    }

    /// <summary>
    /// For each input hit, process each hit and update prevTargets
    /// </summary>
    private void processHits(RaycastHit[] hits)
    {
        // Set that will contain all the new targets
        HashSet<EyeRayInterface> targetsSet = new HashSet<EyeRayInterface>();


        RaycastHit lastHit = default;
        foreach (var hit in hits)
        {
            // Process all targets associated with the hit
            EyeRayInterface[] targets = hit.collider.GetComponents<EyeRayInterface>();
            foreach (EyeRayInterface target in targets)
            {
                // Execute select or hit on the input target
                executeTargetEvent(target, hit);
            }

            // Update the set with the new targets
            targetsSet.UnionWith(targets);

            // Update lastHit
            if (targets.Length != 0) { lastHit = hit; }
        }

        // If any object was hit then enable the ray
        if (lastHit.collider != null) { enableRay(lastHit); }
        else { lineRenderer.enabled = false; }

        // Unselect all the targets that are no longer being selected and update set
        updatePrevTargets(targetsSet);
    }

    /// <summary>
    /// Unselects all targets absent from the input set that were previously in prevTargets
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
    private void executeTargetEvent(EyeRayInterface target, RaycastHit hit)
    {
        // If target is in the prevTarget, then select target
        if (prevTargetsSet.Contains(target)) { target.isSelected(hit); }

        // Otherwise, hit it
        else { target.isHit(hit); }
    }

    /********************** Utility Section **********************/

    /// <summary>
    /// Checks if the input list is empty.
    /// If empty, then updates prevtargets and linerender
    /// </summary>
    /// <returns>
    /// Returns true if the list is valid
    /// </returns>
    private bool validHitList(RaycastHit[] hits)
    {
        // If hits is empty, then INVALID
        if (hits.Length == 0)
        {
            updatePrevTargets(new HashSet<EyeRayInterface>());
            lineRenderer.enabled = false;
            return false;
        }
        else 
        { 
            return true; 
        }
    }

    /// <summary>
    /// Iterates through the list and returns the hit with the smallest distance
    /// This will NOT work on an empty list.
    /// </summary>
    /// <returns>
    /// Returns the closest hit to the eyes
    /// </returns>
    private RaycastHit getClosestHit(RaycastHit[] hits)
    {
        RaycastHit closestHit = hits[0];

        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closestHit.distance) 
            { 
                closestHit = hit; 
            }
        }

        return closestHit;
    }

    /********************** Ray Section **********************/

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