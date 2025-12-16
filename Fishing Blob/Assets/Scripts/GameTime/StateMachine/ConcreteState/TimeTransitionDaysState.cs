
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTransitionDaysState : TimeState
{
    private bool fadingIn;
    private bool fadingOut;

    private bool trackEscInput;
    private bool trackLmbInput;

    private bool enableChecks;
    

    private float alphaTransitionValue;
    
    public TimeTransitionDaysState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered TimeTransitionDaysState");
        enableChecks = false;
        fadingIn = true;
        fadingOut = false;
        alphaTransitionValue = 0f;
        TimeManager.DayCounterValue += 1;
        TimeManager.gameUICanvas.gameObject.SetActive(false);
        TimeManager.StartCoroutine(FadeInTransitionScreen());
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (TimeManager.transitionFadeImage.color.a >= 1f)
        {
            fadingIn = false;
        }
        trackEscInput = TimeManager.inputHandler.GetPauseValue();
        trackLmbInput = TimeManager.inputHandler.GetReelValue();
        if (enableChecks)
        {
            InputChecks();
        }
    }

    private IEnumerator FadeInTransitionScreen()
    {
        while (fadingIn)
        {
            alphaTransitionValue += TimeManager.transitionTick;
            TimeManager.transitionFadeImage.color = new Color(0, 0, 0, alphaTransitionValue);
            yield return new WaitForSecondsRealtime(0.05f);
            
        }
        if (TimeManager.DayCounterValue < 25 && TimeManager.marketManager.MoneyOwnedValue >= TimeManager.marketManager.RentValue)
        {
            NextDay();
            yield return new WaitForSecondsRealtime(3f);
            TimeManager.StartCoroutine(FadeOutTransitionScreen());
        }
        else if (TimeManager.marketManager.MoneyOwnedValue < TimeManager.marketManager.RentValue)
        {
            GameOver();
            enableChecks = true;
        }
        else if(TimeManager.DayCounterValue >= 25)
        {
            GameCompleted();
            enableChecks = true;
            
        }
        
    }

    private void InputChecks()
    {
        if (trackEscInput)
        {
            TimeManager.saveSystem.Save(TimeManager.marketManager.HighScore, TimeManager.HighScoreDayValue, TimeManager.marketManager.HighScoreOverallMoneyValue);
            ResetValuesAndTextsGameReset();
            SceneManager.LoadScene("MainMenu");
        }
        else if (trackLmbInput)
        {
            TimeManager.saveSystem.Save(TimeManager.marketManager.HighScore, TimeManager.HighScoreDayValue, TimeManager.marketManager.HighScoreOverallMoneyValue);
            ResetValuesAndTextsGameReset();
            TimeManager.StartCoroutine(FadeOutTransitionScreen());
        }
    }

    #region Types Of Actions After Fade In
    private void NextDay()
    {
        TimeManager.dayTransitionText.text = $"Day: {TimeManager.DayCounterValue}";
        TimeManager.DayCounterText.text = $"Day: {TimeManager.DayCounterValue}";
        UpdateValuesAndTextsDayComplete();
        TimeManager.dayTransitionText.gameObject.SetActive(true);
        TimeManager.moneyEarnedTransitionText.gameObject.SetActive(true);
    }
    
    private void GameOver()
    {
        FinalScreen();
        TimeManager.gameOverText.gameObject.SetActive(true);
        
    }
    
    private void GameCompleted()
    {
        FinalScreen();
        TimeManager.finalDayReachedText.gameObject.SetActive(true);
    }

    private void FinalScreen()
    {
        TimeManager.DayCounterValue--;
        if (TimeManager.saveSystem.HighScoreDays < TimeManager.DayCounterValue)
        {
            TimeManager.HighScoreDayValue = TimeManager.DayCounterValue;
        }
        UpdateValuesAndTextsDayComplete();
        TimeManager.marketManager.CalculateScore();
        TimeManager.marketManager.CheckHighscore();
        
        TimeManager.scoreText.text = $"Final Score: {TimeManager.marketManager.FinalScore}";
        TimeManager.dayTransitionText.text = $"Days Completed: {TimeManager.DayCounterValue}";
        
        TimeManager.dayTransitionText.gameObject.SetActive(true);
        TimeManager.buttonPromptsText.gameObject.SetActive(true);
        TimeManager.scoreText.gameObject.SetActive(true);
        
        if (TimeManager.isNewHighscore)
        {
            TimeManager.newHighscoreText.gameObject.SetActive(true);
        }
    }

    #endregion

    #region TextUpdating
    private void UpdateValuesAndTextsDayComplete()
    {
        TimeManager.marketManager.UpdateAllValues();
        TimeManager.marketUIManager.UpdateAllTexts();
    }

    private void ResetValuesAndTextsGameReset()
    {
        TimeManager.marketManager.ResetAllValues();
        TimeManager.marketUIManager.UpdateAllTexts();
    }
    #endregion

    private IEnumerator FadeOutTransitionScreen()
    {
        fadingOut = true;
        TimeManager.dayTransitionText.gameObject.SetActive(false);
        TimeManager.moneyEarnedTransitionText.gameObject.SetActive(false);
        TimeManager.gameOverText.gameObject.SetActive(false);
        TimeManager.buttonPromptsText.gameObject.SetActive(false);
        TimeManager.newHighscoreText.gameObject.SetActive(false);
        TimeManager.scoreText.gameObject.SetActive(false);
        TimeManager.finalDayReachedText.gameObject.SetActive(false);
        TimeManager.player.playerRb.transform.position = TimeManager.player.GetPlayerStartPosition();
        while (fadingOut)
        {
            alphaTransitionValue -= TimeManager.transitionTick;
            TimeManager.transitionFadeImage.color = new Color(0, 0, 0, alphaTransitionValue);
            if (alphaTransitionValue <= 0f)
            {
                fadingOut = false;
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
        TimeManager.fishingRod.Fsm.ChangeState(TimeManager.fishingRod.IdleState);
        TimeManager.fishingRod.caster.Fsm.ChangeState(TimeManager.fishingRod.caster.IdleState);
        TimeManager.fishSpawner.DespawnAllFish();
        Fsm.ChangeState(TimeManager.DayStartState);
    }
}
