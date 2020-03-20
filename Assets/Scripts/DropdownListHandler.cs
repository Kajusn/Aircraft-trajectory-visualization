using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownListHandler : MonoBehaviour
{
    [SerializeField]
    private SimulationManager simulationManager;

    [SerializeField]
    private GameObject flightText;

    [SerializeField]
    private DataManager dataManager;

    private Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        Populate(dataManager.keys);
    }
    public void Dropdown_IndexChanged(int index)
    {
        flightText.GetComponent<Text>().text = dropdown.options[index].text;
        dropdown.captionText.text = dropdown.options[index].text;
        simulationManager.ChangeFlight(dropdown.captionText.text);
    }

    // Populate dropdown list
    public void Populate(List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
}
