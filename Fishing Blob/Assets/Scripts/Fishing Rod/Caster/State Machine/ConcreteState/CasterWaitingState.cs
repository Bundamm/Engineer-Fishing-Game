using UnityEngine;
public class CasterWaitingState : CasterState
{
    public CasterWaitingState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Caster Waiting State");
    }    

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        bool checkIfCaughtState = Caster.currentFloaterObject.FloaterStateMachine.IsInState(Caster.currentFloaterObject.ReturningState);
        if (checkIfCaughtState)
        {
            Fsm.ChangeState(Caster.CaughtState);
        }
    }
}
