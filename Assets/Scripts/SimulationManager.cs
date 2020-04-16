using System;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    private AircraftManager acManager;

    [SerializeField]
    private Dropdown dropdown;

    void Start()
    {
        acManager = GetComponent<AircraftManager>();
        StartSimulation();
    }
    public void StartSimulation()
    {
        acManager.Initialize(dropdown.captionText.text);
        GetComponent<CameraManager>().SwitchAircraftCamera();
    }

    // Changes flight
    public void ChangeFlight(string flight)
    {
        GetComponent<AircraftManager>().StartFlight(flight);
    }
}
