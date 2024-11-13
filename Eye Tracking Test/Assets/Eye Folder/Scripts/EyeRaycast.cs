using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRaycast : MonoBehaviour
{
    // Determines the color of the raycast
    [SerializeField] private Material rayMaterial;

    // Determines if the raycast is visible to the user
    [SerializeField] private bool isRayVisible = true;

    // Maximum distance in which the raycast will look for an object to hit
    static public float maxHitDist = 100f;

    private LineRenderer lineRenderer;
    private RayInterface prevTarget;

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
        RaycastHit hit;

        // Check if the ray hits an object
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxHitDist))
        {
            RayInterface target = hit.collider.GetComponent<RayInterface>();

            // If the target has a RayInterface continue
            if (target != null)
            {

                if (target != prevTarget)
                {
                    // Unselect previous target
                    if (prevTarget != null) { prevTarget.isUnselected(); }

                    // Make prevTarget equal new target
                    prevTarget = target;

                    // isHit is controlled by the EyeInteract, but can be modified by overriding
                    target.isHit(hit);
                }

                else
                {
                    prevTarget.isSelected(hit);
                }

                enableRay(isRayVisible, hit);

            }

            // If the target is null (occurs when the object is not interactable, but is hit)
            else
            {
                // Unselect previous target
                if (prevTarget != null)
                { 
                    prevTarget.isUnselected(); 
                    prevTarget = null;
                }

                lineRenderer.enabled = false;
            }
        }

        // If no object is hit at all
        else
        {
            // Disable the LineRenderer if no hit is detected
            lineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Shows the ray from eyeball to target if input variable b == True
    /// </summary>
    /// <param name="b"> Boolean to determine if the ray is visible or not</param>
    /// <param name="hit"> Hit information to help display the Ray </param>
    private void enableRay(bool b, RaycastHit hit)
    {
        if (b)
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
}