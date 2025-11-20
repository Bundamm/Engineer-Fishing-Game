public class PlayerStateMachine
{
    public PlayerState CurrentPlayerState { get; set; }

    public void Initialize(PlayerState initialState)
    {
        CurrentPlayerState = initialState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = newState;
        CurrentPlayerState.EnterState();
    }

    public PlayerState GetCurrentState()
    {
        return CurrentPlayerState;
    }

    public bool IsInState(PlayerState state)
    {
        return state == CurrentPlayerState;
    }
}
