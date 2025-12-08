
public class TimeStateMachine
{
    public TimeState CurrentTimeState { get; set; }

    public void Initialize(TimeState initialState)
    {
        CurrentTimeState = initialState;
        CurrentTimeState.EnterState();
    }

    public void ChangeState(TimeState newState)
    {
        CurrentTimeState.ExitState();
        CurrentTimeState = newState;
        CurrentTimeState.EnterState();
    }

    public bool IsInState(TimeState state)
    {
        return CurrentTimeState == state;
    }
    
}
