using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftManager : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;

    [SerializeField]
    private Aircraft aircraftModel;

    [SerializeField]
    private CameraFollow aircraftCamera;

    private float speed = 0.07f;    // Around 250 km/h

    private List<Coordinates> coordinates;
    private int nextPosition = 0;
    private float smoothSpeed;
    private Aircraft aircraft;
    private LineRenderer trail;

    private string defaultFlight = "ELG1337";

    void Awake()
    {
        aircraft = Instantiate<Aircraft>(aircraftModel);
        aircraft.transform.SetParent(transform);
        trail = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // No flight simulation if there is no aircraft prefab
        if (aircraft == null)
            return;
        // No flight simulation if there are no coordinates
        if (coordinates == null)
            return;
        // Stop flight simulation if all coordinates have been visited
        if (nextPosition == coordinates.Count)
        {
            ResetPosition();
            this.coordinates = null;
            return;
        }

        // Move aircraft to new position
        Vector3 newPosition = new Vector3((float)coordinates[nextPosition].x,
                                          (float)coordinates[nextPosition].z,
                                          (float)coordinates[nextPosition].y);
        aircraft.transform.position = Vector3.MoveTowards(aircraft.transform.position, newPosition, Time.deltaTime * speed);
        trail.positionCount++;
        trail.SetPosition(trail.positionCount - 1, aircraft.transform.position);

        // Rotation movement
        smoothSpeed = 7 * speed;
        var targetRotation = Quaternion.LookRotation(-(newPosition - aircraft.transform.position) + new Vector3(0f, 90.0f, 0f));
        aircraft.transform.rotation = Quaternion.Slerp(aircraft.transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

        // If aircraft is less than 50 meters from the next position
        // move towards next position
        if (Vector3.Distance(aircraft.transform.position, newPosition) < 0.05)
            nextPosition++;
    }

    public void MultiplySpeed(float times)
    {
        this.speed = 0.07f * times;
    }
    // Used to start flight simulation
    public void StartFlight(string flight)
    {
        this.coordinates = (List<Coordinates>)dataManager.coordinatesList[flight];
        ResetPosition();
    }

    // Sets aircraft position to initial position in coordinates list
    private void ResetPosition()
    {
        this.nextPosition = 0;
        trail.positionCount = 1;
        Vector3 newPosition = new Vector3((float)coordinates[nextPosition].x,
                                          (float)coordinates[nextPosition].z,
                                          (float)coordinates[nextPosition].y);
        Vector3 lookAt = new Vector3((float)coordinates[nextPosition + 1].x,
                                     (float)coordinates[nextPosition + 1].z,
                                     (float)coordinates[nextPosition + 1].y);
        aircraft.transform.position = newPosition;
        aircraft.transform.rotation = Quaternion.LookRotation(-(lookAt - aircraft.transform.position) + new Vector3(0f, 90.0f, 0f));
        trail.SetPosition(0, aircraft.transform.position);
    }

    public void Initialize()
    {
        aircraft.CreateAircraft(0.03f, 0.015f, 0f);
        aircraftCamera.SetTarget(aircraft.transform);
        StartFlight(defaultFlight);
    }
}
