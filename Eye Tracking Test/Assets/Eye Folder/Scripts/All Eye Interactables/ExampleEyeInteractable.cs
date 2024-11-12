using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleEyeInteractable : BaseEyeInteractable
{
    // Change to theses materials when mode is a specifc executed
    [SerializeField] Material matHit;
    [SerializeField] Material matSelected;
    [SerializeField] Material matUnselected;

    // Length in seconds that the person needs to be looking at an object to register as selected
    [SerializeField] float timeTillSelected = 2f;

    private Coroutine timer;
    private bool selected = false;

    public override void isHit(RaycastHit hitInfo)
    {
        changeMaterialTo(matHit);
    }

    public override void isSelected(RaycastHit hitInfo)
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

    public override void isUnselected()
    {
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
        GetComponent<Renderer>().material = mat;
    }
}
