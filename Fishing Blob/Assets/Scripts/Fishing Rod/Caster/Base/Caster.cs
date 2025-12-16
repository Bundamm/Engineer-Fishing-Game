using Unity.Cinemachine;
using UnityEngine;

public class Caster : MonoBehaviour, ICastAndDestroyFloater
{
    #region Variables
    [Header("Casting")]
    public float MaxCastPower { get; set; }
    public float CastPowerIncrease { get; set; }
    public Vector2 CastVector { get; set; }
    public bool ContainsFish { get; set; }
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
    public Floater currentFloaterObject;
    [Header("Player")] [SerializeField] 
    public Transform playerCharacter;
    [Header("Camera")]
    [SerializeField]
    public CameraManager cameraManager;
    [SerializeField]
    public CinemachineCamera playerCamera;
    [SerializeField] 
    public CinemachineCamera floaterCamera;
    [Header("Inventory")]
    public InventoryManager inventory;
    [Header("Time Manager")]
    public TimeManager timeManager;
    #endregion
    
    #region State Machine Variables

    public CasterStateMachine Fsm { get; set; }
    public CasterIdleState IdleState { get; set; }
    public CasterThrowingState ThrowingState { get; set; }
    public CasterWaitingForReturnState WaitingForReturnState { get; set; }
    public CasterCaughtState CaughtState { get; set; }
    public CasterDisabledState DisabledState { get; set; }
    #endregion

    private void Awake()
    {
        Fsm = new CasterStateMachine();
        IdleState = new CasterIdleState(this, Fsm);
        ThrowingState = new CasterThrowingState(this, Fsm);
        WaitingForReturnState = new CasterWaitingForReturnState(this, Fsm);
        CaughtState = new CasterCaughtState(this, Fsm);
        DisabledState = new CasterDisabledState(this, Fsm);
    }

    private void Start()
    {
        Fsm.Initialize(IdleState);
    }

    private void Update()
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        Fsm.CurrentCasterState.FrameUpdate();
    }
    
    public void CreateFloater()
    {
        currentFloater = floaterPrefab;
        currentFloater = Instantiate(currentFloater, transform.position, Quaternion.identity);
        currentFloaterObject = currentFloater.GetComponent<Floater>();
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
