using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class OnClickHandler : MonoBehaviour
{
    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        DataManager dm = gameObject.GetComponent<DataManager>();
        Hashtable table = dm.GetComponent<DataManager>().ReadFlightsFromFile();

        // Populates the dropdown list
        GameObject dropdown = GameObject.Find("FlightsList_Dropdown");
        var dpHandler = dropdown.GetComponent<DropdownListHandler>();
        dpHandler.Populate(table);

        // Sends the coordinates table to the trajectory renderer
        GameObject tr = GameObject.Find("TrajectoryRenderer");
        var tm = tr.GetComponent<TrajectoryMapper>();
        tm.Initialize();
    }
}
