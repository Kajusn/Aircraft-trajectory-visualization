using UnityEngine.UI;
using UnityEngine;

public class OnClickHandler : MonoBehaviour
{
    [SerializeField]
    private SimulationManager simulationManager;

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(simulationManager.StartSimulation);
    }
}
