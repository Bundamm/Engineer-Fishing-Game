using Unity.Cinemachine;
using UnityEngine;

public class Caster : MonoBehaviour, ICastAndDestroyFloater
{
    #region Variables
    [Header("Casting")]
    public float MaxCastPower { get; set; }
    public float CastPowerIncrease { get; set; }
    [HideInInspector] 
    public Vector2 castVector;
    [HideInInspector]
    public bool containsFish;
    #endregion
    
    #region Other Objects
    [Header("Rod")] 
    public Rod Rod;
    [Header("Fishing Line")] 
    public LineSpawner lineSpawner;
    [Header("Catch Trigger")]
    public CircleCollider2D catchTrigger;
    [Header("Floater")]
    [SerializeField]
    private GameObject floaterPrefab;
    [HideInInspector]
    public GameObject currentFloater;
    [HideInInspector] 
    public Floater currentFloaterScript;
    [Header("Player")] [SerializeField] 
    public Transform playerCharacter;
    [Header("Camera")]
    [SerializeField]
    public CameraManager cameraManager;
    [SerializeField]
    public CinemachineCamera playerCamera;
    [SerializeField] 
    public CinemachineCamera floaterCamera;
    #endregion
    
    #region State Machine Variables
    [HideInInspector]
    public CasterStateMachine Fsm;
    [HideInInspector]
    public CasterIdleState IdleState;
    [HideInInspector]
    public CasterThrowingState ThrowingState;
    [HideInInspector]
    public CasterWaitingForReturnState WaitingForReturnState;
    [HideInInspector]
    public CasterCaughtState CaughtState;
    #endregion

    private void Awake()
    {
        Fsm = new CasterStateMachine();
        IdleState = new CasterIdleState(this, Fsm);
        ThrowingState = new CasterThrowingState(this, Fsm);
        WaitingForReturnState = new CasterWaitingForReturnState(this, Fsm);
        CaughtState = new CasterCaughtState(this, Fsm);
    }

    private void Start()
    {
        Fsm.Initialize(IdleState);
    }

    private void Update()
    {
        Fsm.CurrentCasterState.FrameUpdate();
    }
    
    public void CreateFloater()
    {
        currentFloater = floaterPrefab;
        currentFloater = Instantiate(currentFloater, transform.position, Quaternion.identity);
        currentFloaterScript = currentFloater.GetComponent<Floater>();
    }

    public void DestroyFloater()
    {
        Destroy(currentFloater);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fsm.CurrentCasterState.OnTriggerEnter2D(collision);
    }

    
}
