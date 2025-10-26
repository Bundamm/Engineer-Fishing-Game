using System.Collections;
using UnityEngine;

public class FloaterLookingForFishState :FloaterState
{
    public FloaterLookingForFishState(Floater floater, FloaterStateMachine fsm) : base(floater, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered LookingForFishState");
        Floater.EnableApproachingCollider();
        Floater.EnableBitingCollider();
        Floater.StartCoroutine(WaitForFish());
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
        Floater.StickToSurface();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Fish"))
        {
            if (!Floater.Fishes.Contains(other.gameObject.GetComponent<Fish>()))
            {
                Floater.Fishes.Add(other.gameObject.GetComponent<Fish>());
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (Floater.Fishes.Contains(other.gameObject.GetComponent<Fish>()))
        {
            Floater.Fishes.Remove(other.gameObject.GetComponent<Fish>());
        }
    }

    private IEnumerator WaitForFish()
    {
        yield return new WaitForSecondsRealtime(Random.Range(10f, 15f));
        if (Floater.Fishes.Count >= 1)
        {
            Fsm.ChangeState(Floater.ChooseAFishState);
        }
    }
}
