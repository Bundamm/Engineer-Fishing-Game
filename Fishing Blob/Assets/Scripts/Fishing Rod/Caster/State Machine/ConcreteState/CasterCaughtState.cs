using UnityEngine;

public class CasterCaughtState : CasterState
{
    public CasterCaughtState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
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
        if (other.CompareTag("Fish") && Caster.currentFloaterScript.randomFish == other.GetComponent<Fish>())
        {
            // TODO: ADD FISH TO INVENTORY BEFORE DESTROYING
            
            Caster.currentFloaterScript.ResetAndDestroyFloater();
            Caster.currentFloaterScript.randomFish.ResetAndDestroyFish();
            Fsm.ChangeState(Caster.IdleState);
        } 
    }
}
