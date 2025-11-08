using UnityEngine;
public class RodStateMachine
{
    public RodState CurrentRodState { get; set; }

    public void Initialize(RodState initialState)
    {
        CurrentRodState = initialState;
        CurrentRodState.EnterState();
    }

    public void ChangeState(RodState newState)
    {
        CurrentRodState.ExitState();
        CurrentRodState = newState;
        CurrentRodState.EnterState();
    }

    public bool IsInState(RodState state)
    {
        return CurrentRodState == state;
    }
}
