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
        TimeManager.player.Fsm.ChangeState(TimeManager.player.IdleState);
        if (!TimeManager.marketUIManager.GetCurrentMarketUIValue())
        {
            TimeManager.pauseCanvas.gameObject.SetActive(true);
            AudioManager.Instance.PlaySound(AudioManager.SoundType.Pause, AudioManager.Instance.ManagerSource);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        if (TimeManager.pauseCanvas.gameObject.activeInHierarchy)
        {
            TimeManager.pauseCanvas.gameObject.SetActive(false);
            AudioManager.Instance.PlaySound(AudioManager.SoundType.Unpause, AudioManager.Instance.ManagerSource);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (TimeManager.inputHandler.GetPauseValue())
        {
            TimeManager.PauseUnpause();
        }
        if (!TimeManager.TimerPaused)
        {
            if (TimeManager.marketUIManager.GetCurrentMarketUIValue())
            {
                TimeManager.marketUIManager.ToggleMarketUI();
                
            }
            if (TimeManager.HoursValue < 22)
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
