using System.Collections.Generic;

public static class TaskArchive
{
    private static readonly Dictionary<TaskID, Task> _tasks = new()
    {
        {TaskID.WateringPlants, new WateringPlantsTask()},
        {TaskID.DoodleJump, new DoodleJumpTask()},
        {TaskID.Television, new EmptyTask()},
        {TaskID.ToyBox, new ToyTask()},
        {TaskID.DrawingGame, new DrawingGameTask()},
    };
    public static Task GetTaskByID(TaskID id)
    {
        if(_tasks.ContainsKey(id))
            return _tasks[id];
        return null;
    }
}

