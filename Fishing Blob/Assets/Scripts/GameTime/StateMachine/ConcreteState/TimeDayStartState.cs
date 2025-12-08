using UnityEngine;
public class TimeDayStartState : TimeState
{
    public TimeDayStartState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        TimeManager.minutesValue = 0;
        TimeManager.hoursValue = 6;
        TimeManager.volumeWeight = 0.3f;
        TimeManager.timeVolume.weight = TimeManager.volumeWeight;
        TimeManager.timeText.text = $"{TimeManager.hoursValue:00}:{TimeManager.minutesValue:00}";
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Mathf.Abs(InputHandler.GetMoveValue().x) > 0 || InputHandler.GetCastValue())
        {
            Fsm.ChangeState(TimeManager.DayActiveState);
        }
    }
}
