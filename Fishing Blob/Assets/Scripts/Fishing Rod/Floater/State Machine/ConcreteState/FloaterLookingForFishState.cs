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
        Debug.Log(Floater.InputHandler.GetReelValue());
        if (Floater.InputHandler.GetReelValue())
        {
            Fsm.ChangeState(Floater.ReturningState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Fish"))
        {
            if (!Floater.Fishies.Contains(other.gameObject.GetComponent<Fish>()))
            {
                Floater.Fishies.Add(other.gameObject.GetComponent<Fish>());
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (Floater.Fishies.Contains(other.gameObject.GetComponent<Fish>()))
        {
            Floater.Fishies.Remove(other.gameObject.GetComponent<Fish>());
        }
    }

    private IEnumerator WaitForFish()
    {
        while (Floater.Fishies.Count == 0)
        {
            yield return new WaitForSecondsRealtime(Random.Range(5f, 10f));
        }
        yield return new WaitForSecondsRealtime(Random.Range(5f, 10f));
        Fsm.ChangeState(Floater.ChooseAFishState);
    }
}
