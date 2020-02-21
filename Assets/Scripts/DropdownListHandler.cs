using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DropdownListHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject trajectoryRenderer;

    [SerializeField]
    private GameObject aircraft;

    [SerializeField]
    private GameObject flightText;

    private Dropdown dropdown;
    private AircraftController ac;
    private TrajectoryMapper tm;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        ac = aircraft.GetComponent<AircraftController>();
        tm = trajectoryRenderer.GetComponent<TrajectoryMapper>();
    }
    public void Dropdown_IndexChanged(int index)
    {
        flightText.GetComponent<Text>().text = dropdown.options[index].text;

        // Calls method to render selected flight trajectory
        tm.RenderTrajectory(dropdown.options[index].text);

        // Calls method to simulate selected flight
        ac.StartFlight(dropdown.options[index].text);
    }

    public void Populate(Hashtable table)
    {
        // Get all available flights
        var options = table.Keys.Cast<string>().ToList();
        dropdown.ClearOptions();
        dropdown.GetComponent<Dropdown>().AddOptions(options);
    }
}
