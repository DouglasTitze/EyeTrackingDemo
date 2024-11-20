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
    private string outputFileName = "canvasData.csv";
    [HideInInspector] public string path;
    [HideInInspector] public HashSet<string> csvData;
    [HideInInspector] public string participantID = "Douglas";

    // CSV headers
    private string[] headers = new string[] { "Date", "Participant ID", "Canvas ID", "Start Time of Interaction", "Interaction Duration" };

    // Start is called before the first frame update
    void Start()
    {
        // Create path and check if the CSV file already exists
        path = Path.Combine(Application.persistentDataPath, outputFileName);
        if (File.Exists(path) == false)
        {
            createCSVFile();
        }

        csvData = new HashSet<string>();
        csvToSet();
    }

    /// <summary>
    /// Creates a csv file with the `outputFileName`
    /// This data is stored at:
    /// PC Path: C:/Users/[USER]/AppData/LocalLow/UsfCAMLS/Eye Tracking v1_0\canvasData.csv
    /// Headset Path: This PC\Quest Pro\Internal shared storage\Android\data\com.UsfCAMLS.EyeTrackingDemo.v1\files\canvasData.csv
    /// </summary>
    private void createCSVFile()
    {
        string headers = headerArrayToString();
        File.AppendAllText(path, headers);
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

    /// <summary>
    /// Extracts all csv data and stores it in a CSV
    /// This is used to ensure no duplicates are added to the CSV file
    /// </summary>
    private void csvToSet()
    {
        foreach (string line in File.ReadAllLines(path))
        {
            // Remove the newline character from the end of each line
            csvData.Add(line.Trim());
        }
    }
}
