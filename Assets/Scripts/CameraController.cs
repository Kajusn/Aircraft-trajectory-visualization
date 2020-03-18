using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera terrainCamera;

    [SerializeField]
    private Camera followCamera;
    void Start()
    {
        terrainCamera.enabled = true;
        followCamera.enabled = false;
    }

    public void SwitchFollow()
    {
        if (followCamera.enabled == false)
        {
            followCamera.enabled = true;
            terrainCamera.enabled = false;
        }
    }
    public void SwitchTerrain()
    {
        if (terrainCamera.enabled == false)
        {
            terrainCamera.enabled = true;
            followCamera.enabled = false;
        }
    }
}
