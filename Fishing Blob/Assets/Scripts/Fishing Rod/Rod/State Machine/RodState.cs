using UnityEngine;

public class RodState
{
    protected Rod Rod;
    protected RodStateMachine Fsm;
    protected InputHandler InputHandler;
    protected RodRotator RodRotator;

    public RodState(Rod rod, RodStateMachine fsm)
    {
        Rod = rod;
        Fsm = fsm;
        InputHandler = rod.InputHandler;
        RodRotator = rod.RodRotator;
        
    }
    
    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}
}
