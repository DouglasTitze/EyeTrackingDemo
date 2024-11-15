using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIEyeRayToggle : MonoBehaviour
{
    [SerializeField] EyeRaycast eyeScript;
    [SerializeField] TextMeshProUGUI rayStatus;

    private void Start()
    {
        rayStatus.text = eyeScript.isRayVisible ? "Ray Is Visible" : "Ray Is NOT Visible";
    }
    public void toggleEyeRay()
    {
        eyeScript.isRayVisible = !eyeScript.isRayVisible;
        rayStatus.text = eyeScript.isRayVisible ? "Ray Is Visible" : "Ray Is NOT Visible";
    }
}
