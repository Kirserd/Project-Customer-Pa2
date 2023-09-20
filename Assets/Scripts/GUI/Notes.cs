using UnityEngine;
using TMPro;

public class Notes : MonoBehaviour
{
    [SerializeField]
    private GaugeVisualizer _childHappiness;
    [SerializeField]
    private GaugeVisualizer _choreMeter;
    [SerializeField]
    private GaugeVisualizer _stress;

    [SerializeField]
    private TextMeshProUGUI _childPercentage;
    [SerializeField]
    private TextMeshProUGUI _chorePercentage;
    [SerializeField]
    private TextMeshProUGUI _stressPercentage;

    public void UpdateAll()
    {
        _childHappiness.Value = PointManager.ChildPoints;
        _choreMeter.Value = PointManager.ChorePoints;
        _stress.Value = PointManager.StressPoints;

        _childPercentage.text = Mathf.Round(_childHappiness.Value) + "%";
        _chorePercentage.text = Mathf.Round(_choreMeter.Value) + "%";
        _stressPercentage.text = Mathf.Round(_stress.Value) + "%";
    }
}
