public class MovingState : PlayerState
{
    public Movement Movement { get; private set; }
    public MovingState(Dad player, Movement movement) : base(player)
    {
        Movement = movement;
    }

    public override void EnterState()
    {
        base.EnterState();
        Movement.enabled = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        Movement.enabled = false;
    }
}
