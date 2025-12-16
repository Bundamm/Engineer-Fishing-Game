using UnityEngine;
public class PlayerChargeState : PlayerState
{
    public PlayerChargeState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.PlayerAnimator.SetBool("isCharging", true);
        Debug.Log("Entering PlayerPrepareState");
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.PlayerAnimator.SetBool("isCharging", false);
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
            Player.MovePlayer(Player.movementSpeedWhileCharging, InputHandler.GetMoveValue().x);
        }
    }
}
 