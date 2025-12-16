using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    #region Other Objects
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField] 
    private TimeManager timeManager;
    [SerializeField]
    private SaveSystem saveSystem;
    #endregion
    
    #region Scroll Views
    [SerializeField]
    private GameObject controlsPanel;
    [SerializeField]
    private GameObject creditsPanel;
    #endregion
    
    #region Text
    [SerializeField]
    private TextMeshProUGUI highscoreText;
    [SerializeField]
    private TextMeshProUGUI mostMoneyOwnedText;
    [SerializeField] 
    private TextMeshProUGUI mostDaysCompletedText;
    #endregion
    
    #region Buttons
    [SerializeField] 
    private Button startButton;
    [SerializeField] 
    private Button controlsButton;
    [SerializeField] 
    private Button creditsButton;
    [SerializeField] 
    private Button exitButton;
    [SerializeField]
    private Button backToMainMenuButton;
    [SerializeField] 
    private Button audioButton;
    [SerializeField] 
    private Button resumeButton;
    #endregion

    #region Sprites
    [SerializeField] 
    private Sprite[] buttonSprites;
    [SerializeField] 
    private Sprite[] toggleSprites;
    #endregion
    
    #region Helper Variables
    private bool _audioEnabled;
    #endregion
    
    private void Start()
    {
        _audioEnabled = AudioListener.volume > 0;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            highscoreText.text = $"Highscore: {saveSystem.HighScore}";
            mostMoneyOwnedText.text = $"Most Money Owned: {saveSystem.MoneyGainedOverallHighScore}$";
            mostDaysCompletedText.text = $"Most Days Completed: {saveSystem.HighScoreDays}";
        }
    }

    #region ButtonWrappers

    public void StartButtonWrapper()
    {
        StartCoroutine(ToggleStartButton());
    }

    public void ControlsButtonWrapper()
    {
        StartCoroutine(ToggleControlsButton());
    }

    public void CreditsButtonWrapper()
    {
        StartCoroutine(ToggleCreditsButton());
    }

    public void ExitButtonWrapper()
    {
        StartCoroutine(ToggleExitButton());
    }

    public void ResumeButtonWrapper()
    {
        StartCoroutine(ToggleResumeButton());
    }

    public void BackToMainMenuButtonWrapper()
    {
        StartCoroutine(ToggleBackToMainMenuButton());
    }
    #endregion
    
    
    private IEnumerator ToggleStartButton()
    {
        startButton.image.sprite = buttonSprites[1];
        yield return new WaitForSecondsRealtime(0.1f);
        startButton.image.sprite = buttonSprites[0];
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.UIButtonPop, AudioManager.Instance.ManagerSource);
        StartGame();
    }

    private IEnumerator ToggleControlsButton()
    {
        controlsButton.image.sprite = buttonSprites[1];
        yield return new WaitForSecondsRealtime(0.1f);
        controlsButton.image.sprite = buttonSprites[0];
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.UIButtonPop2, AudioManager.Instance.ManagerSource);
        ToggleControlsMenu();
    }

    

    private IEnumerator ToggleCreditsButton()
    {
        creditsButton.image.sprite = buttonSprites[1];
        yield return new WaitForSecondsRealtime(0.1f);
        creditsButton.image.sprite = buttonSprites[0];
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.UIButtonPop2, AudioManager.Instance.ManagerSource);
        ToggleCreditsMenu();
    }

    private IEnumerator ToggleExitButton()
    {
        exitButton.image.sprite = buttonSprites[1];
        yield return new WaitForSecondsRealtime(0.1f);
        exitButton.image.sprite = buttonSprites[0];
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.UIButtonPop, AudioManager.Instance.ManagerSource);
        ToggleExit();
    }

    private IEnumerator ToggleResumeButton()
    {
        resumeButton.image.sprite = buttonSprites[1];
        yield return new WaitForSecondsRealtime(0.1f);
        resumeButton.image.sprite = buttonSprites[0];
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.Unpause, AudioManager.Instance.ManagerSource);
        timeManager.PauseUnpause();
    }
    
    public void ToggleAudioButton()
    {
        _audioEnabled = !_audioEnabled;
        AudioManager.Instance.MuteSounds(_audioEnabled);
        
        audioButton.image.sprite = _audioEnabled ? toggleSprites[0] : toggleSprites[1];
        if(_audioEnabled) AudioManager.Instance.PlaySound(AudioManager.SoundType.UIButtonPop, AudioManager.Instance.ManagerSource);
        
    }
    
    private IEnumerator ToggleBackToMainMenuButton()
    {
        backToMainMenuButton.image.sprite = buttonSprites[1];
        yield return new WaitForSecondsRealtime(0.1f);
        backToMainMenuButton.image.sprite = buttonSprites[0];
        yield return new WaitForSecondsRealtime(0.1f);
        AudioManager.Instance.PlaySound(AudioManager.SoundType.UIButtonPop, AudioManager.Instance.ManagerSource);
        BackToMainMenu();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    private void ToggleControlsMenu()
    {
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    
    private void ToggleCreditsMenu()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    
    private void ToggleExit()
    {
        Application.Quit();
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
}
