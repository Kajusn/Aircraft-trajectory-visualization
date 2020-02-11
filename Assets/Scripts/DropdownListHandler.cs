using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DropdownListHandler : MonoBehaviour
{
    private Dropdown dropdown;

    public void Populate(Hashtable table)
    {
        dropdown = gameObject.GetComponent<Dropdown>();
        // Get all available flights
        List<string> L = table.Keys.Cast<string>().ToList();
        dropdown.ClearOptions();

        var i = 0;
        foreach (string option in L)
        {
            dropdown.options.Add(new Dropdown.OptionData(option));
            dropdown.options[i].text = option;
            i++;
        }
        dropdown.GetComponent<Dropdown>().AddOptions(L);
    }
}
