using UnityEngine;
public class Player : MonoBehaviour
{
    #region Player Variables
    [HideInInspector]
    public Rigidbody2D playerRb;
    public InputHandler inputHandler;
    public InteractionType interactionType;
    public bool CurrentInteractionValue { get; set; }
    public Animator PlayerAnimator { get;  private set; }
    public AudioSource PlayerSource { get;  private set; }
    private bool _isFirstSound = true;
    #endregion
    
    #region State Machine Variables
    public PlayerStateMachine Fsm { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMovingState MovingState { get; private set; }
    public PlayerChargeState ChargeState { get; private set; }
    public PlayerStationaryState StationaryState { get; private set; }
    
    #endregion
    #region Movement Variables
    public float movementSpeed = 10f;
    public float movementSpeedWhileCharging = 1f;
    public float speedMultiplier = 10f;
    public Transform leftLimit, rightLimit;
    public Vector2 playerPosition;
    private Vector2 _playerStartPosition;
    #endregion
    
    #region Other Objects
    [Header("Other Objects")]
    public TimeManager timeManager;
    public MarketUIManager marketUIManager;
    #endregion
    
    private void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        Fsm = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, Fsm);
        MovingState = new PlayerMovingState(this, Fsm);
        ChargeState = new PlayerChargeState(this, Fsm);
        StationaryState = new PlayerStationaryState(this, Fsm);
    }

    private void Start()
    {
        PlayerSource = GetComponent<AudioSource>();
        _playerStartPosition = playerRb.transform.position;
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
        playerPosition = playerRb.transform.position;
    }

    public void MovePlayer(float speed, float moveValue)
    {
        Vector2 movementVector = new Vector2(moveValue * Time.deltaTime * speed* speedMultiplier, 0f);
        playerPosition += movementVector;
        playerPosition.x = Mathf.Clamp(playerPosition.x, leftLimit.position.x, rightLimit.position.x);
        playerRb.transform.position = playerPosition;
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

    public enum InteractionType
    {
        Sleep,
        Market,
        None
    }

    public void InteractionTriggerEvent(InteractionType interactionTypeEnum)
    {
        if (CurrentInteractionValue)
        {
            if (inputHandler.GetInteractValue())
            {
                if (interactionTypeEnum == InteractionType.Sleep && marketUIManager.canInteractWithHouse)
                {
                    Debug.Log("INTERACTION COMPLETED");
                    timeManager.Fsm.ChangeState(timeManager.TransitionDaysState);
                }
                else if (interactionTypeEnum == InteractionType.Market)
                {
                    if (timeManager.HoursValue <= 22)
                    {
                        timeManager.PauseUnpause();
                    }
                    else
                    {
                        Fsm.ChangeState(StationaryState);
                    }
                    
                    timeManager.marketUIManager.ToggleMarketUI();
                }
            }
        }
    }

    public Vector2 GetPlayerStartPosition()
    {
        return _playerStartPosition;
    }

    public void PlayMoveSound()
    {

        AudioManager.SoundType moveSound = _isFirstSound ? 
            AudioManager.SoundType.MoveOne :
            AudioManager.SoundType.MoveTwo;
    
        AudioManager.Instance.PlaySound(moveSound, PlayerSource);
    
        _isFirstSound = !_isFirstSound;
    }
}
