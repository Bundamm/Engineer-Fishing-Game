using UnityEngine;
    
public class CasterState
{
    protected Caster Caster;
    protected CasterStateMachine Fsm;
    protected Rod Rod;


    public CasterState(Caster caster, CasterStateMachine fsm)
    {
        Caster = caster;
        Fsm = fsm;
        Rod = caster.rod;
    }

    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}
    
    public virtual void OnTriggerEnter2D(Collider2D other) {}
    public virtual void AnimationTriggerEvent(Rod.AnimationTriggerType triggerType) {}
    
}
