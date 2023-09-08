
public class WateringTask : Task
{

    public override void ForcefullyStop()
    {
    }

    public override void Start(TaskStarter caller)
    {
        base.Start(caller);
    }

    public override void Stop(TaskStarter caller, bool result)
    {
        base.Stop(caller, result);
    }
}
