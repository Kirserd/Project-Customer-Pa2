using UnityEngine;
using UnityEngine.UI;

public class GaugeVisualizer : MonoBehaviour
{
    [SerializeField] private float minValue = 0f;
    [SerializeField] private float maxValue = 1f;
    [SerializeField] private float value = 0.5f;
    [SerializeField] private Image barLevel;
    [SerializeField] [ColorUsage(true)] private Color minColor = Color.red;
    [SerializeField] [ColorUsage(true)] private Color midColor = Color.yellow;
    [SerializeField] [ColorUsage(true)] private Color maxColor = Color.green;

    public float Value
    {
        get => value;
        set
        {
            if (value < minValue)
                value = minValue;
            else if (value > maxValue)
                value = maxValue;

            this.value = value;
            UpdateBar();
        }
    }

    private void Start() => UpdateBar();

    public void UpdateBar()
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        float normalizedValue = (value - minValue) / (maxValue - minValue);

        if (barLevel != null)
            barLevel.rectTransform.localScale = new Vector3(normalizedValue, 1, 1);

        Color lerpedColor;
        if (normalizedValue <= 0.5f)
            lerpedColor = Color.Lerp(minColor, midColor, normalizedValue * 2);
        else
            lerpedColor = Color.Lerp(midColor, maxColor, (normalizedValue - 0.5f) * 2);

        if (barLevel != null)
            barLevel.color = lerpedColor;
    }
}