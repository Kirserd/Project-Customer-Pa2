public abstract class Task
{
    protected bool IsStarted;
    public abstract void Start(TaskStarter caller);
    public abstract void Stop(TaskStarter caller);
    public abstract void ForcefullyStop();
}