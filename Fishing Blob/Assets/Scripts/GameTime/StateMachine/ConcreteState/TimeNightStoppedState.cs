
public class TimeNightStoppedState : TimeState
{
    public TimeNightStoppedState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        //TODO: FIX THE ROD GETTING STUCK IN CHARGING STATE
        
        base.EnterState();
        if (!TimeManager.fishingRod.Caster.Fsm.IsInState(TimeManager.fishingRod.Caster.CaughtState) && 
            !TimeManager.fishingRod.Fsm.IsInState(TimeManager.fishingRod.ThrowAndWait))
        {
            TimeManager.fishingRod.Fsm.ChangeState(TimeManager.fishingRod.DisabledState);
            TimeManager.fishingRod.Caster.Fsm.ChangeState(TimeManager.fishingRod.Caster.DisabledState);
            TimeManager.fishingRod.playerObject.Fsm.ChangeState(TimeManager.fishingRod.playerObject.IdleState);
        }
            
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (TimeManager.fishingRod.Caster.Fsm.IsInState(TimeManager.fishingRod.Caster.IdleState) &&
            TimeManager.fishingRod.Fsm.IsInState(TimeManager.fishingRod.IdleState))
        {
            TimeManager.fishingRod.Fsm.ChangeState(TimeManager.fishingRod.DisabledState);
            TimeManager.fishingRod.Caster.Fsm.ChangeState(TimeManager.fishingRod.Caster.DisabledState);
            TimeManager.fishingRod.playerObject.Fsm.ChangeState(TimeManager.fishingRod.playerObject.IdleState);
        }
    }
}
