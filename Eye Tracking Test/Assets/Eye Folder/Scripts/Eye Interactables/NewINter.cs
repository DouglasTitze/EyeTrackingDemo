using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Changes the material of the interactable depending on its interaction state
/// </summary>
public class NewINter : MonoBehaviour, EyeRayInterface
{
    [SerializeField] GameObject obj;

    // Change to theses materials when mode is a specifc executed
    [SerializeField] Material matHit;
    [SerializeField] Material matSelected;
    [SerializeField] Material matUnselected;

    // Time (in seconds) that the user needs to be interacting with an object to register it as selected
    // Can be switch to a static varibale so all objects which have this script automatically take the specified time to become select
    [SerializeField] float timeTillSelected = 1f;

    private Coroutine timer;
    private bool selected = false;
    private Material prevMaterial;

    private void Start()
    {
        prevMaterial = matUnselected;    
    }

    public void isHit(RaycastHit hitInfo)
    {
        changeMaterialTo(matHit);
    }

    public void isSelected(RaycastHit hitInfo)
    {
        // If the object is not selected, then check / begin the coroutine
        if (selected == false)
        {
            if (timer == null)
            {
                timer = StartCoroutine(executeSelect(timeTillSelected));
            }
        }

        // Else begin executing selected code
        else
        {
            changeMaterialTo(matSelected);
        }
    }

    public void isUnselected()
    {
        // If there was an object about to be selected, then cancel the timer
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }

        selected = false;

        changeMaterialTo(matUnselected);
    }

    /// <summary>
    /// Changes variables to detect hit
    /// </summary>
    /// <param name="time"> Time in seconds between intial hit and selected status </param>
    /// <returns></returns>
    private IEnumerator executeSelect(float time)
    {
        yield return new WaitForSeconds(time);

        selected = true;
        timer = null;
    }

    private void changeMaterialTo(Material mat)
    {
        if (prevMaterial == mat) { return; }

        obj.GetComponent<Renderer>().material = mat;
        prevMaterial = mat;
    }
}
