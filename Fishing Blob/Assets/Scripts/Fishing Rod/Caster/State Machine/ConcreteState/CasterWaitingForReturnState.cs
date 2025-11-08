using UnityEngine;
public class CasterWaitingForReturnState : CasterState
{
    public CasterWaitingForReturnState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Caster Waiting State");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        bool checkIfCaughtState = Caster.currentFloaterScript.Fsm.IsInState(Caster.currentFloaterScript.ReturningState);
        if (checkIfCaughtState)
        {
            Fsm.ChangeState(Caster.CaughtState);
        }
    }
}
