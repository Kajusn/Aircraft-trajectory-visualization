using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LandingProcedureRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject dataManager;

    LineRenderer lr;
    DataManager dm;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        dm = dataManager.GetComponent<DataManager>();
        //RenderProcedure();
    }

    // Renders landing procedure
    public void RenderProcedure()
    {
        lr.transform.Rotate(0, 58, 0, Space.World);
        foreach (var key in dm.approachKeys)
        {
            List<Coordinates> list = (List<Coordinates>)dm.approach[key];
            lr = new LineRenderer();
            lr.positionCount = list.Count;
            for (int i = 0; i < lr.positionCount; i++)
            {
                lr.SetPosition(i, new Vector3((float)list[i].x,
                                              (float)list[i].z,
                                              (float)list[i].y));
            }
        }
    }
}
