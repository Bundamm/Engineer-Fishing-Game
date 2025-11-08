using UnityEngine;

public class FloaterWaitForCaughtState : FloaterState
{
    public FloaterWaitForCaughtState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered waiting for caught state");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        bool fishStateCheck = Floater.randomFish.Fsm.IsInState(Floater.randomFish.HookedState);
        bool reelPerformed = Floater.InputHandler.ReelPerformed();
        if (reelPerformed && fishStateCheck)
        {
            Debug.Log("Reeling Fish");
            Floater.randomFish.Fsm.ChangeState(Floater.randomFish.CaughtState);
            Fsm.ChangeState(Floater.ReturningState);
        }
        else if (reelPerformed)
        {
            Debug.Log("FISH ESCAPED");
            Floater.randomFish.Fsm.ChangeState(Floater.randomFish.SpookedState);
            Fsm.ChangeState(Floater.ReturningState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Floater.StickToSurface();
    }
}
