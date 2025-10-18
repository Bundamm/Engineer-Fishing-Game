using UnityEngine;

public class Fish : MonoBehaviour, IFishMovable
{
    public Rigidbody2D fishRB { get; set; }

    #region Water Variables
    
    private Water _water;
    [SerializeField]
    private float waterBoundry = 0.4f;
    
    public float waterHeight;
    public float waterWidth;
    public float waterStartPosX;
    public float waterStartPosY;
    #endregion

    #region State Machine Variables
    public FishStateMachine Fsm { get; set; }
    public FishIdleState IdleState { get; set; }
    public FishApproachingState ApproachingState { get; set; }
    public FishBitingState BitingState { get; set; }
    public FishCaughtState CaughtState { get; set; }
    public FishSpookedState SpookedState { get; set; }
    #endregion

    #region Fish Idle Variables
    public float lengthOfDirectionVector = 2f;
    public float randomMovementSpeed = 2f;
    #endregion
    
    
    private void Awake()
    {
        _water = FindAnyObjectByType<Water>();
        Fsm = new FishStateMachine();

        IdleState = new FishIdleState(this, Fsm);
        ApproachingState = new FishApproachingState(this, Fsm);
        BitingState = new FishBitingState(this, Fsm);
        CaughtState = new FishCaughtState(this, Fsm);
        SpookedState = new FishSpookedState(this, Fsm);

    }
    
    
    private void Start()
    {
        fishRB =  GetComponent<Rigidbody2D>();
        
        waterHeight = _water.GetMeshHeight();
        waterWidth = _water.GetMeshWidth();
        Debug.Log("water height: " + waterHeight + "water width" + waterWidth);
        waterStartPosX = _water.transform.position.x;
        waterStartPosY = _water.transform.position.y;                
        
        Fsm.Initialize(IdleState);
        
        
    }

    private void Update()
    {
        waterHeight = _water.GetMeshHeight();
        waterWidth = _water.GetMeshWidth();
        Fsm.CurrentFishState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        Fsm.CurrentFishState.PhysicsUpdate();
    }
    
    #region Animation Triggers
    
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        Fsm.CurrentFishState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        Example
    }
    
    #endregion
    
    #region Movement Functions
    public void MoveFish(Vector2 velocity)
    {
        fishRB.linearVelocity = velocity;
        fishRB.transform.up = velocity.normalized;
    }

    public Vector2 GetRandomDirectionInWater()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        return randomDirection;
    }

    #endregion
    
}
