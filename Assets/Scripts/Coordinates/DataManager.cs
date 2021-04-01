using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEditor;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    Transform radar;

    [SerializeField]
    Transform runway;

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
        /* Used to align flight to runway */
        var runwayReferenceX = runway.position.x - Convert.ToDouble(2);
        var runwayReferenceZ = runway.position.z - Convert.ToDouble(3);
        var coordinatesPattern = @"-?\d{1,3}\.\d{1,6},-?\d{1,3}\.\d{1,6}";
        var altitudePattern = @"([1-9]$|[1-9]\d{1,6}$)";
        var flightNumberPattern = @"[A-Z]{1,4}(\d|[A-Z]){2,4}";

        using (var reader = new StreamReader(FilePath))
        {
            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                string flightNumber = Regex.Match(line, flightNumberPattern).Value;
                string altitude = Regex.Match(line, altitudePattern).Value;
                var data = Regex.Match(line, coordinatesPattern).Value.Split(',');
                if (flightNumber == "" || altitude == "" || data == null)
                {
                    Debug.LogError("Invalid flight data format");
                    if (EditorUtility.DisplayDialog("Invalid flight data format",
                        "Expected format: FLIGHT_NUMBER, X, Y, ALTITUDE", "Close"))
                    {
                        Application.Quit();
                    }
                }
                var coordinates = new Coordinates(
                    Convert.ToDouble(data[0]) * NM_M * UnitRatio + runwayReferenceX,
                    Convert.ToDouble(data[1]) * NM_M * UnitRatio + runwayReferenceZ,
                    Convert.ToDouble(altitude) * FT_M * UnitRatio + 0.16152,
                    flightNumber);

                if (!flightsTable.ContainsKey(flightNumber))
                {
                    var newList = new List<Coordinates>();
                    newList.Add(coordinates);
                    flightsTable.Add(flightNumber, newList);
                }
                else
                    ((List<Coordinates>)flightsTable[flightNumber]).Add(coordinates);
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
            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                var data = line.Split(',');
                var coordinates = new Coordinates(
                    Convert.ToDouble(data[0]) * NM_M * UnitRatio + radar.position.x,
                    Convert.ToDouble(data[1]) * NM_M * UnitRatio + radar.position.z,
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

    public Coordinates(double x, double y, double z, string flight)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.flight = flight;
    }
    public Coordinates(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
