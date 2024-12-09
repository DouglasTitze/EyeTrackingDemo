using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Btn_SaveData : MonoBehaviour
{
    [SerializeField] SaveCanvasData saveCanvasDataScript;
    [SerializeField] TextMeshProUGUI canvasBody;
    private string startingText;

    private void Start()
    {
        startingText = "In a real scenario this would be automated, but since this is a demo, enter a random participant ID and click Save Data to export the tracked data to a CSV.";
    }


    public void saveData()
    {
        canvasBody.text = startingText;
        canvasBody.text += "\n\n\n\n";

        bool completed = saveCanvasDataScript.writeToCSV();

        if (completed)
        {
            canvasBody.text += $"Your data has been sucessfully exported to the path: {saveCanvasDataScript.path}\n\n";
            canvasBody.text += "If pressed twice by accident, or on purpose, the duplicate data is not added twice to the CSV. Only non-exisiting data is added.";
        }
        else
        {
            canvasBody.text += "Your data has been unsucessfully exported.\n\n";
            canvasBody.text += "Please enter a participant ID.";
        }
    }
}
