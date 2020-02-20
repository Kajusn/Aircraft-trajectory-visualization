using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    private DataManager dm;
    private Rigidbody aircraft;
    private List<Coordinates> coordinates;
    private int nextPosition = 0;
    [SerializeField]
    private float speed = 5;
    
    // Start is called before the first frame update
    void Awake()
    {
        dm = GameObject.Find("ReadFile_Btn").GetComponent<DataManager>();
        aircraft = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // No flight simulation if there are no coordinates
        if (coordinates == null)
            return;
        // Stop flight simulation if all coordinates have been visited
        if (nextPosition == coordinates.Count)
        {
            //return;
            nextPosition = 0;
            transform.position = new Vector3((float)coordinates[nextPosition].x,
                                             (float)coordinates[nextPosition].z,
                                             (float)coordinates[nextPosition].y);
        }

        var newPosition = new Vector3((float)coordinates[nextPosition].x,
                                      (float)coordinates[nextPosition].z,
                                      (float)coordinates[nextPosition].y);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, newPosition) < 0.3)
            nextPosition++;
    }

    public void StartFlight(string flight)
    {
        this.coordinates = (List<Coordinates>)dm.CoordinatesList[flight];
        this.nextPosition = 0;
        transform.position = new Vector3((float)coordinates[nextPosition].x,
                                         (float)coordinates[nextPosition].z,
                                         (float)coordinates[nextPosition].y);
    }
}
