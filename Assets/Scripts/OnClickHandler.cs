using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OnClickHandler : MonoBehaviour
{
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        DataManager dm = gameObject.GetComponent<DataManager>();
        dm.GetComponent<DataManager>().ReadFlightsFromFile();
    }
}
