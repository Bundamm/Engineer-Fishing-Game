using UnityEngine;
public class FloaterCaughtState : FloaterState
{

    
    
    public FloaterCaughtState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Floater.floaterToCasterAnchor = GameObject.FindGameObjectWithTag("Anchor");
        Floater.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
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
        float percentageToCaster = Mathf.Clamp01(Floater.elapsedTime / Floater.maxTime);
        Vector2 currentStep = Floater.QuadraticMovement(Floater.FloaterPosition, Floater.floaterToCasterAnchor.transform.position, Floater.CasterPosition, percentageToCaster);
        
        Floater.rigidbody2D.MovePosition(currentStep);
    }
    
}
