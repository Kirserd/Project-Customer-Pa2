using UnityEngine;

public class StressBar : MonoBehaviour
{
    private static GaugeVisualizer _stressMeter;

    private void Start()
    {
        _stressMeter = GetComponent<GaugeVisualizer>();
        UpdateStressMeter();
    }
    public static void UpdateStressMeter()
    {
        _stressMeter.Value = PointManager.StressPoints;
    }
}