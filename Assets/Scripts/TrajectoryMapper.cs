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
        List<Coordinates> coordinatesList = (List<Coordinates>)dm.CoordinatesList[flight];
        lr.positionCount = coordinatesList.Count;
        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, new Vector3((float)coordinatesList[i].x, (float)coordinatesList[i].y, (float)coordinatesList[i].z));
        }
    }

    // Start is called before the first frame update
    public void Initialize(Hashtable coordinates)
    {
        this.coordinates = coordinates;
    }
}
