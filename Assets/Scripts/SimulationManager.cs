using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    [SerializeField]
    private Text heightText;

    [SerializeField]
    private Text angleText;

    [SerializeField]
    private Text angleAlert;

    private AircraftManager acManager;

    void Start()
    {
        InvokeRepeating("UpdateAngleAltitude", 0.2f, 0.3f);
        acManager = GetComponent<AircraftManager>();
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

    void UpdateAngleAltitude()
    {
        angleAlert.enabled = false;
        float altitude = Mathf.Round(acManager.aircraft.transform.position.y * 3280.8f)-400; // Convert Km to Ft
        float angle = acManager.angle;
        if (altitude <= 4200 && altitude >= 1800)
        {
            if (angle >= 6.6f || angle <= 4.6f)
                angleAlert.enabled = true;
        }
        angleText.text = angle.ToString()+"%";
        heightText.text = altitude.ToString();
    }
}
