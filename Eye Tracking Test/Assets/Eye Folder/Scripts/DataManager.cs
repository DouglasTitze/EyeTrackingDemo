using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// This script ensures the CSV is created and is the script that holds the path where the data is exported
/// All scipts writing to the CSV should get the path from this script to ensure consistency
/// </summary>
public class DataManager : MonoBehaviour
{
    // Path variables
    private string outputFileName;
    [HideInInspector] public string path;
    [HideInInspector] public HashSet<string> csvData;
    [HideInInspector] public string participantID;

    // CSV headers
    private string[] headers = new string[] { "Canvas ID", "Start Time of Interaction", "Interaction Duration" };


    private void Start()
    {
        csvData = new HashSet<string> { };
        participantID = "";
    }

    /// <summary>
    /// Creates a csv file with the `outputFileName`
    /// This data is stored at:
    /// PC Path: C:/Users/[USER]/AppData/LocalLow/UsfCAMLS/Eye Tracking v1_0\[ParticipantID]-[MM_dd_yyyy]-canvasData.csv
    /// Headset Path: This PC\Quest Pro\Internal shared storage\Android\data\com.UsfCAMLS.EyeTrackingDemo.v1\files\[ParticipantID]-[MM_dd_yyyy]-canvasData.csv
    /// </summary>
    public void createCSVFile()
    {
        // Create path and check if the CSV file already exists
        if (participantID != "")
        {
            string date = DateTime.Now.ToString("MM_dd_yyyy");
            outputFileName = participantID + "-" + date + "-" + "canvasData.csv";
            path = Path.Combine(Application.persistentDataPath, outputFileName);
            if (File.Exists(path) == false)
            {
                string headers = headerArrayToString();
                File.AppendAllText(path, headers);
            }
        }

    }

    /// <summary>
    /// Converts the headers declared at the top of the file into a string
    /// </summary>
    /// <returns>
    /// Returns the CSV friendly string version of the headers
    /// </returns>
    private string headerArrayToString()
    {
        string outputString = "";

        for (int i = 0; i < headers.Length - 1; i++)
        {
            outputString += headers[i] + ",";
        }

        outputString += headers[headers.Length - 1] + "\n";
        return outputString;
    }
}
