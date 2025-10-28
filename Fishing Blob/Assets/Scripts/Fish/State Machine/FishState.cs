using UnityEngine;

public class FishState
{
    protected Fish Fish;
    protected FishStateMachine Fsm;

    public FishState(Fish fish, FishStateMachine fsm)
    {
        Fish = fish;
        Fsm = fsm;
    }
    
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(Fish.AnimationTriggerType triggerType) {}
    
    public virtual void OnTriggerEnter2D(Collider2D other) {}
    public virtual void OnTriggerExit2D(Collider2D other) {}
    
}
