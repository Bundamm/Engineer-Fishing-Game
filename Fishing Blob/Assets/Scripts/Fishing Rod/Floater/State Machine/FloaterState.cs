using UnityEngine;

public class FloaterState
{
    protected Floater Floater;
    protected FloaterStateMachine Fsm;
    protected Caster Caster;

    public FloaterState(Floater floater, FloaterStateMachine fsm)
    {
        Floater = floater;
        Fsm = fsm;
        Caster = floater.Caster;
    }
    
    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}
    public virtual void OnTriggerEnter2D(Collider2D other) {}
    public virtual void OnTriggerExit2D(Collider2D other) {}
    
    public virtual void OnCollisionEnter2D(Collision2D collision) {}
} 
