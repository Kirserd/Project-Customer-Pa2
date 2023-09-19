public class PlayerState
{
    protected Dad _player;
    protected PlayerStateMachine _stateMachine;
    public PlayerState NextState { get; protected set; }
    public PlayerState(Dad player)
    {
        _player = player;
        _stateMachine = Dad.PlayerStateMachine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
}
