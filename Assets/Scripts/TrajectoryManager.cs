using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    [SerializeField]
    private Tube prefab;

    [SerializeField]
    private DataManager dataManager;

    void Start()
    {
        RenderProcedure();
    }

    // Renders landing procedure
    public void RenderProcedure()
    {
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
            tube.transform.SetParent(transform);
        }
    }
}
