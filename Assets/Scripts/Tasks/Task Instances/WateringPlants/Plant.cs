﻿using System.Collections;
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
            if(value > MAX)
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
        } 
    }
    private float _waterGauge = 0f;
    public bool IsLocked { get; private set; }
    public float FulfillSpeed { get; private set; } = 0.5f; 
    public const float MAX = 100f;
    public const float MIN = 0f;
    public const float PREDICATE = 95f;
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
        _waterGaugeVisualizer = gauge.GetComponent<GaugeVisualizer>();
        StartCoroutine(PositionGUI(gauge));
    }
    private IEnumerator PositionGUI(GameObject gauge)
    {
        yield return new WaitForEndOfFrame();
        gauge.transform.SetParent(Task.TaskGUI);
        gauge.transform.position = MouseManager.Instance.GameCamera.WorldToScreenPoint(transform.position);
    }
    private void SubscribePlant() => _task.SubscribePlant(this);
    public void Water() => WaterGauge += FulfillSpeed;
}