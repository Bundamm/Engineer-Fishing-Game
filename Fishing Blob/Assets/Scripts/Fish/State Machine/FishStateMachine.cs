using UnityEngine;

public class FishStateMachine
{
    public FishState CurrentFishState { get; set; }

    public void Initialize(FishState initialState)
    {
        CurrentFishState = initialState;
        CurrentFishState.EnterState();
    }

    public void ChangeState(FishState newState)
    {
        CurrentFishState.ExitState();
        CurrentFishState = newState;
        CurrentFishState.EnterState();
    }

    public FishState GetCurrentState()
    {
        return CurrentFishState;
    }

    public bool IsInState(FishState state)
    {
        return state == CurrentFishState;
    }
}
