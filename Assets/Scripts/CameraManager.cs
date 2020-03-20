using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera idleCamera;
    [SerializeField]
    private Camera aircraftCamera;
    // Start is called before the first frame update
    void Start()
    {
        idleCamera.enabled = true;
        aircraftCamera.enabled = false;
    }

    // Switch to aircraft camera
    public void SwitchAircraftCamera()
    {
        if (aircraftCamera.enabled == false)
        {
            aircraftCamera.enabled = true;
            idleCamera.enabled = false;
        }
    }
    // Switch to idle camera
    public void SwitchIdleCamera()
    {
        if (idleCamera.enabled == false)
        {
            idleCamera.enabled = true;
            aircraftCamera.enabled = false;
        }
    }
}
