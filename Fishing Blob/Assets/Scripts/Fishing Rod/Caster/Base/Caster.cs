using UnityEngine;

public class Caster : MonoBehaviour, ICastAndDestroyFloater
{
    #region Variables
    [Header("Floater")]
    [SerializeField]
    private GameObject floaterPrefab;
    [HideInInspector]
    public GameObject currentFloater;
    [Header("Casting")]
    public float MaxCastPower { get; set; }
    public float CastPowerIncrease { get; set; }
    [HideInInspector] 
    public Vector2 castVector;
    [Header("Rod")] 
    public Rod rod;
    [Header("Fishing Line")] 
    public LineSpawner lineSpawner;
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
    #endregion

    private void Awake()
    {
        Fsm = new CasterStateMachine();
        IdleState = new CasterIdleState(this, Fsm);
        ThrowingState = new CasterThrowingState(this, Fsm);
        WaitingForReturnState = new CasterWaitingForReturnState(this, Fsm);

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
    }

    public void DestroyFloater()
    {
        Destroy(gameObject);
    }

    
}
