using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple interactable that logs when each method is executed
/// </summary>
public class LogEyeInteractable : MonoBehaviour, EyeRayInterface
{
    public virtual void isHit(RaycastHit hitInfo)
    {
        Debug.Log("The object '" + hitInfo.collider.gameObject + "' has been hit.");
    }

    public virtual void isSelected(RaycastHit hitInfo)
    {
        Debug.Log("The object '" + hitInfo.collider.gameObject + "' is being selected.");
    }

    public virtual void isUnselected()
    {
        Debug.Log("The object '" + gameObject + "' has stopped being selected.");
    }
}
