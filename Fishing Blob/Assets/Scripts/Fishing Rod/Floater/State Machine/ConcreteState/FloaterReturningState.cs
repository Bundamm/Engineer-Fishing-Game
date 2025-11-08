using UnityEngine;
public class FloaterReturningState : FloaterState
{

    private float percentageToCaster;
    private Vector2 floaterToAnchorPos;
    
    public FloaterReturningState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Floater.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        floaterToAnchorPos = Floater.CalculateMidPointBetweenFloaterAndCaster();
        percentageToCaster = 0f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        Floater.CasterPosition = Floater.Caster.transform.position;
        Floater.elapsedTime += Time.fixedDeltaTime;
        percentageToCaster = Floater.elapsedTime / Floater.maxTime;
        Floater.rigidbody2D.transform.position= Floater.QuadraticMovement(Floater.FloaterPosition, floaterToAnchorPos, Floater.CasterPosition, percentageToCaster);
    }
}
