using UnityEngine;

public class Trajectory_Toggle : MonoBehaviour
{
    [SerializeField]
    private GameObject trajectoryRenderer;

    public void ToggleBehavior(bool toggle)
    {
        trajectoryRenderer.SetActive(toggle);
    }
}
