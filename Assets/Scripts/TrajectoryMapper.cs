using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryMapper : MonoBehaviour
{
    [SerializeField]
    private GameObject dataManager;

    LineRenderer lr;
    DataManager dm;
    private string defaultFlight = "ELG1337";

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        dm = dataManager.GetComponent<DataManager>();
    }

    // Populates the line with the coordinates of the flight
    public void RenderTrajectory(string flight)
    {
        lr.transform.Rotate(0, 58, 0, Space.World);
        List<Coordinates> coordinatesList = (List<Coordinates>)dm.coordinatesList[flight];
        lr.positionCount = coordinatesList.Count;
        for (int i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, new Vector3((float)coordinatesList[i].x,
                                          (float)coordinatesList[i].z, 
                                          (float)coordinatesList[i].y));
        }
    }

    // Initialize trajectory renderer
    public void Initialize()
    {
        // Render default flight
        RenderTrajectory(defaultFlight);
    }
}
