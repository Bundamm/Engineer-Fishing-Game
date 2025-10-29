using UnityEngine;
public class FloaterWaitForBitingState : FloaterState
{
    public FloaterWaitForBitingState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Waiting for Biting State");
        Floater.DisableApproachingCollider();
        Floater.EnableBitingCollider();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Floater.StickToSurface();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        Debug.Log("Floater random fish: " + Floater.randomFish);
        if (other.CompareTag("Fish") && other.GetComponent<Fish>() == Floater.randomFish)
        {
            Debug.Log("Random fish entered biting state");
            Floater.randomFish.Fsm.ChangeState(Floater.randomFish.BitingState);
            Floater.DisableBitingCollider();
            Fsm.ChangeState(Floater.WaitForCaughtState);
        }
        
    }
    
}
