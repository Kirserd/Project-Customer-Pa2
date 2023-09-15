public class TaskState : PlayerState
{
    private TaskData _data;
    public TaskState(Dad player, TaskData data) : base(player) => _data = data;
    public override void EnterState()
    {
        TaskIcon.AllTasksFade.Invoke(true);
        TaskStarter.AllHintsAppear.Invoke(true);
        base.EnterState();
    }
    public override void ExitState()
    {
        TaskIcon.AllTasksFade.Invoke(false);
        TaskStarter.AllHintsAppear.Invoke(false);
        base.ExitState();
    }
}
