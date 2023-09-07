public class MovingState : PlayerState
{
    public Movement Movement { get; private set; }
    public MovingState(Dad player, PlayerStateMachine stateMachine, Movement movement) : base(player, stateMachine)
    {
        Movement = movement;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
