using System.Collections.Generic;
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
    public FishSpawner fishSpawner;
    public Rod fishingRod;
    public MarketManager marketManager;
    public MarketUIManager marketUIManager;
    public Player player;
    public SaveSystem saveSystem;
    public TextMeshProUGUI marketIndicator;
    public TextMeshProUGUI houseIndicator;
    #endregion
    
    #region Lights
    [SerializeField]
    private GameObject staticLights;
    [SerializeField]
    private Light2D playerLampLight;
    [SerializeField]
    private List<Animator> lamps; 
    #endregion
    
    #region Transition Elements
    [Header("Transition Elements")]
    public Image transitionFadeImage;
    public Canvas gameUICanvas;
    public TextMeshProUGUI dayTransitionText;
    public TextMeshProUGUI moneyEarnedTransitionText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalDayReachedText;
    public TextMeshProUGUI newHighscoreText;
    public TextMeshProUGUI buttonPromptsText;
    public TextMeshProUGUI scoreText;
    public bool isNewHighscore;
    
    public float transitionTick = 0.05f;
    #endregion
    
    #region PauseElements 
    [Header("Pause Elements")]
    public Canvas pauseCanvas;
    #endregion
    
    #region Other Objects Values
    [Header("Other Objects Values")]
    public float volumeWeight;
    public float volumeWeightTick = 0.00001f;
    #endregion
    
    #region TimerValues

    [field: Header("Timer Values")] 
    [field: SerializeField]
    public int TickValue { get; set; } = 10;
    [field: SerializeField]
    public int TimeOfNoon { get; set; } = 9;
    [field: SerializeField]
    public int TimeOfAfternoon { get; set; } = 14;
    public bool TimerPaused { get; set; }
    public int DayCounterValue { get; set; } = 1;
    public int HighScoreDayValue { get; set; } = 1;
    public decimal MinutesValue { get; set; }
    public int HoursValue { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI DayCounterText { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI TimeText { get; set; }

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
        HighScoreDayValue = saveSystem.HighScoreDays;
        marketManager.SetStartingValues();
        marketUIManager.UpdateValueTexts();
        marketUIManager.UpdateRentValueText();
        Fsm.Initialize(DayStartState);
    }
    
    private void Update()
    {
        Fsm.CurrentTimeState.FrameUpdate();
        if (HoursValue >= 18)
        {
            foreach (Animator lamp in lamps)
            {
                lamp.SetBool("IsOn", true);
            }
            staticLights.SetActive(true);
            playerLampLight.enabled = true;
        }
        else
        {
            foreach (Animator lamp in lamps)
            {
                lamp.SetBool("IsOn", false);
            }
            staticLights.SetActive(false);
            playerLampLight.enabled = false;
        }

    }
    
    public void PauseUnpause()
    {
        TimerPaused = !TimerPaused;
    }
    
}
