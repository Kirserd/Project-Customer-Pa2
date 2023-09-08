using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Plant : MonoBehaviour 
{
    public delegate void OnGaugeFilledHandler();
    public event OnGaugeFilledHandler OnGaugeFilled;
    public float WaterGauge 
    { 
        get => _waterGauge; 
        private set
        {
            if(value < 0) 
            {
                _waterGauge = 0;
                return;
            }
            _waterGauge = value;
        } 
    }
    private float _waterGauge = 50f;
    public bool IsLocked { get; private set; }
    public float FulfillSpeed { get; private set; } = 5f; 
    public const float MAX = 100f;
    public const float MIN = 0f;
    public void Water()
    {
        WaterGauge += Time.fixedDeltaTime * FulfillSpeed;
        Debug.Log(gameObject.name + " Watered " + WaterGauge);
    }
    private void FixedUpdate() => WaterGauge -= Time.fixedDeltaTime * (FulfillSpeed / 10);
}