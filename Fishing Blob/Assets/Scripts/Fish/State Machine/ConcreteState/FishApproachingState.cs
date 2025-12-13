using UnityEngine;
using UnityEngine.Audio;

public class FishApproachingState : FishState
{
    public FishApproachingState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Fish Approaching State");
        Fish.fishRB.angularVelocity = 0f;
        Fish.fishRB.linearVelocity = Vector2.zero;
        Fish.Floater.FloaterStateMachine.ChangeState(Fish.Floater.WaitForBitingState);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Fish.Floater.inputHandler.GetReelValue())
        {
            Fsm.ChangeState(Fish.SpookedState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        float angleBetweenFishAndFloater = Fish.GetAngleBetweenFishAndFloater();
        Fish.transform.Rotate(0, 0, angleBetweenFishAndFloater);

        Vector2 targetPath = Fish.GetPathToFloater();
        Fish.MoveFish(targetPath);
    }

    public override void AnimationTriggerEvent(Fish.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
