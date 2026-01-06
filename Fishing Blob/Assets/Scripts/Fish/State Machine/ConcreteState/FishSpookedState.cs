using System.Collections;
using UnityEngine;

public class FishSpookedState : FishState
{
    private bool _movingAwayFromFloater = true;
    private Vector2 _floaterPos;
    
    public FishSpookedState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered FishSpookedState");
        Fish.fishRB.linearVelocity = Vector3.zero;
        _floaterPos = Fish.Floater.transform.position;
        Fish.StartCoroutine(WaitUntilIdle());
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!_movingAwayFromFloater)
        {
            Fsm.ChangeState(Fish.IdleState);
        }
        MoveAwayFromFloater();
    }

    private IEnumerator WaitUntilIdle()
    {
        yield return new WaitForSecondsRealtime(Fish.fishType.WaitUntilIdleTime);
        _movingAwayFromFloater = false;
    }

private void MoveAwayFromFloater()
    {
        Vector2 directionAwayFromFloater = ((Vector2)Fish.transform.position - _floaterPos).normalized;
        Fish.MoveFish(directionAwayFromFloater);
    }
}
