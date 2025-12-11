using UnityEngine;
using System.Collections.Generic;

public class Floater : MonoBehaviour, ISurfaceStick, IFloaterColliders
{
    #region Variables
    
    #region Input
    [HideInInspector]
    public InputHandler inputHandler;
    #endregion
    
    [SerializeField]
    private CircleCollider2D approachingCollider;
    [SerializeField]
    private CircleCollider2D bitingCollider;
    [SerializeField]
    private FishSpawner fishSpawner;
    
    [HideInInspector]
    public List<Fish> fishies;
    [HideInInspector]
    public Fish randomFish;
    public EdgeCollider2D waterCollider2D { get; private set; }
    public Rigidbody2D floaterRb { get; private set; }
    public Water water { get; private set; }
    public Caster caster { get; private set; }

    
    
    #endregion
    
    #region Time Manager
    private TimeManager _timeManager;
    #endregion
    
    #region Positions
    [Header("Movement")]
    public Vector2 casterPosition { get; set; }
    public Vector2 floaterPosition { get; set; }
    
    public float maxTime = 3;
    public float anchorPointPositionMultiplier = 3;
    [HideInInspector]
    public float elapsedTime;

    #endregion
    
    #region State Machine Variables
    public FloaterStateMachine floaterStateMachine { get; set; }
    public FloaterLookingForFishState lookingForFishState { get; set; }
    public InitialFloaterState initialState { get; set; }
    public FloaterChooseAFishState chooseAFishState { get; set; }
    public FloaterWaitForBitingState  waitForBitingState { get; set; }
    public FloaterWaitForCaughtState  waitForCaughtState { get; set; }
    public FloaterReturningState  returningState { get; set; }
    #endregion
    
    #region Basic Unity Void Methods
    private void Awake()
    {
        floaterStateMachine = new FloaterStateMachine();
        initialState = new InitialFloaterState(this, floaterStateMachine);
        lookingForFishState = new FloaterLookingForFishState(this, floaterStateMachine);
        chooseAFishState = new FloaterChooseAFishState(this, floaterStateMachine);
        waitForBitingState = new FloaterWaitForBitingState(this, floaterStateMachine);
        waitForCaughtState = new FloaterWaitForCaughtState(this, floaterStateMachine); 
        returningState = new FloaterReturningState(this, floaterStateMachine);

        fishies = new List<Fish>();
        inputHandler = FindAnyObjectByType<InputHandler>();
        water = FindAnyObjectByType<Water>();
        waterCollider2D = water.GetComponent<EdgeCollider2D>();
        floaterRb = GetComponent<Rigidbody2D>();
        caster = FindAnyObjectByType<Caster>();
        _timeManager = FindAnyObjectByType<TimeManager>();
    }

    private void Start()
    {
        floaterStateMachine.Initialize(initialState);
    }

    private void Update()
    {
        if (_timeManager.Fsm.IsInState(_timeManager.PausedState)) return;
        floaterStateMachine.CurrentFloaterState.FrameUpdate();
    }
    
    private void FixedUpdate()
    {
        if (_timeManager.Fsm.IsInState(_timeManager.PausedState)) return;
        floaterStateMachine.CurrentFloaterState.PhysicsUpdate();
    }
    #endregion
    
    #region Trigger Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        floaterStateMachine.CurrentFloaterState.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        floaterStateMachine.CurrentFloaterState.OnTriggerExit2D(other);
    }
    #endregion
    
    #region Collision Methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        floaterStateMachine.CurrentFloaterState.OnCollisionEnter2D(collision);
    }
    #endregion
    
    #region Surface Sticking
    public void StickToSurface()
    {
        Vector2 closestPoint = waterCollider2D.ClosestPoint(floaterRb.position);
        floaterRb.gravityScale = 0;
        floaterRb.angularVelocity = 0;
        floaterRb.MovePosition(Vector2.Lerp(floaterRb.position, closestPoint, Time.fixedDeltaTime));
    }

    public void UnstickFromSurface()
    {
        floaterRb.gravityScale = 1;
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
        Vector2 midPointPosition = casterPosition + (floaterPosition - casterPosition) / 2;
        Vector2 midPointDirection = casterPosition - floaterPosition;
        Vector2 perpendicularToMidPointDirection = new Vector2(midPointDirection.y, -midPointDirection.x).normalized;
        Vector2 pointAboveMidPoint = midPointPosition + (perpendicularToMidPointDirection * anchorPointPositionMultiplier);
        // floaterToCasterAnchor.transform.position = pointAboveMidPoint;
        return pointAboveMidPoint;
    }
    
    #endregion

    #region Reset
    public void ResetAndDestroyFloater()
    {
        floaterStateMachine.ChangeState(initialState);
        // remove fish from list here
        Destroy(gameObject);
    }
    #endregion
    
     
}
