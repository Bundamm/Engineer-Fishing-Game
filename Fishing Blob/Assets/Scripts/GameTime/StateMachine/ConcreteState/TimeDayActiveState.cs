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

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (TimeManager.inputHandler.GetPauseValue())
        {
            TimeManager.PauseUnpause();
            AudioManager.Instance.PlaySound(AudioManager.SoundType.Pause, AudioManager.Instance.ManagerSource);
        }
        if (!TimeManager.TimerPaused)
        {
            TimeManager.MinutesValue += (decimal)Time.deltaTime * TimeManager.TickValue;
            if (TimeManager.MinutesValue >= 59)
            {
                TimeManager.MinutesValue = 0;
                TimeManager.HoursValue += 1;
            }

            if (TimeManager.HoursValue < TimeManager.TimeOfNoon)
            {
                TimeManager.volumeWeight -= Time.deltaTime * TimeManager.volumeWeightTick;
            }
            else if(TimeManager.HoursValue >= TimeManager.TimeOfAfternoon)
            {
                TimeManager.volumeWeight += Time.deltaTime * TimeManager.volumeWeightTick;
            }

            TimeManager.timeVolume.weight = TimeManager.volumeWeight;
            TimeManager.TimeText.text = $"{TimeManager.HoursValue:00}:{TimeManager.MinutesValue:00}";
        }
        else
        {
            Fsm.ChangeState(TimeManager.PausedState);
        }

        if (TimeManager.HoursValue >= 22)
        {
            Fsm.ChangeState(TimeManager.NightStoppedState);
        }
    }

    
}
