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
        { TaskID.WateringPlants2, new WateringPlantsTask() },
        { TaskID.DoodleJump2, new DoodleJumpTask() },
        { TaskID.ToyBox2, new ToyTask() },
        { TaskID.DrawingGame2, new DrawingGameTask() },
        { TaskID.WateringPlants3, new WateringPlantsTask() },
        { TaskID.DoodleJump3, new DoodleJumpTask() },
        { TaskID.ToyBox3, new ToyTask() },
        { TaskID.DrawingGame3, new DrawingGameTask() },
    };
    public static Task GetTaskByID(TaskID id)
    {
        if(_tasks.ContainsKey(id))
            return _tasks[id];
        return null;
    }
}
