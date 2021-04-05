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
    string coordinatesPattern = @"(-?\d{1,3}\.\d{1,6}|-?\d{1,3}),(-?\d{1,3}\.\d{1,6}|-?\d{1,3})";
    string altitudePattern = @"([0-9]$|[1-9]\d{1,6}$)";
    string flightNumberPattern = @"[A-Z]{1,4}(\d|[A-Z]){2,4}";

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
        double runwayReferenceX = 0;
        double runwayReferenceZ = 0;

        /* Calculating runway reference coordinates
         * Used to adjust flight coordinates so aircraft lands/takes off at runway coordinates */
        using (var reader = new StreamReader(FilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var nextLine = reader.ReadLine();
                var flightNumber = Regex.Match(line, flightNumberPattern).Value;
                var nextFlightNumber = Regex.Match(nextLine, flightNumberPattern).Value;
                if (Regex.IsMatch(line, @",0$") && flightNumber != nextFlightNumber)
                {
                    var refCoordinates = Regex.Match(line, coordinatesPattern).Value.Split(',');
                    runwayReferenceX = Convert.ToDouble(refCoordinates[0]) * NM_M * UnitRatio - runway.position.x;
                    runwayReferenceZ = Convert.ToDouble(refCoordinates[1]) * NM_M * UnitRatio - runway.position.z;
                    break;
                }
            }
        }

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
                    Convert.ToDouble(data[0]) * NM_M * UnitRatio - runwayReferenceX,
                    Convert.ToDouble(data[1]) * NM_M * UnitRatio - runwayReferenceZ,
                    Convert.ToDouble(altitude) * FT_M * UnitRatio + runway.position.y,
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
                    Convert.ToDouble(data[2]) * FT_M * UnitRatio + runway.position.y);
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
