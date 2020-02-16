using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DropdownListHandler : MonoBehaviour
{
    private List<string> options;
    private Dropdown dropdown;
    private Text selectedItem;

    public void Dropdown_IndexChanged(int index)
    {
        selectedItem.text = options[index];
        GameObject tr = GameObject.Find("TrajectoryRenderer");
        var tm = tr.GetComponent<TrajectoryMapper>();
        tm.RenderTrajectory(selectedItem.text);
    }

    public void Populate(Hashtable table)
    {
        dropdown = gameObject.GetComponent<Dropdown>();
        // Get all available flights
        options = table.Keys.Cast<string>().ToList();
        dropdown.ClearOptions();
        dropdown.GetComponent<Dropdown>().AddOptions(options);
    }

    void Start()
    {
        var text = GameObject.Find("Flight_Text");
        this.selectedItem = text.GetComponent<Text>();
    }
}
