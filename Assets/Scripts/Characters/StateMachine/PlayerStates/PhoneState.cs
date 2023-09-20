public class PhoneState : PlayerState
{
    public PhoneState(Dad player) : base(player){}

    public override void EnterState()
    {
        TaskIcon.AllTasksFade?.Invoke(true);
        TaskStarter.AllHintsAppear?.Invoke(true);
        base.EnterState();
    }
    public override void ExitState()
    {
        TaskIcon.AllTasksFade?.Invoke(false);
        TaskStarter.AllHintsAppear?.Invoke(false);
        base.ExitState();
    }
}
