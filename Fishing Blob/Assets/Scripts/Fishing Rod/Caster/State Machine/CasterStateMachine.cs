using UnityEngine;
public class CasterStateMachine
{
    public CasterState CurrentCasterState { get; protected set; }

    public void Initialize(CasterState initialState)
    {
        CurrentCasterState = initialState;
        CurrentCasterState.EnterState();
    }

    public void ChangeState(CasterState newState)
    {
        CurrentCasterState.ExitState();
        CurrentCasterState = newState;
        CurrentCasterState.EnterState();
    }

    public bool IsInState(CasterState state)
    {
        return CurrentCasterState == state;
    } 
    
}
