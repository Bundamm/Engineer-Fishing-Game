using UnityEngine;
using System.Collections.Generic;

public class Floater : MonoBehaviour, ISurfaceStick, IFloaterColliders
{
    #region Variables
    
    #region Input
    [HideInInspector]
    public InputHandler InputHandler;
    #endregion
    
    public List<Fish> Fishies = new List<Fish>();
    [HideInInspector]
    public Fish randomFish;
    
    public EdgeCollider2D WaterCollider2D { get; private set; }
    public Rigidbody2D FloaterRb { get; private set; }
    public Water Water { get; private set; }
    public Caster Caster { get; private set; }

    [SerializeField]
    private CircleCollider2D approachingCollider;
    [SerializeField]
    private CircleCollider2D bitingCollider;
    
    #endregion
    
    #region Time Manager
    private TimeManager _timeManager;
    #endregion
    
    #region Positions
    [Header("Movement")]
    [HideInInspector]
    public Vector2 CasterPosition { get; set; }
    [HideInInspector]
    public Vector2 FloaterPosition { get; set; }
    
    public float maxTime = 3;
    public float anchorPointPositionMultiplier = 3;
    [HideInInspector]
    public float elapsedTime;

    #endregion
    
    #region State Machine Variables
    public FloaterStateMachine Fsm { get; set; }
    public FloaterLookingForFishState LookingForFishState { get; set; }
    public InitialFloaterState InitialState { get; set; }
    public FloaterChooseAFishState ChooseAFishState { get; set; }
    public FloaterWaitForBitingState  WaitForBitingState { get; set; }
    public FloaterWaitForCaughtState  WaitForCaughtState { get; set; }
    public FloaterReturningState  ReturningState { get; set; }
    #endregion
    
    #region Basic Unity Void Methods
    private void Awake()
    {
        Fsm = new FloaterStateMachine();
        InitialState = new InitialFloaterState(this, Fsm);
        LookingForFishState = new FloaterLookingForFishState(this, Fsm);
        ChooseAFishState = new FloaterChooseAFishState(this, Fsm);
        WaitForBitingState = new FloaterWaitForBitingState(this, Fsm);
        WaitForCaughtState = new FloaterWaitForCaughtState(this, Fsm); 
        ReturningState = new FloaterReturningState(this, Fsm);

        InputHandler = FindAnyObjectByType<InputHandler>();
        Water = FindAnyObjectByType<Water>();
        WaterCollider2D = Water.GetComponent<EdgeCollider2D>();
        FloaterRb = GetComponent<Rigidbody2D>();
        Caster = FindAnyObjectByType<Caster>();
        _timeManager = FindAnyObjectByType<TimeManager>();
    }

    private void Start()
    {
        Fsm.Initialize(InitialState);
    }

    private void Update()
    {
        if (_timeManager.Fsm.IsInState(_timeManager.PausedState)) return;
        Fsm.CurrentFloaterState.FrameUpdate();
    }
    
    private void FixedUpdate()
    {
        if (_timeManager.Fsm.IsInState(_timeManager.PausedState)) return;
        Fsm.CurrentFloaterState.PhysicsUpdate();
    }
    #endregion
    
    #region Trigger Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        Fsm.CurrentFloaterState.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Fsm.CurrentFloaterState.OnTriggerExit2D(other);
    }
    #endregion
    
    #region Collision Methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Fsm.CurrentFloaterState.OnCollisionEnter2D(collision);
    }
    #endregion
    
    #region Surface Sticking
    public void StickToSurface()
    {
        Vector2 closestPoint = WaterCollider2D.ClosestPoint(FloaterRb.position);
        FloaterRb.gravityScale = 0;
        FloaterRb.angularVelocity = 0;
        FloaterRb.MovePosition(Vector2.Lerp(FloaterRb.position, closestPoint, Time.fixedDeltaTime));
    }

    public void UnstickFromSurface()
    {
        FloaterRb.gravityScale = 1;
    }
    #endregion
    
    #region Enable And Disable Circle Colliders
    public void EnableApproachingCollider()
    {
        approachingCollider.enabled = true;
    }

    public void DisableApproachingCollider()
    {
        approachingCollider.enabled = false;
    }

    public void EnableBitingCollider()
    {
        bitingCollider.enabled = true;
    }

    public void DisableBitingCollider()
    {
        bitingCollider.enabled = false;
    }
    #endregion
    
    #region Movement Methods

    public Vector2 QuadraticMovement(Vector2 startPos, Vector2 anchor, Vector2 endPos, float time)
    {
        Vector2 startToAnchor = Vector2.Lerp(startPos, anchor, time);
        Vector2 anchorToEnd = Vector2.Lerp(anchor, endPos, time);
        return Vector2.Lerp(startToAnchor, anchorToEnd, time);
    }
    
    public Vector2 CalculateMidPointBetweenFloaterAndCaster()
    {
        Vector2 midPointPosition = CasterPosition + (FloaterPosition - CasterPosition) / 2;
        Vector2 midPointDirection = CasterPosition - FloaterPosition;
        Vector2 perpendicularToMidPointDirection = new Vector2(midPointDirection.y, -midPointDirection.x).normalized;
        Vector2 pointAboveMidPoint = midPointPosition + (perpendicularToMidPointDirection * anchorPointPositionMultiplier);
        // floaterToCasterAnchor.transform.position = pointAboveMidPoint;
        return pointAboveMidPoint;
    }
    
    #endregion

    #region Reset
    public void ResetAndDestroyFloater()
    {
        Fsm.ChangeState(InitialState);
        Destroy(gameObject);
    }
    #endregion
    
     
}
