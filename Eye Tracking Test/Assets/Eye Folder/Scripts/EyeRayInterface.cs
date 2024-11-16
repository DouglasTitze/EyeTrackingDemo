using UnityEngine;

/// <summary>
/// Interface used on all objects that are interactable with the ray
/// 
/// !!!!The logic which determines when these methods are executed is in 'EyeRaycast'!!!!
/// </summary>
public interface EyeRayInterface
{
    /// <summary>
    /// Executes on the initial hit of the object
    /// </summary>
    void isHit(RaycastHit hitInfo);

    /// <summary>
    /// Each contiguous hit after isHit executes this function
    /// </summary>
    void isSelected(RaycastHit hitInfo);

    /// <summary>
    /// Executes whenever the object is no longer being hit
    /// </summary>
    void isUnselected();
}