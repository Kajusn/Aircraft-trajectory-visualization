using System.Collections.Generic;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [SerializeField]
    private GameObject dataManager;

    [SerializeField]
    [Range(0.05f, 1.5f)]
    private float speed = 0.07f;    // Around 250 km/h

    private DataManager dm;
    private List<Coordinates> coordinates;
    private int nextPosition = 0;
    private float smoothSpeed;

    void Awake()
    {
        dm = dataManager.GetComponent<DataManager>();
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
            ResetPosition();
        }

        // Position movement
        Vector3 newPosition = new Vector3((float)coordinates[nextPosition].x,
                                          (float)coordinates[nextPosition].z,
                                          (float)coordinates[nextPosition].y);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);

        // Rotation movement
        smoothSpeed = 7 * speed;
        var targetRotation = Quaternion.LookRotation(-(newPosition - transform.position));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

        // If aircraft is less than 50 meters from the next position
        // move towards next position
        if (Vector3.Distance(transform.position, newPosition) < 0.05)
            nextPosition++;
    }

    // Used to start flight simulation
    public void StartFlight(string flight)
    {
        this.coordinates = (List<Coordinates>)dm.coordinatesList[flight];
        ResetPosition();
    }

    // Sets aircraft position to initial position in coordinates list
    private void ResetPosition()
    {
        this.nextPosition = 0;
        Vector3 newPosition = new Vector3((float)coordinates[nextPosition].x,
                                          (float)coordinates[nextPosition].z,
                                          (float)coordinates[nextPosition].y);
        Vector3 lookAt = new Vector3((float)coordinates[nextPosition + 1].x,
                                     (float)coordinates[nextPosition + 1].z,
                                     (float)coordinates[nextPosition + 1].y);
        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(-(lookAt - transform.position));
    }
}
