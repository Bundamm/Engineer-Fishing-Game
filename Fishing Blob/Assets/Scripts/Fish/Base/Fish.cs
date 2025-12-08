using UnityEngine;

public class Fish : MonoBehaviour, IFishMovable, IFishAndFloaterPositionAndRotation
{
    #region Fish Variables

    public Rigidbody2D fishRB {get; set;}
    public FishTypes fishType;
    [SerializeField]
    private CircleCollider2D fishFaceCollider;
    #endregion

    #region Water Variables
    private Water _water;
    [HideInInspector]
    public float waterHeight;
    [HideInInspector]
    public float waterWidth;
    [HideInInspector]
    public float waterStartPosX;
    [HideInInspector]
    public float waterStartPosY;
    #endregion
    
    #region Fish Approaching Variables
    public Floater Floater { get; set; }
    [HideInInspector]
    public Vector2 StartFishPositionAtBiting { get; set; }
    #endregion
    
    #region Time Manager
    private TimeManager _timeManager;
    #endregion
    #region State Machine Variables
    public FishStateMachine Fsm { get; private set; }
    public FishIdleState IdleState { get; private set; }
    public FishApproachingState ApproachingState { get; private set; }
    public FishBitingState BitingState { get; private set; }
    public FishCaughtState CaughtState { get; private set; }
    public FishSpookedState SpookedState { get; private set; }
    public FishHookedState HookedState { get; private set; }
    #endregion
    
    #region Basic Unity Methods
    
    private void Awake()
    {
        _water = FindAnyObjectByType<Water>();
        _timeManager = FindAnyObjectByType<TimeManager>();
        Fsm = new FishStateMachine();

        IdleState = new FishIdleState(this, Fsm);
        ApproachingState = new FishApproachingState(this, Fsm);
        BitingState = new FishBitingState(this, Fsm);
        CaughtState = new FishCaughtState(this, Fsm);
        SpookedState = new FishSpookedState(this, Fsm);
        HookedState = new FishHookedState(this, Fsm);

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
        if (_timeManager.Fsm.IsInState(_timeManager.PausedState))
        {
            fishRB.linearVelocity = Vector2.zero;
            return;
        }
        waterHeight = _water.GetMeshHeight();
        waterWidth = _water.GetMeshWidth();
        Fsm.CurrentFishState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        if (_timeManager.Fsm.IsInState(_timeManager.PausedState)) return;
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

    public void StickToFloater()
    {
        transform.SetParent(Floater.transform);
        fishRB.angularVelocity = 0f;
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
    
    #region Reset

    public void ResetAndDestroyFish()
    {
        Fsm.ChangeState(IdleState);
        Destroy(gameObject);
    }
    
    #endregion
}
