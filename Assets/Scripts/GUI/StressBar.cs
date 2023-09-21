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
        StartCoroutine(ChildSounds());
        StartCoroutine(DadSounds());
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
    public IEnumerator ChildSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            if(Random.Range(0,5) > 3)
            {
                if (PointManager.ChildPoints < 33) AudioManager.Source.PlayOneShot(AudioManager.Clips["ChildSad"], 0.5f);
                else if (PointManager.ChildPoints < 66) AudioManager.Source.PlayOneShot(AudioManager.Clips["ChildNeutral"], 0.5f);
                else AudioManager.Source.PlayOneShot(AudioManager.Clips["ChildHappy"], 0.5f);
            }
        }
    }
    public IEnumerator DadSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f);
            if (Random.Range(0, 5) > 3)
            {
                if (PointManager.StressPoints < 50) AudioManager.Source.PlayOneShot(AudioManager.Clips["Calm"], 0.5f);
                else AudioManager.Source.PlayOneShot(AudioManager.Clips["Stressed"], 0.5f);
            }
        }
    }
}