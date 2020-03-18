using System;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject aircraft;

    [SerializeField]
    private Text speedText;

    private AircraftController ac;
    private Slider slider;


    void Start()
    {
        ac = aircraft.GetComponent<AircraftController>();
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChanged(); });
    }
    public void ValueChanged()
    {
        ac.speed = 0.07f * slider.value;
        speedText.text = Math.Round(slider.value, 1) + "x";
    }
}
