using UnityEngine;

public class InitialFloaterState : FloaterState
{
    public InitialFloaterState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        Floater.UnstickFromSurface();
        Floater.FloaterRb.bodyType = RigidbodyType2D.Dynamic;
        Debug.Log("Entered Initial Floater State");
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
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (other is EdgeCollider2D)
            {
                float vel = Floater.FloaterRb.linearVelocity.y * Floater.Water.forceMultiplier;
                vel = Mathf.Clamp(Mathf.Abs(vel), 0f, Floater.Water.maxForce);
                Floater.Water.Splash(Floater.GetComponent<Collider2D>(), vel*1.5f);
                Floater.FloaterPosition = Floater.transform.position;
                Fsm.ChangeState(Floater.LookingForFishState);
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Floater.FloaterPosition = Floater.transform.position;
            Floater.FloaterRb.linearVelocity = Vector2.zero;
            Fsm.ChangeState(Floater.ReturningState);
        }
    }
}
