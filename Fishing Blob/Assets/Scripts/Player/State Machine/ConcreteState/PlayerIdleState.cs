
using System;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering PlayerIdleState");
        
        Player.playerRb.linearVelocity = Vector2.zero;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        Player.InteractionTriggerEvent(Player.interactionType);
        if (Mathf.Abs(InputHandler.GetMoveValue().x) > Mathf.Epsilon)
        {
            Debug.Log(InputHandler.GetMoveValue().x);
            Fsm.ChangeState(Player.MovingState);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (LayerMask.LayerToName(other.gameObject.layer) == "House")
        {
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
