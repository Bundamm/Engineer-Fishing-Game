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
        Floater.FloaterRb.bodyType = RigidbodyType2D.Kinematic;
        floaterToAnchorPos = Floater.CalculateMidPointBetweenFloaterAndCaster();
        Floater.Water.Splash(Floater.GetComponent<Collider2D>(), 5f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.Reel, Floater.FloaterSource);
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
        if (Floater.timeManager.Fsm.IsInState(Floater.timeManager.PausedState)) return;
        
        Floater.CasterPosition = Floater.Caster.transform.position;
        Floater.elapsedTime += Time.fixedDeltaTime;
        percentageToCaster = Floater.elapsedTime / Floater.maxTime;
        Floater.FloaterRb.transform.position= Floater.QuadraticMovement(Floater.FloaterPosition, floaterToAnchorPos, Floater.CasterPosition, percentageToCaster);
    }
}
