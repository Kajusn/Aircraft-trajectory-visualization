using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private Text angleText;

    void Start()
    {
        InvokeRepeating("UpdateAngle", 0.2f, 0.3f);
    }
    public void StartSimulation()
    {
        GetComponent<AircraftManager>().Initialize();
        GetComponent<CameraManager>().SwitchAircraftCamera();
    }

    public void ChangeFlight(string flight)
    {
        GetComponent<AircraftManager>().StartFlight(flight);
    }

    void UpdateAngle()
    {
        angleText.text = GetComponent<AircraftManager>().angle.ToString();
    }
}
