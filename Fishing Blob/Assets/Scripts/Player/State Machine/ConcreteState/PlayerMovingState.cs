using System.IO.Enumeration;
using UnityEngine;
public class PlayerMovingState : PlayerState
{
    public PlayerMovingState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        Debug.Log("Entering PlayerMovingState");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Player.PlayerAnimator.SetTrigger("Move");
        Player.InteractionTriggerEvent(Player.interactionType);
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (Mathf.Abs(InputHandler.GetMoveValue().x) > Mathf.Epsilon)
        {
            Player.MovePlayer(Player.movementSpeed, InputHandler.GetMoveValue().x);
        }
        else
        {
            Fsm.ChangeState(Player.IdleState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "House")
        {
            //TODO: ADD CHECK IF FISH SOLD
            Player.interactionType = Player.InteractionType.Sleep;
            if (Player.timeManager.HoursValue >= 22)
            {
                Player.CurrentInteractionValue = true;
            }
        }
        else if (LayerMask.LayerToName(other.gameObject.layer) == "Market")
        {
            Player.interactionType = Player.InteractionType.Market;
            Player.CurrentInteractionValue = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (LayerMask.LayerToName(other.gameObject.layer) == "House" || LayerMask.LayerToName(other.gameObject.layer) == "Market")
        {
            Player.CurrentInteractionValue = false;
            Player.interactionType = Player.InteractionType.None;
        }
    }
}
