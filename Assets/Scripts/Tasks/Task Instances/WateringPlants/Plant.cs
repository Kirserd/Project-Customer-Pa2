using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Plant : MonoBehaviour 
{
    public static WateringPlantsTask _task;
    private bool _watered = false;

    [SerializeReference]
    private GameObject _wateringGaugePrefab;
    private GaugeVisualizer _waterGaugeVisualizer;

    public float WaterGauge 
    { 
        get => _waterGauge; 
        private set
        {
            if(value < 0) 
            {
                _task.ForcefullyStop(false);
                _waterGauge = 0;
                return;
            }
            else if(value > MAX)
            {
                _waterGauge = MAX;
                return;
            }
            _waterGauge = value;
            _waterGaugeVisualizer.Value = value;

            if (!_watered && _waterGauge >= PREDICATE)
            {
                _task.UpdateStatus(this, true);
                _watered = true;
            }
            else if (_watered && _waterGauge < PREDICATE)
            {
                _task.UpdateStatus(this, false);
                _watered = false;
            }
        } 
    }
    private float _waterGauge = 50f;
    public bool IsLocked { get; private set; }
    public float FulfillSpeed { get; private set; } = 0.5f; 
    public const float MAX = 100f;
    public const float MIN = 0f;
    public const float PREDICATE = 80f;
    public void Start()
    {
        ValidateTask();
        SetupGUI();
        SubscribePlant();
    }
    private void ValidateTask()
    {
        if (_task is not null)
            return;

        if (Task.Instance is WateringPlantsTask task)
            _task = task;
        else
            Debug.LogException(new System.Exception("Not settled task error"));
    }
    private void SetupGUI()
    {
        GameObject gauge = Instantiate(_wateringGaugePrefab);

        gauge.transform.SetParent(Task.TaskGUI);
        gauge.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        _waterGaugeVisualizer = gauge.GetComponent<GaugeVisualizer>();
    }
    private void SubscribePlant() => _task.SubscribePlant(this);
    public void Water() => WaterGauge += FulfillSpeed;
    private void FixedUpdate() => WaterGauge -= FulfillSpeed / 10;
}