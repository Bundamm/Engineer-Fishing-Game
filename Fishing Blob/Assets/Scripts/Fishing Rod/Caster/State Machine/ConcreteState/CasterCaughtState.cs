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

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Floater"))
        {
            Caster.currentFloaterObject.ResetAndDestroyFloater();
            Caster.lineSpawner.DeleteLine();
            Caster.cameraManager.ChangeCamera(Caster.playerCamera, Caster.playerCharacter);
            Caster.rod.CheckPlayerFacingDirection();
            if (Caster.rod.Fsm.IsInState(Caster.rod.ThrowingState))
            {
                Caster.rod.Fsm.ChangeState(Caster.rod.IdleState);
                Caster.rod.playerObject.Fsm.ChangeState(Caster.rod.playerObject.IdleState);
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
