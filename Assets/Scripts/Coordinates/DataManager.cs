using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    Transform radar;

    [SerializeField]
    String flightsFileName;

    [SerializeField]
    String ILSFileName;

    private string flightsFilePath;
    private string ilsFilePath;
    public Hashtable coordinatesList { get; private set; }
    public List<string> keys { get; private set; }
    public List<Coordinates> ils { get; private set; }

    private int NM_M = 1852;            // Nautical miles to meters conversion
    private double FT_M = 0.3048;       // Feet to meters conversion
    private double UnitRatio = 0.001;   // Real world 1 meter is represented by 0.001 Unity units

    // Checks if data file exists
    void Awake()
    {
        flightsFilePath = Application.dataPath + "/" + flightsFileName;
        ilsFilePath = Application.dataPath + "/" + ILSFileName;

        if (!File.Exists(flightsFilePath) || !File.Exists(ilsFilePath))
        {
            Debug.LogError("Could not initialize Data Manager component - Missing data files");
            return;
        }
        coordinatesList = ReadFlights(flightsFilePath);
        ils = ReadIls(ilsFilePath);
    }

    // Reads flights coordinates from CSV file
    private Hashtable ReadFlights(string FilePath)
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

    // Reads landing procedure coordinates from CSV file
    private List<Coordinates> ReadIls(String FilePath)
    {
        var coordinatesList = new List<Coordinates>();
        using (var reader = new StreamReader(FilePath))
        {
            var line = reader.ReadLine();   // First line of data is unnecessary
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                var data = line.Split(',');
                var coordinates = new Coordinates(
                Convert.ToDouble(data[0]) * NM_M * UnitRatio + radar.position.x,    //34.06161
                Convert.ToDouble(data[1]) * NM_M * UnitRatio + radar.position.z,    //28.2831
                Convert.ToDouble(data[2]) * FT_M * UnitRatio + 0.16152);
                coordinatesList.Add(coordinates);
            }
        }
        return coordinatesList;
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
    public Coordinates(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
