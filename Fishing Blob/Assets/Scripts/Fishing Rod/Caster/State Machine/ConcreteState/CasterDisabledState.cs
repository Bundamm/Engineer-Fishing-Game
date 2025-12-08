using UnityEngine;
public class CasterDisabledState : CasterState
{
    public CasterDisabledState(Caster caster, CasterStateMachine fsm) : base(caster, fsm)
    {
    }

    public override void EnterState()
    {
        Debug.Log("CasterDisabledState Entered");
    }
}
