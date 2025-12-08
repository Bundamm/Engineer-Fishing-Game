using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    #region Other Objects
    [Header("Other Objects")]
    public InputHandler inputHandler;
    public Volume timeVolume;
    public GameObject localLights;
    public Rod fishingRod;
    #endregion
    
    #region Other Objects Values
    [Header("Other Objects Values")]
    public float volumeWeight;
    public float volumeWeightTick = 0.00001f;
    
    #endregion
    
    #region TimerValues
    [Header("Timer Values")]
    public int tickValue = 10;

    public int timeOfNoon = 9;
    public int timeOfAfternoon = 14;
    
    [HideInInspector]
    public bool timerPaused;
    [HideInInspector]
    public int dayCounterValue;
    [HideInInspector]
    public decimal minutesValue;
    [HideInInspector]
    public int hoursValue;
    public TextMeshProUGUI dayCounterText;
    public TextMeshProUGUI timeText;
    #endregion
    
    #region State Machine Variables

    public TimeStateMachine Fsm { get; private set; }
    public TimeDayActiveState DayActiveState { get; private set; }
    public TimePausedState PausedState { get; private set; }
    public TimeNightStoppedState NightStoppedState { get; private set; }
    public TimeDayStartState DayStartState { get; private set; }
    
    public TimeTransitionDaysState TransitionDaysState { get; private set; }
    #endregion

    private void Awake()
    {
        Fsm = new TimeStateMachine();
        DayStartState = new TimeDayStartState(this, Fsm);
        DayActiveState = new TimeDayActiveState(this, Fsm);
        PausedState = new TimePausedState(this, Fsm);
        NightStoppedState = new TimeNightStoppedState(this, Fsm);
        TransitionDaysState = new TimeTransitionDaysState(this, Fsm);
        
    }
    
    private void Start()
    {
        Fsm.Initialize(DayStartState);
    }
    
    private void Update()
    {
        Fsm.CurrentTimeState.FrameUpdate();
    }
    
    public void PauseUnpause()
    {
        if (inputHandler.GetPauseValue())
        {
            timerPaused = !timerPaused;
        }
    }
}
