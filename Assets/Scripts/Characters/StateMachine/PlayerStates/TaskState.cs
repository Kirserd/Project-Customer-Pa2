public class TaskState : PlayerState
{
    private TaskData _data;
    public TaskState(Dad player, TaskData data) : base(player) => _data = data;
    public override void EnterState()
    {
        base.EnterState();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}
