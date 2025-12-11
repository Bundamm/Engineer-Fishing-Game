
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

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (Player.interactionType == Player.InteractionType.Market)
        {
            if (Player.timeManager.hoursValue >= 22)
            {
                if (Player.InputHandler.GetPauseValue())
                {
                    Fsm.ChangeState(Player.IdleState);
                    Player.timeManager.marketUIManager.ToggleMarketUI();
                }
            }
            else
            {
                Fsm.ChangeState(Player.IdleState);
            }
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
