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
    
    public Caster caster;
    [HideInInspector]
    public float castPower;

    [Header("Rod Rotation")] 
    public Quaternion targetRotation;
    
    public float chargeRotationSpeed = 1;
    
    public float castRotationSpeed = 2;

    
    #endregion
    
    #region State Machine Variables
    
    public RodStateMachine Fsm { get; set; }
    public RodIdleState IdleState { get; set; }
    public RodChargingState ChargingState { get; set; }

    public RodRotatingToThrowState CastingState { get; set; }
    public RodThrowAndWait ThrowAndWait { get; set; }

    #endregion

    private void Awake()
    {
        Fsm = new RodStateMachine();
        
        IdleState = new RodIdleState(this, Fsm);
        ChargingState = new RodChargingState(this, Fsm);
        CastingState = new RodRotatingToThrowState(this, Fsm);
        ThrowAndWait = new RodThrowAndWait(this, Fsm);
    }

    private void Start()
    {
        Fsm.Initialize(IdleState);
    }
    
    private void Update()
    {
        Fsm.CurrentRodState.FrameUpdate();
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
        Debug.Log(caster);
        if (caster.Fsm.IsInState(caster.IdleState))
        {
            caster.Fsm.ChangeState(caster.ThrowingState);
        }
    }
    #endregion
    
}
