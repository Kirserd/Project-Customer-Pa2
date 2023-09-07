using System.Collections;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public enum TimeInterval
    {
        EARLY_MORNING = 6,
        MID_MORNING = 8,
        LATE_MORNING = 10,
        EARLY_AFTERNOON = 12,
        MID_AFTERNOON = 14,
        LATE_AFTERNOON = 16,
        EARLY_EVENING = 18,
        MID_EVENING = 20,
        LATE_EVENING = 22
    }
    public delegate void OnTimeIntervalChangedHandler(TimeInterval interval);
    public static OnTimeIntervalChangedHandler OnTimeIntervalChanged;

    [Header("Time-related parameters")]
    [SerializeField]
    [Range(1F, 10F)]
    private float _timeMultiplier = 1f;
    [SerializeField]
    [Range(100F, 1000F)]
    private float _hourLength = 100f;
    [SerializeField]
    private float _timer = 0f;

    [Header("Translated time")]
    [SerializeField]
    private float _hour = 8f;
    [SerializeField]
    private float _minute = 0f;
    [SerializeField]
    private TimeInterval _interval = TimeInterval.EARLY_MORNING;
    private TimeInterval _prevInterval = TimeInterval.LATE_EVENING;

    private void Start() => Refresh();
    private void Refresh()
    {
        _timer = _hour * _hourLength;
    }

    private void Update()
    {
        CountTime();
        ValidateInterval();
    }

    private void CountTime()
    {
        _timer += Time.deltaTime * _timeMultiplier;
        _hour = Mathf.Floor(_timer / _hourLength);
        _minute = Mathf.Round((_timer - _hourLength * _hour) / _hourLength * 60);
    }

    private void ValidateInterval()
    {
        if (_interval != _prevInterval)
        {
            UpdateTasks();
            return;
        }
        else
        {
            if (_hour > (int)_interval + 2)
                _interval = (TimeInterval)((int)_interval + 2);
        }
    }

    private void UpdateTasks()
    {
        OnTimeIntervalChanged.Invoke(_interval);
        _prevInterval = _interval;
    }
}
