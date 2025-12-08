using UnityEngine;
public class RodDisabledState : RodState
{
    public RodDisabledState(Rod rod, RodStateMachine fsm) : base(rod, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Rod Disabled State Entered");
    }
}
