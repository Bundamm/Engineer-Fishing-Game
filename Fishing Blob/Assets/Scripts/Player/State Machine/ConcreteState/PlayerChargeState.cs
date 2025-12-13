using UnityEngine;
public class PlayerChargeState : PlayerState
{
    public PlayerChargeState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        Debug.Log("Entering PlayerPrepareState");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Player.PlayerAnimator.SetTrigger("Charge");
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
 