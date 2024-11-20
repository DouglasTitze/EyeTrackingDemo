using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveCanvasData : MonoBehaviour
{
    [SerializeField] private CanvasEyeInteractable canvas;
    [SerializeField] private string canvasIdentifier;
    [SerializeField] private DataManager dataManager;

    // Fill these variables in using the dataManger
    [HideInInspector] public string path;
    private string participantID;

    // Start is called before the first frame update
    void Start()
    {
        // Assign path
        path = dataManager.path;

        // Assign participantID;
        // In a real scenario this !!WILL!! need to happen elsewere, becuase the participant ID will not be filled in at the start of the scenario.
        participantID = dataManager.participantID;
    }

    /// <summary>
    /// Writes all data to the CSV file at path
    /// This method is executed by a button in the scenario
    /// </summary>
    public void writeToCSV()
    {
        HashSet<string> csvData = dataManager.csvData;

        List<string> rowsToAdd = new List<string> { };

        // REPLACE WITH Participant ID
        string date = DateTime.Now.ToString("MM/dd/yyyy");

        foreach (var (interactionStartTime, interactionDuration) in canvas.allInteractions)
        {
            // These MUST be in the same order as the headers
            string row = date + ",";
            row += participantID + ",";
            row += canvasIdentifier + ",";
            row += interactionStartTime.ToString("HH:mm:ss:fff") + ",";
            row += floatToTimeString(interactionDuration);


            // If the row is a duplicate in the CSV, then skip adding it to the list
            if (csvData.Contains(row)) { continue; }

            rowsToAdd.Add(row);
            csvData.Add(row);
        }

        // This method does what it says, but also adds a newline to the end of each string in the list
        File.AppendAllLines(path, rowsToAdd);
    }

    /// <summary>
    /// Converts the input time into a string
    /// </summary>
    /// <param name="milliseconds">
    /// A double representing the time spent in milliseconds
    /// </param>
    /// <returns>
    /// Return string in the format of Mm Ss MMMMms
    /// </returns>
    private string floatToTimeString(double milliseconds)
    {
        int min = (int)(milliseconds / 60000);
        int sec = (int)((milliseconds % 60000)/ 1000);
        int milli = (int)(milliseconds % 1000);

        return $"{min}m {sec}s {milli}ms";
    }
}
