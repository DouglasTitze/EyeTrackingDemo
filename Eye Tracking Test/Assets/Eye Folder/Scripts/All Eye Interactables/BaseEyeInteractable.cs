using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEyeInteractable : MonoBehaviour, RayInterface
{
    /// <summary>
    /// This script is only executed on the initial hit of the object
    /// <summary>
    public virtual void isHit(RaycastHit hitInfo)
    {
        Debug.Log(hitInfo.collider.gameObject);
    }

    /// <summary>
    /// If the object has been hit, then each contiguous hit executes this function
    /// </summary>
    public virtual void isSelected(RaycastHit hitInfo)
    {
        Debug.Log(hitInfo.collider.gameObject);
    }

    /// <summary>
    /// Runs whenever the object is no longer being hit by a ray cast
    /// </summary>
    public virtual void isUnselected()
    {
        Debug.Log(gameObject);
    }
}
