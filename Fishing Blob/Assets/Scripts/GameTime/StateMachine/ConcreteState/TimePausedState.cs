using UnityEngine;

public class TimePausedState : TimeState
{
    public TimePausedState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //TODO: UI MANAGER ENABLE PAUSE UI
    }

    public override void ExitState()
    {
        base.ExitState();
        //TODO: UI MANAGER DISABLE PAUSE UI, ENABLE GAME UI
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        TimeManager.PauseUnpause();
        if (!TimeManager.timerPaused)
        {
            Fsm.ChangeState(TimeManager.DayActiveState);
        }
    }
}
