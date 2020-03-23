using System;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Handler : MonoBehaviour
{
    [SerializeField]
    private SimulationManager simulationManager;

    [SerializeField]
    private Text speedText;

    private Slider slider;


    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChanged(); });
    }
    public void ValueChanged()
    {
        simulationManager.GetComponent<AircraftManager>().MultiplySpeed(slider.value);
        speedText.text = Math.Round(slider.value, 1) + "x";
    }
}
