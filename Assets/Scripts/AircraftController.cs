using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    private DataManager dm;
    private GameObject aircraft;
    private List<Coordinates> coordinates;
    private int nextPosition = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        var dmObj = GameObject.Find("ReadFile_Btn");
        dm = dmObj.GetComponent<DataManager>();
        this.aircraft = GameObject.Find("Aircraft");
    }

    void Update()
    {
        // No flight simulation if there are no coordinates
        if (coordinates == null)
            return;
        // Stop flight simulation if all coordinates have been visited
        if (nextPosition == coordinates.Count)
            //return;
            nextPosition = 0;
        var currentPosition = nextPosition;
        this.aircraft.transform.position = new Vector3( (float)coordinates[currentPosition].x,
                                                        (float)coordinates[currentPosition].z,
                                                        (float)coordinates[currentPosition].y);
        nextPosition++;
    }

    public void StartFlight(string flight)
    {
        this.coordinates = (List<Coordinates>)dm.CoordinatesList[flight];
        this.nextPosition = 0;
    }
}
