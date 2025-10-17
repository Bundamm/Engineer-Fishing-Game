using UnityEngine;

public class FishSpookedState : FishState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public FishSpookedState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
    public override void AnimationTriggerEvent(Fish.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
    
}
