using System.Collections.Generic;
using UnityEngine;

public static class PointManager
{
    private static float _chorePoints;
    private static float _childPoints;
    private static float _stressPoints;

    public static float ChorePoints
    {
        get => _chorePoints;
        set
        {
            if (value >= 100)
                _chorePoints = 100;
            else if (value <= 0)
                _chorePoints = 0;
            else
                _chorePoints = value;
        }
    }
    public static float ChildPoints
    {
        get => _childPoints;
        set
        {
            if (value >= 100)
                _childPoints = 100;
            else if (value <= 0)
                _childPoints = 0;
            else
                _childPoints = value;
        }
    }
    public static float StressPoints
    {
        get => _stressPoints;
        set
        {
            _stressPoints = value;
            if (_stressPoints >= 100)
            {
                if (Dad.PlayerStateMachine.CurrentState is not MovingState)
                    return;

                _stressPoints = 0;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Dad>().Shout();
                ChildPoints -= ChildPoints / 3f;
            }
        }
    }
    public static float AllStress;
    public static float StressIterations;

    public static List<(TaskID, bool)> CompletionOrder = new();

    public static void Start()
    {
        _childPoints = 0;
        _chorePoints = 0;
        _stressPoints = 0;
        CompletionOrder.Clear();
        DayCycle.ResetTime();
    }

    public static void FinalizeGame(DayCycle.TimeInterval interval)
    {
        if (interval != DayCycle.TimeInterval.Night)
            return;
        Debug.Log("Finalization...");
        DayCycle.StopCount();
        CalculateCoordinate();
    }

    public static Vector2 CalculateCoordinate()
    {
        Vector2 result = Vector2.zero;
        float yUnMapped = _chorePoints;
        result.y = (Remap(yUnMapped, 0, 100) - 0.5f) * 2f;

        float avgStress = AllStress / StressIterations;

        float xUnMapped = _childPoints - avgStress / 2f;
        result.x = (Remap(xUnMapped, 0, 100) - 0.5f) * 1.6f + GetAvgChildPro() * 0.2f;

        Debug.Log("avgStress: " + avgStress);
        Debug.Log("rawChild: " + Mathf.Clamp01(_childPoints));
        Debug.Log("avgChildPrio: " + GetAvgChildPro());
        Debug.Log("chores: " + result.y);
        Debug.Log("child: " + result.x);

        return result;

        float Remap(float value, float minValue, float maxValue)
        {
            return Mathf.Clamp01((value - minValue) / (maxValue - minValue));
        }
        float GetAvgChildPro()
        {
            float avgPrio = 0;
            for (int i = 0; i < CompletionOrder.Count; i++)
            {
                if (CompletionOrder[i].Item2) 
                    avgPrio += 1f / ((i+1) * 0.5f);
                else 
                    avgPrio -= 1f / ((i+1) * 0.5f);
            }
            return avgPrio;
        }
    }
}