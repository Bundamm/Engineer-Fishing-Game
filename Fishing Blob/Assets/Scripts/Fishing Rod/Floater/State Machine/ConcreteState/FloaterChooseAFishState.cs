
using UnityEngine;

public class FloaterChooseAFishState : FloaterState
{
    public FloaterChooseAFishState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered Choose A Fish State Amount of fishies: " + Floater.fishies.Count );
        
        Floater.randomFish = Floater.fishies[Random.Range(0, Floater.fishies.Count)];
        Floater.randomFish.fishRB.linearVelocity = Vector2.zero;
        Debug.Log("Fish Chosen: " + Floater.randomFish + "Index in Fishes List" + Floater.fishies.IndexOf(Floater.randomFish));
        Floater.randomFish.Floater = Floater;
        Floater.randomFish.Fsm.ChangeState(Floater.randomFish.ApproachingState);
        
    }

    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Floater.inputHandler.GetReelValue())
        {
            Floater.randomFish.Fsm.ChangeState(Floater.randomFish.SpookedState);
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
