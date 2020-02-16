using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryMapper : MonoBehaviour
{
    LineRenderer lr;
    DataManager dm;
    private Hashtable coordinates;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        var dmObj = GameObject.Find("ReadFile_Btn");
        dm = dmObj.GetComponent<DataManager>();
    }

    // Populates the line with the coordinates of the flight
    public void RenderTrajectory(string flight)
    {
        lr.transform.Rotate(0, 58, 0, Space.World);
        List<Coordinates> coordinatesList = (List<Coordinates>)dm.CoordinatesList[flight];
        lr.positionCount = coordinatesList.Count;
        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, new Vector3((float)coordinatesList[i].x, (float)coordinatesList[i].z, (float)coordinatesList[i].y));
        }
        Debug.Log("Landing coordinates are: X " + coordinatesList[lr.positionCount - 1].x + "  Y " + coordinatesList[lr.positionCount - 1].z + "  Z " + coordinatesList[lr.positionCount - 1].y);
        Debug.Log("Take-off coordinates are: X " + coordinatesList[0].x + "  Y " + coordinatesList[0].z + "  Z " + coordinatesList[0].y);
    }

    // 
    public void Initialize(Hashtable coordinates)
    {
        this.coordinates = coordinates;
        // Render default flight
        RenderTrajectory("AZA1271");

        var aircraft = GameObject.Find("Aircraft");
        var ac = aircraft.GetComponent<AircraftController>();
        ac.StartFlight("AZA1271");
    }
}
