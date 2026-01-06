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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Floater.StickToSurface();
        if (Floater.timeManager.Fsm.IsInState(Floater.timeManager.PausedState)) return;
        if (Floater.inputHandler.GetReelValue())
        {
            Fsm.ChangeState(Floater.ReturningState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Fish"))
        {
            if (!Floater.fishies.Contains(other.gameObject.GetComponent<Fish>()))
            {
                Floater.fishies.Add(other.gameObject.GetComponent<Fish>());
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (Floater.fishies.Contains(other.gameObject.GetComponent<Fish>()))
        {
            Floater.fishies.Remove(other.gameObject.GetComponent<Fish>());
        }
    }

    private IEnumerator WaitForFish()
    {
        while (Floater.fishies.Count == 0)
        {
            yield return new WaitForSecondsRealtime(Random.Range(0.5f, 5f));
        }
        yield return new WaitForSecondsRealtime(Random.Range(0.5f, 5f));
        if (Floater.fishies.Count > 0)
        {
            Fsm.ChangeState(Floater.ChooseAFishState);
        }
        
    }
}
