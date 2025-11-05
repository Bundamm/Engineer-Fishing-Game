using UnityEngine;

public class CasterCaughtState : CasterState
{
    public CasterCaughtState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Caster.catchTrigger.enabled = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        Caster.catchTrigger.enabled = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    
    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
