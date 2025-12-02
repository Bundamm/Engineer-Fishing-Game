using System.Collections;
using UnityEngine;
public class FishHookedState : FishState
{
    private Coroutine _spookCoroutine;
    
    public FishHookedState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering hooked state");
    }

    public override void ExitState()
    {
        base.ExitState();
        if (_spookCoroutine != null)
        {
            Fish.StopCoroutine(_spookCoroutine);
        }
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    
    

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Floater"))
        {
            Fish.fishRB.linearVelocity = Vector3.zero;
            _spookCoroutine = Fish.StartCoroutine(WaitForUntilSpookedState());
            Debug.Log("Fish stopped at floater");
        }
    }

    private IEnumerator WaitForUntilSpookedState()
    {
        yield return new WaitForSecondsRealtime(Fish.fishType.TimeUntilSpooked);
        Fsm.ChangeState(Fish.SpookedState);
    }
    

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
