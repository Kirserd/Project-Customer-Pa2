public class PlayerStateMachine
{
    public PlayerState CurrentState { get; protected set; }
    public void Initialize(PlayerState initializeState) => CurrentState = initializeState;
    public void UpdateState(PlayerState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
    public void UpdateState()
    {
        var newState = CurrentState.NextState;
        if (newState == null)
            return;

        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
