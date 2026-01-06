using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Floater : MonoBehaviour, ISurfaceStick, IFloaterColliders
{
    #region Variables
    
    #region Input
    [HideInInspector]
    public InputHandler inputHandler;
    #endregion
    
    #region Other Objects
    [SerializeField]
    private CircleCollider2D approachingCollider;
    [SerializeField]
    private CircleCollider2D bitingCollider;
    private FishSpawner _fishSpawner;
    #endregion
    
    
    [HideInInspector]
    public List<Fish> fishies;
    [HideInInspector]
    public Fish randomFish;
    public AudioSource FloaterSource { get; private set; }
    public EdgeCollider2D WaterCollider2D { get; private set; }
    public Rigidbody2D FloaterRb { get; private set; }
    public Water Water { get; private set; }
    public Caster Caster { get; private set; }

    
    
    #endregion
    
    #region Time Manager And Light
    [HideInInspector]
    public TimeManager timeManager;
    private Light2D _floaterLight;
    #endregion
    
    #region Positions
    [Header("Movement")]
    public Vector2 CasterPosition { get; set; }
    public Vector2 FloaterPosition { get; set; }
    
    public float maxTime = 3;
    public float anchorPointPositionMultiplier = 3;
    [HideInInspector]
    public float elapsedTime;

    #endregion
    
    #region State Machine Variables
    public FloaterStateMachine FloaterStateMachine { get; set; }
    public FloaterLookingForFishState LookingForFishState { get; set; }
    public FloaterInitialState FloaterInitialState { get; set; }
    public FloaterChooseAFishState ChooseAFishState { get; set; }
    public FloaterWaitForBitingState  WaitForBitingState { get; set; }
    public FloaterWaitForCaughtState  WaitForCaughtState { get; set; }
    public FloaterReturningState  ReturningState { get; set; }
    #endregion
    
    #region Basic Unity Methods
    private void Awake()
    {
        FloaterStateMachine = new FloaterStateMachine();
        FloaterInitialState = new FloaterInitialState(this, FloaterStateMachine);
        LookingForFishState = new FloaterLookingForFishState(this, FloaterStateMachine);
        ChooseAFishState = new FloaterChooseAFishState(this, FloaterStateMachine);
        WaitForBitingState = new FloaterWaitForBitingState(this, FloaterStateMachine);
        WaitForCaughtState = new FloaterWaitForCaughtState(this, FloaterStateMachine); 
        ReturningState = new FloaterReturningState(this, FloaterStateMachine);

        fishies = new List<Fish>();
        inputHandler = FindAnyObjectByType<InputHandler>();
        Water = FindAnyObjectByType<Water>();
        WaterCollider2D = Water.GetComponent<EdgeCollider2D>();
        FloaterRb = GetComponent<Rigidbody2D>();
        _floaterLight = GetComponent<Light2D>();
        Caster = FindAnyObjectByType<Caster>();
        timeManager = FindAnyObjectByType<TimeManager>();
    }

    private void Start()
    {
        FloaterStateMachine.Initialize(FloaterInitialState);
        FloaterSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        FloaterStateMachine.CurrentFloaterState.FrameUpdate();
        if (timeManager.HoursValue >= 18)
        {
            _floaterLight.enabled = true;
        }
        else
        {
            _floaterLight.enabled = false;
        }
    }
    
    private void FixedUpdate()
    {

        FloaterStateMachine.CurrentFloaterState.PhysicsUpdate();
    }
    #endregion
    
    #region Trigger Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        FloaterStateMachine.CurrentFloaterState.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        FloaterStateMachine.CurrentFloaterState.OnTriggerExit2D(other);
    }
    #endregion
    
    #region Collision Methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FloaterStateMachine.CurrentFloaterState.OnCollisionEnter2D(collision);
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
        FloaterStateMachine.ChangeState(FloaterInitialState);
        // remove fish from list here
        Destroy(gameObject);
    }
    #endregion
    
}
