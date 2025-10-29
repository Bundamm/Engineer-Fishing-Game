using System.Collections;
using UnityEngine;

public class FishBitingState : FishState
{

    private bool _movingTowardsStart;
    private Vector2 _pathToFloater;
    private Vector2 _pathToStart;
    private Coroutine _waitCoroutine;
    private int _randomAmountOfBitesCounter;
    private int _currentBiteNumber;
    public FishBitingState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Fish Entered Biting State");
        Fish.fishRB.linearVelocity = Vector3.zero;
        Fish.StartFishPositionAtBiting = Fish.fishRB.transform.position;
        _movingTowardsStart = false;
        Random.InitState(System.DateTime.Now.Millisecond);
        _randomAmountOfBitesCounter = Random.Range(1, 4);
        Debug.Log(_randomAmountOfBitesCounter);
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        _pathToStart = Fish.GetPathToStartPositionOfFish();
        _pathToFloater = Fish.GetPathToFloater();
        if (!_movingTowardsStart)
        {
            Fish.MoveFishWithoutRotating(_pathToFloater);
        }

        if (_movingTowardsStart)
        {
            Fish.MoveFishWithoutRotating(_pathToStart);
            CheckDistanceToStartPos();
        }

        if (_currentBiteNumber >= _randomAmountOfBitesCounter)
        {
            Fish.MoveFishWithoutRotating(_pathToFloater);
            Fsm.ChangeState(Fish.HookedState);
        }
    }
    
    private IEnumerator WaitAtStart()
    {
        yield return new WaitForSecondsRealtime(Random.Range(1f, 3f));
        _movingTowardsStart = false;
        _currentBiteNumber++;
        _waitCoroutine = null;
        
    }

    public void CheckDistanceToStartPos()
    {

        if (Vector2.Distance(Fish.StartFishPositionAtBiting, Fish.GetFishPosition()) < 0.5f)
        {
            _waitCoroutine ??= Fish.StartCoroutine(WaitAtStart());
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Floater"))
        {
            _movingTowardsStart = true;
            Fish.fishRB.linearVelocity = Vector3.zero;
            Debug.Log("At floater is now true");
        }
    }
}
