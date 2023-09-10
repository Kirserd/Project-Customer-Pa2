using System.Collections.Generic;
public class WateringTask : Task
{
    private readonly Dictionary<Plant, bool> _wateringStatus = new();
    public void SubscribePlant(Plant plant) => _wateringStatus.Add(plant, false);
    public void UpdateStatus(Plant plant, bool state)
    {
        _wateringStatus[plant] = state;

        bool _taskDone = true;
        foreach (Plant status in _wateringStatus.Keys)
            if (!_wateringStatus[status])
                _taskDone = false;

        if (_taskDone)
            Stop(_caller, true);
    }
}
