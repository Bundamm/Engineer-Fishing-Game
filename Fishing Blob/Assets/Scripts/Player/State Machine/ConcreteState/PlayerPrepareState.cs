using UnityEngine;
public class PlayerPrepareState : PlayerState
{
    private Vector2 playerPosition;
    public PlayerPrepareState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        playerPosition = Player.playerRB.transform.position;
        Debug.Log("Entering PlayerPrepareState");
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
            Player.MovePlayer(Player.movementSpeedWhileCharging, InputHandler.GetMoveValue().x);
        }
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
 