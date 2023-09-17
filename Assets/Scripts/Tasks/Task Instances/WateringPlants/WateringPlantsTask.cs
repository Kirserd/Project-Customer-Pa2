using System.Collections.Generic;
public class WateringPlantsTask : Task
{
    private readonly Dictionary<Plant, bool> _wateringStatus = new();
    public void SubscribePlant(Plant plant) => _wateringStatus.Add(plant, false);
    public void UpdateStatus(Plant plant, bool state)
    {
        _wateringStatus[plant] = state;

        bool taskDone = true;
        foreach (Plant status in _wateringStatus.Keys)
            if (!_wateringStatus[status])
                taskDone = false;

        if (taskDone)
            Stop(_caller, TaskStarter.Availability.Done);
    }
}
