﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownListHandler : MonoBehaviour
{
    [SerializeField]
    private SimulationManager simulationManager;

    [SerializeField]
    private DataManager dataManager;

    private Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        Populate(dataManager.keys);
        dropdown.value = dropdown.options.IndexOf(dropdown.options[0]);
    }
    public void Dropdown_IndexChanged(int index)
    {
        simulationManager.ChangeFlight(dropdown.captionText.text);
    }

    // Populate dropdown list
    public void Populate(List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
}
