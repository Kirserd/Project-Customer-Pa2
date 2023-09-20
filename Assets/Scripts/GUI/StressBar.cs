using System.Collections;
using UnityEngine;

public class StressBar : MonoBehaviour
{
    private static GaugeVisualizer _stressMeter;

    private void Start()
    {
        _stressMeter = GetComponent<GaugeVisualizer>();
        StartCoroutine(UpdateStressMeter());
    }
    public IEnumerator UpdateStressMeter()
    {
        while (true) 
        { 
            yield return new WaitForSeconds(0.5f);
            _stressMeter.Value = PointManager.StressPoints;
        } 
    }
    private void Update()
    {
        PointManager.StressPoints -= 0.1f * Time.deltaTime;
    }
}