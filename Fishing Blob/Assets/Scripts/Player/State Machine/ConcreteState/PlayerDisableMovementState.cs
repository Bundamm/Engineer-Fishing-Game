
using UnityEngine;

public class PlayerDisableMovementState : PlayerState
{
    public PlayerDisableMovementState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered PlayerDisableMovementState");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //TODO: ADD QUITTING FROM MARKET WINDOW
        if (Player.interactionType == Player.InteractionType.Market)
        {
            Fsm.ChangeState(Player.IdleState);
        }
        if (Player.timeManager.Fsm.IsInState(Player.timeManager.DayStartState))
        {
            Fsm.ChangeState(Player.IdleState);
        }
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
