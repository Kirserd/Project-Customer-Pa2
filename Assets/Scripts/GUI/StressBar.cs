using System.Collections;
using UnityEngine;

public class StressBar : MonoBehaviour
{
    private static GaugeVisualizer _stressMeter;

    private void Start()
    {
        DayCycle.OnTimeIntervalChanged += PointManager.FinalizeGame;
        _stressMeter = GetComponent<GaugeVisualizer>();
        StartCoroutine(UpdateStressMeter());
    }
    public IEnumerator UpdateStressMeter()
    {
        while (true) 
        { 
            yield return new WaitForSeconds(0.5f);
            PointManager.StressPoints -= 0.1f;
            PointManager.AllStress += PointManager.StressPoints / 10f;
            PointManager.StressIterations += 0.05f;
            _stressMeter.Value = PointManager.StressPoints;
        } 
    }
}