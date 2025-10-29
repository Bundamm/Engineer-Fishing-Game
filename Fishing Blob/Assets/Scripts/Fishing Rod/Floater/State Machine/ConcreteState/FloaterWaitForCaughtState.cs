using UnityEngine;

public class FloaterWaitForCaughtState : FloaterState
{
    public FloaterWaitForCaughtState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        bool fishStateCheck = Floater.randomFish.Fsm.IsInState(Floater.randomFish.HookedState);
        if (Floater.InputHandler.ReelPerformed() && fishStateCheck)
        {
            Floater.randomFish.Fsm.ChangeState(Floater.randomFish.CaughtState);
            Fsm.ChangeState(Floater.CaughtState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Floater.StickToSurface();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
