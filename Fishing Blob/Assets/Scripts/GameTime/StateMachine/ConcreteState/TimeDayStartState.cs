using System.Collections;
using UnityEngine;
public class TimeDayStartState : TimeState
{
    public TimeDayStartState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        TimeManager.MinutesValue = 0;
        TimeManager.HoursValue = 6;
        TimeManager.volumeWeight = 0.3f;
        TimeManager.timeVolume.weight = TimeManager.volumeWeight;
        TimeManager.TimeText.text = $"{TimeManager.HoursValue:00}:{TimeManager.MinutesValue:00}";
        TimeManager.gameUICanvas.gameObject.SetActive(true);
        if (TimeManager.DayCounterValue == 1)
        {
            TimeManager.StartCoroutine(DayOneInfoText());
        }
        AudioManager.Instance.PlaySound(AudioManager.SoundType.DayStart, AudioManager.Instance.ManagerSource);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Mathf.Abs(InputHandler.GetMoveValue().x) > 0 || InputHandler.GetCastValue())
        {
            Fsm.ChangeState(TimeManager.DayActiveState);
        }
    }

    private IEnumerator DayOneInfoText()
    {
        TimeManager.dayOneTimeInfoAnimator.SetBool("FadeIn", true);
        yield return new WaitForSecondsRealtime(7f);
        TimeManager.dayOneTimeInfoAnimator.SetBool("FadeIn", false);
    }
}
