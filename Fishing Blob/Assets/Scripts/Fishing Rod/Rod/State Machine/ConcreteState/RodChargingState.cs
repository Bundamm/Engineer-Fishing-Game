using UnityEngine;

public class RodChargingState : RodState
{
    
    public RodChargingState(Rod rod, RodStateMachine fsm) : base(rod, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering RodChargingState");
        Rod.RodRotator.SetIsRotated(false);
        Rod.playerObject.Fsm.ChangeState(Rod.playerObject.PrepareState);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting RodChargingState");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (InputHandler.GetCastValue())
        {
            ChargeCast();
        }
        else
        {
            if (Rod.RodRotator.GetIsRotated())
            {
                RodRotator.SetIsRotated(false);
                Fsm.ChangeState(Rod.CastingState);
            }
            else
            {
                RodRotator.RotateRod(RodRotator.GetStartRotation(), Rod.chargeRotationSpeed);
                Fsm.ChangeState(Rod.IdleState);
                Rod.playerObject.Fsm.ChangeState(Rod.playerObject.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void ChargeCast()
    {
        Rod.CheckPlayerFacingDirection();
        
        RodRotator.RotateRod(Rod.targetRotation, Rod.chargeRotationSpeed);
        if (RodRotator.GetIsRotated())
        {
            Rod.CalculateCastPower();
        }
        
    }
}
