using UnityEngine;

public class CasterCaughtState : CasterState
{
    public CasterCaughtState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("entered caster caught state");
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

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Floater"))
        {
            Caster.currentFloaterObject.ResetAndDestroyFloater();
            Caster.lineSpawner.DeleteLine();
            Caster.cameraManager.ChangeCamera(Caster.playerCamera, Caster.playerCharacter);
            Caster.Rod.CheckPlayerFacingDirection();
            if (Caster.Rod.Fsm.IsInState(Caster.Rod.ThrowAndWait))
            {
                Caster.Rod.Fsm.ChangeState(Caster.Rod.IdleState);
                Caster.Rod.playerObject.Fsm.ChangeState(Caster.Rod.playerObject.IdleState);
            }
            if (Caster.ContainsFish)
            {
                Caster.inventory.IncreaseAmountOfFish(Caster.currentFloaterObject.randomFish, 1);
                Caster.currentFloaterObject.randomFish.ResetAndDestroyFish();
            }
            Fsm.ChangeState(Caster.IdleState);
            
        }
    }
}
