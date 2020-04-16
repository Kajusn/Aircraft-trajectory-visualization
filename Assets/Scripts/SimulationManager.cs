using System;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    private AircraftManager acManager;

    void Start()
    {
        acManager = GetComponent<AircraftManager>();
        StartSimulation();
    }
    public void StartSimulation()
    {
        acManager.Initialize();
        GetComponent<CameraManager>().SwitchAircraftCamera();
    }

    // Changes flight
    public void ChangeFlight(string flight)
    {
        GetComponent<AircraftManager>().StartFlight(flight);
    }
}
