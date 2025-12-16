
public class TimeNightStoppedState : TimeState
{
    public TimeNightStoppedState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        if (!TimeManager.fishingRod.caster.Fsm.IsInState(TimeManager.fishingRod.caster.CaughtState) && 
            !TimeManager.fishingRod.Fsm.IsInState(TimeManager.fishingRod.ThrowAndWait))
        {
            TimeManager.fishingRod.Fsm.ChangeState(TimeManager.fishingRod.DisabledState);
            TimeManager.fishingRod.caster.Fsm.ChangeState(TimeManager.fishingRod.caster.DisabledState);
            TimeManager.fishingRod.playerObject.Fsm.ChangeState(TimeManager.fishingRod.playerObject.IdleState);
        }
        AudioManager.Instance.PlaySound(AudioManager.SoundType.DayOver, AudioManager.Instance.ManagerSource);
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (TimeManager.fishingRod.caster.Fsm.IsInState(TimeManager.fishingRod.caster.IdleState) &&
            TimeManager.fishingRod.Fsm.IsInState(TimeManager.fishingRod.IdleState))
        {
            TimeManager.fishingRod.Fsm.ChangeState(TimeManager.fishingRod.DisabledState);
            TimeManager.fishingRod.caster.Fsm.ChangeState(TimeManager.fishingRod.caster.DisabledState);
            TimeManager.fishingRod.playerObject.Fsm.ChangeState(TimeManager.fishingRod.playerObject.IdleState);
        }

        if (TimeManager.inputHandler.GetPauseValue())
        {
            TimeManager.PauseUnpause();
            TimeManager.Fsm.ChangeState(TimeManager.PausedState);
        }
        
    }
}
