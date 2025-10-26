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
        Fish.Floater.Fsm.ChangeState(Fish.Floater.WaitForBitingState);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Vector3 lookAtFloater = Fish.transform.InverseTransformPoint(Fish.Floater.gameObject.transform.position);
        float angleBetweenFishAndFloater = Mathf.Atan2(lookAtFloater.y, lookAtFloater.x) * Mathf.Rad2Deg - 90;
        Fish.transform.Rotate(0, 0, angleBetweenFishAndFloater);
        
        Vector2 targetPosition = Fish.Floater.gameObject.transform.position;
        Vector2 targetPath = targetPosition - (Vector2)Fish.transform.position;
        Fish.MoveFish(targetPath);
        
        
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
