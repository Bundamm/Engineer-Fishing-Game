using UnityEngine;

public class FishIdleState : FishState
{
    private Vector2 _targetDir;
    private Vector2 _startPos;
    private Vector2 _targetPos;
    private Vector2 _direction;
    private Vector2 _lowerStartPointOfWater;

    private float _waterHeight;
    private float _waterWidth;
    private float _waterBoundry;
    private int _amountOfTries;
    
    private float _randomMovementSpeed;
    
    public FishIdleState(Fish fish, FishStateMachine fsm) : base(fish, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Fish.fishRB.linearVelocity = Vector2.zero;
        _waterBoundry = Fish.waterBoundry;
        _targetDir = Fish.GetRandomDirectionInWater();
        _startPos = Fish.transform.position;
        _targetPos = _startPos + _targetDir * Fish.lengthOfDirectionVector;
        _randomMovementSpeed = Random.Range(1f, Fish.randomMovementSpeed);
        _lowerStartPointOfWater = new Vector2(Fish.waterStartPosX, Fish.waterStartPosY);
        _waterHeight = Fish.waterHeight;
        _waterWidth = Fish.waterWidth;
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
        
        Vector2 upperEndPointOfWater = new Vector2(Fish.waterStartPosX + _waterWidth, Fish.waterStartPosY + _waterHeight);
        Vector2 lowerEndPointOfWater = new Vector2(Fish.waterStartPosX + _waterWidth, Fish.waterStartPosY);
        _targetPos.x = Mathf.Clamp(_targetPos.x, _lowerStartPointOfWater.x + _waterBoundry, lowerEndPointOfWater.x -  _waterBoundry);
        _targetPos.y = Mathf.Clamp(_targetPos.y, _lowerStartPointOfWater.y + _waterBoundry, upperEndPointOfWater.y - _waterBoundry);
        Vector2 finishedMovement = movementLeft * _randomMovementSpeed;
        if (_targetPos.x > lowerEndPointOfWater.x || _targetPos.x < Fish.waterStartPosX ||
            _targetPos.y > upperEndPointOfWater.y || _targetPos.y < Fish.waterStartPosY)
        {
            finishedMovement *= -5;
        }
        
        
        Fish.MoveFish(finishedMovement);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }
    
}
