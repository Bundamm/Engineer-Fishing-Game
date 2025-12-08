using System.Collections;

using UnityEngine;

public class Rod : MonoBehaviour
{
    #region DO PRZEROBIENIA
    [Header("Scripts")]
    [SerializeField]
    public InputHandler InputHandler;
    [SerializeField]
    public RodRotator RodRotator;

    [Header("Casting")]
    [SerializeField]
    private float maxCastPower = 400f;
    [SerializeField] 
    private float castPowerIncrease = 5f;
    public Caster Caster;
    [HideInInspector]
    public float castPower;

    [Header("Rod Rotation")] 
    public Quaternion targetRotation;
    
    public float chargeRotationSpeed = 1;
    
    public float castRotationSpeed = 2;

    [Header("Player")] 
    public Transform playerTransform;

    public Player playerObject;
    
    [Header("Time Manager")]
    public TimeManager timeManager;
    
    #endregion
    
    #region State Machine Variables
    public RodStateMachine Fsm { get; set; }
    public RodIdleState IdleState { get; set; }
    public RodChargingState ChargingState { get; set; }

    public RodRotatingToThrowState CastingState { get; set; }
    public RodThrowAndWait ThrowAndWait { get; set; }
    
    public RodDisabledState DisabledState { get; set; }

    #endregion

    private void Awake()
    {
        Fsm = new RodStateMachine();
        
        IdleState = new RodIdleState(this, Fsm);
        ChargingState = new RodChargingState(this, Fsm);
        CastingState = new RodRotatingToThrowState(this, Fsm);
        ThrowAndWait = new RodThrowAndWait(this, Fsm);
        DisabledState = new RodDisabledState(this, Fsm);
        
        playerObject = playerTransform.GetComponent<Player>();
    }

    private void Start()
    {
        Fsm.Initialize(IdleState);
    }
    
    private void Update()
    {
        if (timeManager.Fsm.IsInState(timeManager.PausedState)) return;
        
        Fsm.CurrentRodState.FrameUpdate();
        
    }

    public void CheckPlayerFacingDirection()
    {
        float rotationMagnitude = Mathf.Abs(targetRotation.z);
        if (playerTransform.localScale.x < 0f)
        {
            targetRotation.z = -rotationMagnitude;
        }
        else
        {
            targetRotation.z = Mathf.Abs(rotationMagnitude);
        }
    }
     

    #region Public Methods
    public void ResetCast()
    {
        RodRotator.SetIsRotated(false);
        RodRotator.transform.rotation = RodRotator.GetStartRotation();
    }
    
    public void CalculateCastPower()
    {
        if (castPower <= maxCastPower)
        {
            castPower += castPowerIncrease * Time.deltaTime;
        }
    }

    public void CasterThrowFloater()
    {
        Debug.Log("Throwing Floater");
        Debug.Log(Caster);
        if (Caster.Fsm.IsInState(Caster.IdleState))
        {
            Caster.Fsm.ChangeState(Caster.ThrowingState);
        }
    }
    #endregion
    
}
