
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
        Player.playerRB.linearVelocity = Vector2.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Mathf.Abs(InputHandler.GetMoveValue().x) > Mathf.Epsilon)
        {
            Debug.Log(InputHandler.GetMoveValue().x);
            Fsm.ChangeState(Player.MovingState);
        }
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
        if (LayerMask.LayerToName(other.gameObject.layer) == "House")
        {
            //TODO: ADD CHECK IF FISH SOLD
            if (Player.timeManager.hoursValue >= 22)
            {
                Player.InteractionTriggerEvent(Player.InteractionType.Sleep);
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
