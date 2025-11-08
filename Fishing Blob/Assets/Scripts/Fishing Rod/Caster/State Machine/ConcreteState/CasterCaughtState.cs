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
            Debug.Log("BLEEEEEE");
            // TODO: ADD FISH TO INVENTORY BEFORE DESTROYING
            Caster.currentFloaterScript.ResetAndDestroyFloater();
            if (Caster.Rod.Fsm.IsInState(Caster.Rod.ThrowAndWait))
            {
                Caster.Rod.Fsm.ChangeState(Caster.Rod.IdleState);
            }
            if (Caster.containsFish)
            {
                Caster.currentFloaterScript.randomFish.ResetAndDestroyFish();
            }
            Caster.lineSpawner.DeleteLine();
            Fsm.ChangeState(Caster.IdleState);
        }
    }
}
