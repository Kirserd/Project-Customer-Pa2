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
                _stressPoints = 0;
                
                GameObject.FindGameObjectWithTag("Player").GetComponent<Dad>().Shout();
                ChildPoints -= ChildPoints / 3f;
            }
        }
    }
    public static List<TaskID> CompletionOrder = new();

    public static void Start()
    {
        _childPoints = 0;
        _chorePoints = 0;
        _stressPoints = 0;
        CompletionOrder.Clear();
        DayCycle.ResetTime();
    }
}