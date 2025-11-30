using UnityEngine;

public class PlayerState
{
    protected Player Player;
    protected PlayerStateMachine Fsm;
    protected InputHandler InputHandler;

    public PlayerState(Player player, PlayerStateMachine fsm)
    {
        Player = player;
        Fsm = fsm;
        InputHandler = Player.InputHandler;
    }
    
    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}
    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {}
    public virtual void OnTriggerEnter2D(Collider2D other) {}
    public virtual void OnTriggerExit2D(Collider2D other) {}
}
