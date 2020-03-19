using System.Collections.Generic;
using UnityEngine;

public class LandingProcedureRenderer : MonoBehaviour
{
    [SerializeField]
    private Tube prefab;

    [SerializeField]
    private GameObject dataManager;

    DataManager dm;

    void Start()
    {
        dm = dataManager.GetComponent<DataManager>();
        RenderProcedure();
    }

    // Renders landing procedure
    public void RenderProcedure()
    {
        foreach (var key in dm.approachKeys)
        {
            List<Coordinates> list = (List<Coordinates>)dm.approach[key];
            for (int i = 0; i < list.Count-1; i++)
            {
                Vector3 current = new Vector3((float)list[i].x,
                                              (float)list[i].z,
                                              (float)list[i].y);
                Vector3 next = new Vector3((float)list[i+1].x,
                                           (float)list[i+1].z,
                                           (float)list[i+1].y);
                Tube tube = Instantiate(prefab);
                tube.Settings(Vector3.Distance(current, next), current, next);
                if (key == "final")
                {
                    tube.CreateTube(true);
                }
                else
                {
                    tube.CreateTube(false);
                }
            }
        }
    }

    /*void OnDrawGizmos()
    {
        foreach (var key in dm.approachKeys)
        {
            List<Coordinates> list = (List<Coordinates>)dm.approach[key];
            for (int i = 0; i < list.Count; i++)
            {
                Vector3 current = new Vector3((float)list[i].x,
                                              (float)list[i].z,
                                              (float)list[i].y);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(current, 0.1f);
            }
        }
    }*/
}
