using System.IO.Enumeration;
using UnityEngine;
public class PlayerMovingState : PlayerState
{
    private Vector2 playerPosition;
    public PlayerMovingState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        playerPosition = Player.playerRB.transform.position;
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
            Vector2 movementVector = new Vector2(InputHandler.GetMoveValue().x * Time.deltaTime * Player.movementSpeed * Player.speedMultiplier, 0f);
            playerPosition += movementVector;
            playerPosition.x = Mathf.Clamp(playerPosition.x, Player.leftLimit.position.x, Player.rightLimit.position.x);
            Player.playerRB.transform.position = playerPosition;
            if (InputHandler.GetMoveValue().x != 0)
            {
                Player.transform.localScale = new Vector2(InputHandler.GetMoveValue().x * 0.8f, 0.8f);
            }
        }
        else
        {
            Fsm.ChangeState(Player.idleState);
        }
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
