
using UnityEngine;

public class PlayerDisableMovementState : PlayerState
{
    public PlayerDisableMovementState(Player player, PlayerStateMachine fsm) : base(player, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.PlayerAnimator.SetBool("isMoving", false);
        Debug.Log("Entered PlayerDisableMovementState");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if (Player.interactionType == Player.InteractionType.Market)
        {
            if (Player.timeManager.HoursValue >= 22)
            {
                if (Player.inputHandler.GetPauseValue())
                {
                    Fsm.ChangeState(Player.IdleState);
                    Player.timeManager.marketUIManager.ToggleMarketUI();
                }
                if (!Player.timeManager.TimerPaused)
                {
                    Fsm.ChangeState(Player.IdleState);
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
}
