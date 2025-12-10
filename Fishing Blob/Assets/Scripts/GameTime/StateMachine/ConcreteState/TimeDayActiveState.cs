using UnityEngine;
using UnityEngine.Rendering;

public class TimeDayActiveState : TimeState
{
    public TimeDayActiveState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Time Active State");
        
    }

    public override void ExitState()
    {
        base.ExitState();
        
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
            TimeManager.minutesValue += (decimal)Time.deltaTime * TimeManager.tickValue;
            if (TimeManager.minutesValue >= 60)
            {
                TimeManager.minutesValue = 0;
                TimeManager.hoursValue += 1;
            }

            if (TimeManager.hoursValue < TimeManager.timeOfNoon)
            {
                TimeManager.volumeWeight -= Time.deltaTime * TimeManager.volumeWeightTick;
            }
            else if(TimeManager.hoursValue >= TimeManager.timeOfAfternoon)
            {
                TimeManager.volumeWeight += Time.deltaTime * TimeManager.volumeWeightTick;
            }

            TimeManager.timeVolume.weight = TimeManager.volumeWeight;
            TimeManager.timeText.text = $"{TimeManager.hoursValue:00}:{TimeManager.minutesValue:00}";
        }
        else
        {
            Fsm.ChangeState(TimeManager.PausedState);
        }

        if (TimeManager.hoursValue >= 22)
        {
            Fsm.ChangeState(TimeManager.NightStoppedState);
        }
    }

    
}
