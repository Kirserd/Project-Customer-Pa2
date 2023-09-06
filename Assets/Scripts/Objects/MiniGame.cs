public abstract class MiniGame
{
    protected bool IsStarted;
    public abstract void Start(MinigameStarter caller);
    public abstract void Stop(MinigameStarter caller);
    public abstract void ForcefullyStop();
}