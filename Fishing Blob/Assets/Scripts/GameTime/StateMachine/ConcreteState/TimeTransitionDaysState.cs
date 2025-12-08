
using UnityEngine;

public class TimeTransitionDaysState : TimeState
{
    public TimeTransitionDaysState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered TimeTransitionDaysState");
        TimeManager.dayCounterValue += 1;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
}
