using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string FilePath = @"C:\Users\kajus\Aircraft trajectory visualization\Assets\flights.csv";
    public Hashtable CoordinatesList;
    private List<string> Flights;
    private int NmToM = 1852;
    private double FtToM = 0.3048;
    private double EngineUnitsRatio = 0.001;

    // Reads coordinates from CSV file
    public Hashtable ReadFlightsFromFile()
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
                var coordinates = new Coordinates(
                Convert.ToDouble(data[2]) * NmToM * EngineUnitsRatio + 26.378,            // Reiktu prideti priklausomybe nuo radaro koordinaciu
                Convert.ToDouble(data[3]) * NmToM * EngineUnitsRatio + 0.192,            // Reiktu prideti priklausomybe nuo radaro koordinaciu
                Convert.ToDouble(data[4]) * FtToM * EngineUnitsRatio + 23.53,          // Reiktu prideti priklausomybe nuo radaro koordinaciu
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

            Debug.Log("Finished reading data from file!");
            this.CoordinatesList = flightsTable;
            return flightsTable;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
