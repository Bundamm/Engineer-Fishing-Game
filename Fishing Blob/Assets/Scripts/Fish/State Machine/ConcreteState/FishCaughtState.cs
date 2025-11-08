using UnityEngine;

public class FishCaughtState : FishState
{
    
    public FishCaughtState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered FishCaughtState");
        Fish.StickToFloater();
        Fish.Floater.Caster.containsFish = true;

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        
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
