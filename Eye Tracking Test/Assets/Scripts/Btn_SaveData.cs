using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Btn_SaveData : MonoBehaviour
{
    [SerializeField] SaveCanvasData saveCanvasDataScript;
    [SerializeField] TextMeshProUGUI canvasBody;

    public void saveData()
    {
        saveCanvasDataScript.writeToCSV();
        canvasBody.text += "\n\n\n\n";
        canvasBody.text += $"Your data has been sucessfully exported to the path: {saveCanvasDataScript.path}\n\n";
        canvasBody.text += $"If pressed twice by accident, or on purpose, the duplicate data is not added twice to the CSV. Only non-exisiting data is added.";
    }
}
