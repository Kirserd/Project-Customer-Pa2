using UnityEngine;
using TMPro;

public class PhoneTimeVisualizator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeText;

    private void Start() => DayCycle.OnTimeChanged += RefreshTime;
    private void RefreshTime(float hour, float minute)
    {
        bool IsAM = hour < 13;
        _timeText.text = (IsAM ? hour : hour - 12) + ":"
            + (minute < 10 ? "0" + minute : minute) + (IsAM ? "AM" : "PM");
    }
}
