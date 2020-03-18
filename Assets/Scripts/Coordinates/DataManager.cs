using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string defaultPath = @"C:\Users\kajus\Aircraft-trajectory-visualization\Assets\flights.csv";
    private string altPath = @"C:\Users\kajus\Aircraft trajectory visualization\Assets\flights.csv";
    private string approach1 = @"C:\Users\kajus\Aircraft-trajectory-visualization\Assets\d8_pom.csv";
    private string approach1Alt = @"C:\Users\kajus\Aircraft trajectory visualization\Assets\d8_pom.csv";
    private string approach2 = @"C:\Users\kajus\Aircraft-trajectory-visualization\Assets\d8_pom_final_approach.csv";
    private string approach2Alt = @"C:\Users\kajus\Aircraft trajectory visualization\Assets\d8_pom_final_approach.csv";
    private string approach3 = @"C:\Users\kajus\Aircraft-trajectory-visualization\Assets\final_approach.csv";
    private string approach3Alt = @"C:\Users\kajus\Aircraft trajectory visualization\Assets\final_approach.csv";

    public Hashtable coordinatesList { get; private set; }
    public List<string> keys { get; private set; }

    public Hashtable approach { get; private set; }
    public List<string> approachKeys { get; private set; }

    private int NM_M = 1852;            // Nautical miles to meters conversion
    private double FT_M = 0.3048;       // Feet to meters conversion
    private double UnitRatio = 0.001;   // Real world 1 meter is represented by 0.001 Unity units

    // Checks if data file exists
    void Awake()
    {
        var approachFiles = new List<string>();
        approachFiles.Add(approach1); approachFiles.Add(approach2); approachFiles.Add(approach3);
        var approachFilesAlt = new List<string>();
        approachFiles.Add(approach1Alt); approachFiles.Add(approach2Alt); approachFiles.Add(approach3Alt);
        if (File.Exists(defaultPath))
        {
            coordinatesList = ReadFlightsFromFile(defaultPath);
            approach = ApproachFromFile(approachFiles);
        }
        else if (File.Exists(altPath))
        {
            coordinatesList = ReadFlightsFromFile(altPath);
            approach = ApproachFromFile(approachFilesAlt);
        }
        else
            Debug.LogError("Could not initialize Data Manager component - File does not exist");
    }

    // Reads coordinates from CSV file
    private Hashtable ReadFlightsFromFile(string FilePath)
    {
        var coordinatesList = new List<Coordinates>();
        var flightsTable = new Hashtable();

        using (var reader = new StreamReader(FilePath))
        {
            var line = reader.ReadLine();   // First line of data is unnecessary
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                var data = line.Split(',');
                var coordinates = new Coordinates(          // ADD or SUBTRACT values to first two to move entire trajectory
                Convert.ToDouble(data[2]) * NM_M * UnitRatio + -147.0122375, //-147.7389375     
                Convert.ToDouble(data[3]) * NM_M * UnitRatio + 122.9220125, //122.4673125
                Convert.ToDouble(data[4]) * FT_M * UnitRatio + 0.16152,
                data[1],    // Flight ID
                data[0]);   // Time coordinates were retrieved

                if (!flightsTable.ContainsKey(data[1]))
                {
                    var newList = new List<Coordinates>();
                    newList.Add(coordinates);
                    flightsTable.Add(data[1], newList);
                }
                else
                    ((List<Coordinates>)flightsTable[data[1]]).Add(coordinates);
            }
            keys = flightsTable.Keys.Cast<string>().ToList();
            return flightsTable;
        }
    }

    // Reads approach coordinates from CSV files
    private Hashtable ApproachFromFile(List<string> FilePaths)
    {
        var coordinatesList = new List<Coordinates>();
        var approachTable = new Hashtable();

        foreach(var file in FilePaths)
        {
            using (var reader = new StreamReader(file))
            {
                var line = reader.ReadLine();   // First line of data is unnecessary
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var data = line.Split(',');
                    var coordinates = new Coordinates(
                    Convert.ToDouble(data[2]) * NM_M * UnitRatio + -34.06161, 
                    Convert.ToDouble(data[3]) * NM_M * UnitRatio + -28.2831,
                    Convert.ToDouble(data[4]) * FT_M * UnitRatio + 0.16152,
                    data[1]);

                    if (!approachTable.ContainsKey(data[1]))
                    {
                        var newList = new List<Coordinates>();
                        newList.Add(coordinates);
                        approachTable.Add(data[1], newList);
                    }
                    else
                        ((List<Coordinates>)approachTable[data[1]]).Add(coordinates);
                }
            }
        }
        keys = approachTable.Keys.Cast<string>().ToList();
        return approachTable;
    }
}

public class Coordinates
{
    public double x;
    public double y;
    public double z;
    public string flight;
    public string time;

    public Coordinates(double x, double y, double z, string flight, string time)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.flight = flight;
        this.time = time;
    }
    public Coordinates(double x, double y, double z, string flight)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
