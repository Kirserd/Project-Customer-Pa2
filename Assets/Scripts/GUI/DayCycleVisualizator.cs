using UnityEngine;
using TMPro;

public class DayCycleVisualizator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private TextMeshProUGUI _timeTypeText;
    [SerializeField]
    private TextMeshProUGUI _intervalText;

    private void Start()
    {
        DayCycle.OnTimeChanged += RefreshTime;
        DayCycle.OnTimeIntervalChanged += RefreshInterval;
    }
    private void RefreshTime(float hour, float minute)
    {
        bool IsAM = hour < 13;
        bool IsLong = hour < 10;
        _timeText.text = (IsAM? hour : hour - 12) + ":" + (minute < 10 ? "0" + minute : minute);
        _timeTypeText.text = (IsLong? "" : " ") + (IsAM? "AM" : "PM");
    }
    private void RefreshInterval(DayCycle.TimeInterval interval) => _intervalText.text = interval.ToString();
}
