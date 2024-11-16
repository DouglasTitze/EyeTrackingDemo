using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EyeRaycast : MonoBehaviour
{
    // Determines the color of the raycast
    [SerializeField] private Material rayMaterial;

    // Determines if the raycast is visible to the user
    [SerializeField] public bool isRayVisible = true;

    // When enabled the ray can interact with more than one interactable at once
    // If disabled, then it can only interact with one interactable at a time
    static private bool multiHitEnabled = true;

    // Maximum distance in which the raycast will look for an object to hit
    static public float maxHitDist = 100f;

    private LineRenderer lineRenderer;

    // Holds the last frames interactables (if mulitHit = False, then it only ever contains one element)
    private HashSet<EyeRayInterface> prevTargets = new HashSet<EyeRayInterface>();

    private void Start()
    {
        // Initialize the LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f; // Set the starting width of the line
        lineRenderer.endWidth = 0.02f;   // Set the ending width of the line
        lineRenderer.material = rayMaterial;
    }

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
    /// For each hit along the ray, process each hit and update prevTargets
    /// </summary>
    private void processHits(RaycastHit[] hits)
    {
        RaycastHit lastHit = default;
        HashSet<EyeRayInterface> targets = new HashSet<EyeRayInterface>();
        foreach (var hit in hits)
        {
            EyeRayInterface target = hit.collider.GetComponent<EyeRayInterface>();

            // If the target has a EyeRayInterface continue
            if (target != null)
            {
                targets.Add(target);

                // If target is not in prevTargets, then hit target
                if (prevTargets.Contains(target) == false) { target.isHit(hit); }

                // Otherwise, select it
                else { target.isSelected(hit); }

                lastHit = hit;
            }
        }

        // If any object was hit then enable the ray
        if (lastHit.collider != null) { enableRay(lastHit); }
        else { lineRenderer.enabled = false; }

        // Unselect all the targets that are no longer being selected and update set
        updatePrevTargets(targets);
    }

    /// <summary>
    /// Process the first interactable hit
    /// </summary>
    private void processHit(RaycastHit hit)
    {
        EyeRayInterface target;
        if (hit.collider != null)
        {
            target = hit.collider.GetComponent<EyeRayInterface>();

            // If the target has a EyeRayInterface continue
            if (target != null)
            {
                // If target is not the prevTarget, then hit target
                if (prevTargets.Contains(target) == false)
                {
                    // Unselect previous target
                    updatePrevTargets(new HashSet<EyeRayInterface> { target });

                    // Hit the target
                    target.isHit(hit);
                }

                // Otherwise, select it
                else { target.isSelected(hit); }

                // Enable the ray
                enableRay(hit);

                // Exit the function early to avoid executing the unselect logic
                return;
            }
        }

        // This will only get hit if the hit was either null or not a EyeRayInterface object
        updatePrevTargets(new HashSet<EyeRayInterface>());
        lineRenderer.enabled = false;
    }

    /// <summary>
    /// Removes all targets absent from the input set that were previously in prevTargets
    /// Also, updates prevTargets to the input set
    /// </summary>
    private void updatePrevTargets(HashSet<EyeRayInterface> targets)
    {
        foreach (var oldTarget in prevTargets.Except(targets))
        {
            oldTarget.isUnselected();
        }

        prevTargets = targets;
    }
}