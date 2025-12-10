using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    #region Other Objects
    [Header("Other Objects")]
    public InputHandler inputHandler;
    public Volume timeVolume;
    public GameObject localLights;
    public Rod fishingRod;
    public MarketManager marketManager;
    public MarketUIManager marketUIManager;
    #endregion
    
    #region Transition Elements
    [Header("Transition Elements")]
    public Image transitionFadeImage;
    public Canvas gameUICanvas;
    public TextMeshProUGUI dayTransitionText;
    public TextMeshProUGUI moneyEarnedTransitionText;
    
    public float transitionTick = 0.05f;
    #endregion
    
    #region PauseElements 
    //TODO: IMPORTANT ADD PAUSE UI
    [Header("Pause Elements")]
    public Canvas pauseCanvas;
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
    public int dayCounterValue = 1;
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
        marketManager.SetStartingValues();
        marketUIManager.UpdateValueTexts();
        Fsm.Initialize(DayStartState);
    }
    
    private void Update()
    {
        Fsm.CurrentTimeState.FrameUpdate();
        
    }
    
    public void PauseUnpause()
    {
        timerPaused = !timerPaused;
    }
}
