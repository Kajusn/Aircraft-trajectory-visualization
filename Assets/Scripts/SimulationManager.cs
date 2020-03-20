using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;

    public void StartSimulation()
    {
        GetComponent<AircraftManager>().Initialize();
        GetComponent<CameraManager>().SwitchAircraftCamera();
    }

    public void ChangeFlight(string flight)
    {
        GetComponent<AircraftManager>().StartFlight(flight);
    }
}
