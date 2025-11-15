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

    public override void FrameUpdate()
    {
        
    }
}
