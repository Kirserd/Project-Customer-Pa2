using System.Collections.Generic;

public static class TaskArchive
{
    private static Dictionary<TaskID, Task> _tasks = new()
    {
        {TaskID.WateringPlants, new WateringTask()}
    };
    public static Task GetTaskByID(TaskID id)
    {
        if(_tasks.ContainsKey(id))
            return _tasks[id];
        return null;
    }
}