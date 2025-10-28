using UnityEngine;

public class Fish : MonoBehaviour, IFishMovable, IFishAndFloaterPositionAndRotation
{
    #region Fish Variables
    public Rigidbody2D fishRB { get; set; }

    [SerializeField]
    private CircleCollider2D fishFaceCollider; 
    
    #endregion

    #region Water Variables
    
    private Water _water;
    public float waterBoundry = 0.4f;
    
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
    
    #region Fish Approaching Variables
    public Floater Floater { get; set; }
    public Vector2 StartFishPositionAtBiting { get; set; }
    #endregion
    
    #region Basic Unity Methods
    
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
    #endregion
    
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
    
    #region Movement Methods
    public void MoveFish(Vector2 velocity)
    {
        fishRB.linearVelocity = velocity;
        fishRB.transform.up = velocity.normalized;
    }

    public void MoveFishWithoutRotating(Vector2 velocity)
    {
        fishRB.linearVelocity = velocity;
    }

    public Vector2 GetRandomDirectionInWater()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        return randomDirection;
    }

    #endregion
    
    #region Floater Position And Rotation Methods
    public Vector2 GetPathToFloater()
    {
        Vector2 targetPosition = Floater.gameObject.transform.position;
        Vector2 targetPath = targetPosition - (Vector2)transform.position;
        return  targetPath;
    }

    public Vector2 GetPathToStartPositionOfFish()
    {
        Vector2 startPosition = StartFishPositionAtBiting;
        Vector2 startPath = startPosition - (Vector2)fishRB.transform.position;
        return startPath;
    }

    public float GetAngleBetweenFishAndFloater()
    {
        Vector3 lookAtFloater = transform.InverseTransformPoint(Floater.gameObject.transform.position);
        float angleBetweenFishAndFloater = Mathf.Atan2(lookAtFloater.y, lookAtFloater.x) * Mathf.Rad2Deg - 90;
        return angleBetweenFishAndFloater;
    }

    public Vector2 GetFloaterPosition()
    {
        return Floater.rigidbody2D.position;
    }

    public Vector2 GetFishPosition()
    {
        return fishRB.transform.position;
    }

    #endregion
    
    #region Trigger Methods

    void OnTriggerEnter2D(Collider2D other)
    {
        Fsm.CurrentFishState.OnTriggerEnter2D(other);   
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Fsm.CurrentFishState.OnTriggerExit2D(other);
    }
    
    #endregion
}
