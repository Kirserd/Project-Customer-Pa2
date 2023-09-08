public class PlayerState
{
    protected Dad _player;
    protected PlayerStateMachine _stateMacnine;
    public PlayerState NextState { get; protected set; }
    public PlayerState(Dad player)
    {
        _player = player;
        _stateMacnine = player.PlayerStateMachine;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
}
