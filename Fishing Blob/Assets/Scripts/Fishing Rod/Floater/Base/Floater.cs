using UnityEngine;
using System.Collections.Generic;

public class Floater : MonoBehaviour, ISurfaceStick, IFloaterColliders
{
    #region Variables
    
    #region Input
    [HideInInspector]
    public InputHandler InputHandler;
    #endregion

    public List<Fish> Fishes = new List<Fish>();
    public Fish randomFish;
    
    public EdgeCollider2D waterCollider2D { get; private set; }
    public Rigidbody2D rigidbody2D { get; private set; }
    public Water Water { get; private set; }
    public Caster Caster { get; }

    [SerializeField]
    private CircleCollider2D approachingCollider;
    [SerializeField]
    private CircleCollider2D bitingCollider;
    
    
    #endregion
    
    #region State Machine Variables
    public FloaterStateMachine Fsm { get; set; }
    public FloaterLookingForFishState LookingForFishState { get; set; }
    public InitialFloaterState InitialState { get; set; }
    public FloaterChooseAFishState ChooseAFishState { get; set; }
    public FloaterWaitForBitingState  WaitForBitingState { get; set; }
    public FloaterWaitForCaughtState  WaitForCaughtState { get; set; }
    public FloaterCaughtState  CaughtState { get; set; }
    #endregion
    #region Basic Unity Void Methods
    private void Awake()
    {
        Fsm = new FloaterStateMachine();
        InitialState = new InitialFloaterState(this, Fsm);
        LookingForFishState = new FloaterLookingForFishState(this, Fsm);
        ChooseAFishState = new FloaterChooseAFishState(this, Fsm);
        WaitForBitingState = new FloaterWaitForBitingState(this, Fsm);
        WaitForCaughtState = new FloaterWaitForCaughtState(this, Fsm);
        CaughtState = new FloaterCaughtState(this, Fsm);

        InputHandler = FindAnyObjectByType<InputHandler>();
        Water = FindAnyObjectByType<Water>();
        waterCollider2D = Water.GetComponent<EdgeCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Fsm.Initialize(InitialState);
    }

    private void Update()
    {
        Fsm.CurrentFloaterState.FrameUpdate();
    }
    
    private void FixedUpdate()
    {
        Fsm.CurrentFloaterState.PhysicsUpdate();
    }
    #endregion
    #region Trigger Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        Fsm.CurrentFloaterState.OnTriggerEnter2D(other);

 
        //         int multiplier = 1;
        //         if (rigidbody2D.linearVelocity.y < 0)
        //         {
        //             multiplier = -1;
        //         }
        //         else
        //         {
        //             multiplier = 1;
        //         }
        //         vel *= multiplier;
        //         
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Fsm.CurrentFloaterState.OnTriggerExit2D(other);
    }
    #endregion
    
    #region Surface Sticking
    public void StickToSurface()
    {
        Vector2 closestPoint = waterCollider2D.ClosestPoint(rigidbody2D.position);
        rigidbody2D.gravityScale = 0;
        rigidbody2D.angularVelocity = 0;
        rigidbody2D.MovePosition(Vector2.Lerp(rigidbody2D.position, closestPoint, Time.fixedDeltaTime));
    }

    public void UnstickFromSurface()
    {
        rigidbody2D.gravityScale = 1;
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
}
