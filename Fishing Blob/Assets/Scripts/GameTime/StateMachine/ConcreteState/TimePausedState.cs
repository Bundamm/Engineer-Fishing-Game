using UnityEngine;

public class TimePausedState : TimeState
{
    public TimePausedState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Time Paused State");
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
        if (TimeManager.inputHandler.GetPauseValue())
        {
            TimeManager.PauseUnpause();
        }
        if (!TimeManager.timerPaused)
        {
            if (TimeManager.marketUIManager.GetCurrentMarketUIValue())
            {
                TimeManager.marketUIManager.ToggleMarketUI();
            }
            if (TimeManager.hoursValue < 22)
            {
                Fsm.ChangeState(TimeManager.DayActiveState);
            }
            else
            {
                Fsm.ChangeState(TimeManager.NightStoppedState);
            }
        }
    }
}
