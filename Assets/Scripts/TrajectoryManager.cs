using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    [SerializeField]
    private Tube prefab;

    [SerializeField]
    private DataManager dataManager;

    private GameObject landingProcedure;

    private Transform[] tubes;

    private Aircraft aircraft;

    void Start()
    {
        RenderProcedure();
        aircraft = GetComponent<AircraftManager>().aircraft;
        //aircraft.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
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
            Tube tube = Instantiate(prefab);
            tube.Settings(Vector3.Distance(current, next), current, next);
            tube.CreateTube(false);
            tube.transform.SetParent(landingProcedure.transform);
        }
        tubes = landingProcedure.GetComponentsInChildren<Transform>();
    }

    // Checks if aircraft is following landing procedure guidelines
    void CheckTrajectory()
    {
        foreach (Transform tubePart in tubes)
        {
            tubePart.gameObject.SetActive(false);
        }
    }
}
