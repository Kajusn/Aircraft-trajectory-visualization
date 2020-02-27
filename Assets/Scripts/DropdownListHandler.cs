using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DropdownListHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject flightText;

    [SerializeField]
    private GameObject dataManager;

    private DataManager dm;
    private Dropdown dropdown;

    void Start()
    {
        dm = dataManager.GetComponent<DataManager>();
        dropdown = GetComponent<Dropdown>();
        Populate(dm.keys);
    }
    public void Dropdown_IndexChanged(int index)
    {
        flightText.GetComponent<Text>().text = dropdown.options[index].text;
    }

    // Populate dropdown list
    public void Populate(List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
}
