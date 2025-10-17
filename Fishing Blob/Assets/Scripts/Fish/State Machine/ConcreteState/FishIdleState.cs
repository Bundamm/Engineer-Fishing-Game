using UnityEngine;

public class FishIdleState : FishState
{
    private Vector2 _targetDir;
    private Vector2 _startPos;
    private Vector2 _targetPos;
    private Vector2 _direction;
    private Vector2 _startPointOfWater;

    private float _waterHeight;
    private float _waterWidth;
    private int _amountOfTries;
    
    private float _randomMovementSpeed;
    
    public FishIdleState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _targetDir = Fish.GetRandomDirectionInWater();
        _startPos = Fish.transform.position;
        _targetPos = _startPos + _targetDir * Fish.lengthOfDirectionVector;
        _randomMovementSpeed = Random.Range(1f, Fish.randomMovementSpeed);
        _startPointOfWater = new Vector2(Fish.waterStartPosX, Fish.waterStartPosY);
    }
    
    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Vector2 movementLeft = _targetPos - (Vector2)Fish.transform.position;
        if(movementLeft.sqrMagnitude < 0.01f)
        {
            _targetDir = Fish.GetRandomDirectionInWater();
            _startPos = Fish.transform.position;
            _targetPos = _startPos + _targetDir * Fish.lengthOfDirectionVector;
            _randomMovementSpeed = Random.Range(1f, Fish.randomMovementSpeed);
        }
        Vector2 finishedMovement = movementLeft * _randomMovementSpeed;
        Vector2 endPointOfWater = new Vector2(_startPointOfWater.x + _waterWidth, _startPointOfWater.y + _waterHeight);
        // TODO: Fix
        Debug.Log("End point of water " + endPointOfWater + "Start Point of water " + _startPointOfWater + "Fish position " + Fish.transform.position);
        if (endPointOfWater.x - Fish.transform.position.x < 0.01f || Fish.transform.position.x - _startPointOfWater.x < 0.01f || Fish.transform.position.y - _startPointOfWater.y < 0.01f || endPointOfWater.y - Fish.transform.position.y < 0.01f)
        {
            _amountOfTries++;
            finishedMovement *= -1;
        }

        if (_amountOfTries >= 3)
        {
            _amountOfTries = 0;
            finishedMovement *= -1 * 5;
        }
        
        Fish.MoveFish(finishedMovement);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    public override void AnimationTriggerEvent(Fish.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        
    }
    

    

}
