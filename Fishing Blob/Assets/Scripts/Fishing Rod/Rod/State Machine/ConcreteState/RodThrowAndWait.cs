using UnityEngine;

public class RodThrowAndWait : RodState
{
    
    public RodThrowAndWait(Rod rod, RodStateMachine fsm) : base(rod, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Rod Release Floater State");
        Rod.CasterThrowFloater();
        Rod.playerObject.Fsm.ChangeState(Rod.playerObject.DisableMovementState);
        
    }

    public override void ExitState()
    {
        
    }
    
}
