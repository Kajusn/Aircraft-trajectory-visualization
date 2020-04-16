using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrajectoryManager : MonoBehaviour
{
    [SerializeField]
    private Tube prefab;

    [SerializeField]
    private Material curtainMaterial;

    [SerializeField]
    private DataManager dataManager;

    private GameObject landingProcedure;

    [SerializeField]
    private Text heightText;

    [SerializeField]
    private Text angleText;

    [SerializeField]
    private Image angleAlert;

    private AircraftManager acManager;

    void Start()
    {
        acManager = GetComponent<AircraftManager>();
        angleAlert.enabled = false;
        RenderProcedure();
        InvokeRepeating("UpdateAngleAltitude", 0.2f, 0.3f);
    }

    // Renders landing procedure
    public void RenderProcedure()
    {
        landingProcedure = new GameObject();
        landingProcedure.transform.SetParent(transform);
        landingProcedure.name = "ILS";
        List<Coordinates> list = dataManager.ils;
        for (int i = 0; i < list.Count - 1; i++)
        {
            Vector3 current = new Vector3((float)list[i].x,
                                            (float)list[i].z,
                                            (float)list[i].y);
            Vector3 next = new Vector3((float)list[i + 1].x,
                                        (float)list[i + 1].z,
                                        (float)list[i + 1].y);
            // Creates a tube object connecting two coordinates
            Tube tube = Instantiate(prefab);
            tube.Settings(Vector3.Distance(current, next), current, next);
            tube.CreateTube(false);
            tube.transform.SetParent(landingProcedure.transform);

            if (i == 0)
                continue;
            // Creates curtain under tube object for better 3D visualization
            GameObject curtain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            curtain.GetComponent<MeshRenderer>().material = curtainMaterial;
            Vector3 curtainOffset = new Vector3(0, tube.transform.position.y / 2 + tube.radius, 0);
            curtain.transform.position = Vector3.Lerp(current, next, 0.5f) - curtainOffset;
            curtain.transform.rotation = tube.transform.rotation;
            curtain.transform.localScale = new Vector3(0.01f, Vector3.Distance(current, next), tube.transform.position.y);
            curtain.transform.SetParent(tube.transform);
        }
    }

    // Updates angle and altitute values, alerts if angle is not within allowed limit
    void UpdateAngleAltitude()
    {
        angleAlert.enabled = false;
        int altitude = (int)Mathf.Round(acManager.aircraft.transform.position.y * 3280.8f) - 400; // Convert Km to Ft
        double angle = Math.Round(acManager.angle * 1.11f, 1);  // Angle calculated in percentage of 90 degrees
        if (altitude <= 4200 && altitude >= 1800)
        {
            if (angle >= 6.6f || angle <= 4.6f)
            {
                angleAlert.enabled = true;
            }
        }
        angleText.text = angle.ToString() + "%";
        heightText.text = altitude.ToString();
    }
}
