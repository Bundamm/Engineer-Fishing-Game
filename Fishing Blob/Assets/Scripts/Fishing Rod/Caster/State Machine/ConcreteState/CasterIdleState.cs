using UnityEngine;

public class CasterIdleState : CasterState
{

    public CasterIdleState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Caster.ContainsFish = false;
        Debug.Log("Entering Caster Idle State");
    }
}