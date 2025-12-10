using UnityEngine;
public class Player : MonoBehaviour
{
    #region Player Variables
    [HideInInspector]
    public Rigidbody2D playerRB;
    [HideInInspector]
    public CircleCollider2D playerCollider;
    public InputHandler InputHandler;
    public bool currentInteractionValue;
    public InteractionType interactionType;
    #endregion
    
    #region State Machine Variables
    public PlayerStateMachine Fsm { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMovingState MovingState { get; private set; }
    public PlayerPrepareState PrepareState { get; private set; }
    public PlayerDisableMovementState DisableMovementState { get; private set; }
    
    #endregion
    
    #region Movement Variables
    public float movementSpeed = 10f;
    public float movementSpeedWhileCharging = 1f;
    public float speedMultiplier = 10f;
    public Transform leftLimit, rightLimit;
    public Vector2 playerPosition;
    #endregion
    
    #region Time Manager
    [Header("Time Manager")]
    public TimeManager timeManager;
    #endregion
    
    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        Fsm = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, Fsm);
        MovingState = new PlayerMovingState(this, Fsm);
        PrepareState = new PlayerPrepareState(this, Fsm);
        DisableMovementState = new PlayerDisableMovementState(this, Fsm);
    }

    private void Start()
    {
        Fsm.Initialize(IdleState);
    }

    private void Update()
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        Fsm.CurrentPlayerState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        Fsm.CurrentPlayerState.PhysicsUpdate();
        playerPosition = playerRB.transform.position;
    }
    
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        Fsm.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public void MovePlayer(float speed, float moveValue)
    {
        Vector2 movementVector = new Vector2(moveValue * Time.deltaTime * speed* speedMultiplier, 0f);
        playerPosition += movementVector;
        playerPosition.x = Mathf.Clamp(playerPosition.x, leftLimit.position.x, rightLimit.position.x);
        playerRB.transform.position = playerPosition;
        if (moveValue != 0)
        {
            transform.localScale = new Vector2(moveValue * 0.8f, 0.8f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        Fsm.CurrentPlayerState.OnTriggerEnter2D(collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        Fsm.CurrentPlayerState.OnTriggerExit2D(collision);
    }
    
    public enum AnimationTriggerType
    {
        Example
    }

    public enum InteractionType
    {
        Sleep,
        Market,
        None
    }

    public void InteractionTriggerEvent(InteractionType interactionTypeEnum)
    {
        if (currentInteractionValue)
        {
            if (InputHandler.GetInteractValue())
            {
                Fsm.ChangeState(DisableMovementState);
            
                if (interactionTypeEnum == InteractionType.Sleep)
                {
                    Debug.Log("INTERACTION COMPLETED");
                    timeManager.Fsm.ChangeState(timeManager.TransitionDaysState);
                }
                else if (interactionTypeEnum == InteractionType.Market)
                {
                    //TODO: IMPLEMENT MARKET INTERACTION
                    timeManager.PauseUnpause();
                    timeManager.marketUIManager.ToggleMarketUI();
                }
            }
        }
    }
}
