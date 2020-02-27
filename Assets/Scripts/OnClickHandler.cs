using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class OnClickHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject trajectoryRenderer;

    [SerializeField]
    private GameObject aircraft;

    private TrajectoryMapper tm;
    private AircraftController ac;

    void Start()
    {
        tm = trajectoryRenderer.GetComponent<TrajectoryMapper>();
        ac = aircraft.GetComponent<AircraftController>();
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // Initialize trajectory renderer
        tm.Initialize();
    }
}
