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

    public Hashtable coordinatesList { get; private set; }
    public List<string> keys { get; private set; }

    private int NM_M = 1852;            // Nautical miles to meters conversion
    private double FT_M = 0.3048;       // Feet to meters conversion
    private double UnitRatio = 0.001;   // Real world 1 meter is represented by 0.001 Unity units

    // Checks if data file exists
    void Awake()
    {
        if (File.Exists(defaultPath))
        {
            coordinatesList = ReadFlightsFromFile(defaultPath);
        }
        else if (File.Exists(altPath))
        {
            coordinatesList = ReadFlightsFromFile(altPath);
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
}
