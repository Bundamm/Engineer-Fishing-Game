using UnityEngine;

public class RodThrowingState : RodState
{
    
    public RodThrowingState(Rod rod, RodStateMachine fsm) : base(rod, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Rod Release Floater State");
        Rod.CasterThrowFloater();
        Rod.playerObject.Fsm.ChangeState(Rod.playerObject.StationaryState);
        
    }

    public override void ExitState()
    {
        
    }
    
}
