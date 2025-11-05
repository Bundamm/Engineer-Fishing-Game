using UnityEngine;

public class FloaterStateMachine
{
    public FloaterState CurrentFloaterState { get; protected set; }

    public void Initialize(FloaterState initialState)
    {
        CurrentFloaterState = initialState;
        CurrentFloaterState.EnterState();
    }

    public void ChangeState(FloaterState newState)
    {
        CurrentFloaterState.ExitState();
        CurrentFloaterState = newState;
        CurrentFloaterState.EnterState();
    }
    
    public bool IsInState(FloaterState state)
    {
        return state == CurrentFloaterState;
    }
}
