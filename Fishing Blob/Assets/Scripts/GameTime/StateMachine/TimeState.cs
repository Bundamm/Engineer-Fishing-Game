
public class TimeState
{
    protected TimeManager TimeManager;
    protected TimeStateMachine Fsm;
    protected InputHandler InputHandler;

    public TimeState(TimeManager timeManager, TimeStateMachine fsm)
    {
        TimeManager = timeManager;
        Fsm = fsm;
        InputHandler = TimeManager.inputHandler;
    }
    
    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void FrameUpdate() {}

}
