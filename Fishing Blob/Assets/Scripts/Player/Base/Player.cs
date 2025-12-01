using UnityEngine;
public class Player : MonoBehaviour
{
    #region Player Variables
    [HideInInspector]
    public Rigidbody2D playerRB;
    [HideInInspector]
    public CircleCollider2D playerCollider;
    public InputHandler InputHandler;
    public float movementSpeed = 10f;
    public float speedMultiplier = 10f;
    #endregion
    
    #region State Machine Variables
    public PlayerStateMachine Fsm { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMovingState movingState { get; private set; }
    public PlayerPrepareState prepareState { get; private set; }
    public PlayerDisableMovementState DisableMovementState { get; private set; }
    
    #endregion

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        Fsm = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, Fsm);
        movingState = new PlayerMovingState(this, Fsm);
        prepareState = new PlayerPrepareState(this, Fsm);
        DisableMovementState = new PlayerDisableMovementState(this, Fsm);
    }

    private void Start()
    {
        Fsm.Initialize(idleState);
    }

    private void Update()
    {
        Fsm.CurrentPlayerState.FrameUpdate();
    }
    
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        Fsm.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        Example
    }
}
