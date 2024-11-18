using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class CanvasEyeInteractable : MonoBehaviour, EyeRayInterface
{
    [SerializeField] TextMeshProUGUI bodyText;

    // Updates the canvas bodyText continuously when enabled, else only update the canvas when initially called
    [SerializeField] public bool liveUpdateEnabled = true;

    [HideInInspector] public bool isSectionEnabled = false;

    private DateTime start;

    // Dictionary of <timeOfInteraction, totalInteractionTime (miliseconds)>
    private Dictionary<DateTime, Double> allInteractions = new Dictionary<DateTime, double>();

    public void isHit(RaycastHit hitInfo)
    {
        start = DateTime.Now;
        updateTotalTime();
    }

    public void isSelected(RaycastHit hitInfo)
    {
        // If live update is enabled then constantally update the interaction time
        if (liveUpdateEnabled) { updateTotalTime(); }
    }

    public void isUnselected()
    {
        updateTotalTime();
    }

    private void updateTotalTime()
    {
        if (allInteractions.ContainsKey(start))
        {

            TimeSpan timeDifference = DateTime.Now - start;
            double totalMilliseconds = timeDifference.TotalMilliseconds;

            allInteractions[start] = totalMilliseconds;
        }
        else
        {
            allInteractions.Add(start, 0.0);
        }
    }

    private void Update()
    {
        if (isSectionEnabled) { updateBodyText(); }   
    }

    /// <summary>
    /// Return a string of the total time which the user has looked at this canvas
    /// </summary>
    /// <returns></returns>
    public string getTotalTimeStr()
    {
        double total = 0.0;
        foreach (double d in allInteractions.Values)
        {
            total += d;
        }

        int seconds = (int)(total / 1000);
        int milli = (int)(total % 1000);

        return seconds + "s " + milli + "ms";
    }

    public void updateBodyText()
    {
        //string line1 = "This is the data collected given how often and how long you have looked at the canvas\n";
        //string line2 = "Total Unique Interactions: " + allInteractions.Count + "\n";
        //string line3 = "Total Interaction Time: " + getTotalTimeStr();
        //bodyText.text = line1 + line2 + line3;

        bodyText.text = "This is the data collected given how often and how long you have looked at the canvas\nTotal Unique Interactions: " + allInteractions.Count + "\nTotal Interaction Time: " + getTotalTimeStr();
    }
}