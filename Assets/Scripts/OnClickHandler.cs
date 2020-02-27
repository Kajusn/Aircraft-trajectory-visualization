using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class OnClickHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject trajectoryRenderer;

    [SerializeField]
    private GameObject aircraft;

    [SerializeField]
    private GameObject optionText;

    private TrajectoryMapper tm;
    private AircraftController ac;
    private Text option;

    void Start()
    {
        tm = trajectoryRenderer.GetComponent<TrajectoryMapper>();
        ac = aircraft.GetComponent<AircraftController>();
        option = optionText.GetComponent<Text>();
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (option.text.Length > 10)
        {
            // Initialize aircraft and trajectory rendering
            tm.Initialize();
            ac.Initialize();
        }
        else
        {
            tm.RenderTrajectory(option.text);
            ac.StartFlight(option.text);
        }
    }
}
