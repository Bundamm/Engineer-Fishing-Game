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
