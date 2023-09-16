using System.Collections.Generic;

public static class TaskArchive
{
    private static readonly Dictionary<TaskID, Task> _tasks = new()
    {
        {TaskID.WateringPlants, new WateringPlantsTask()},
        {TaskID.DoodleJump, new DoodleJumpTask()},
        {TaskID.Television, new TelevisionTask()},
    };
    public static Task GetTaskByID(TaskID id)
    {
        if(_tasks.ContainsKey(id))
            return _tasks[id];
        return null;
    }
}
