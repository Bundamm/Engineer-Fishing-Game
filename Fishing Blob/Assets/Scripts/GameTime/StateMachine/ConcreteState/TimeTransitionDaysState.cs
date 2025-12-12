
using System.Collections;
using UnityEngine;

public class TimeTransitionDaysState : TimeState
{
    private bool fadingIn;
    private bool fadingOut;

    private float alphaTransitionValue;
    
    public TimeTransitionDaysState(TimeManager timeManager, TimeStateMachine fsm) : base(timeManager, fsm)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered TimeTransitionDaysState");
        
        fadingIn = true;
        fadingOut = false;
        alphaTransitionValue = 0f;
        TimeManager.dayCounterValue += 1;
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
    }

    private IEnumerator FadeInTransitionScreen()
    {
        while (fadingIn)
        {
            alphaTransitionValue += TimeManager.transitionTick;
            TimeManager.transitionFadeImage.color = new Color(0, 0, 0, alphaTransitionValue);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        TimeManager.dayTransitionText.text = $"Day: {TimeManager.dayCounterValue}";
        TimeManager.dayCounterText.text = $"Day: {TimeManager.dayCounterValue}";
        TimeManager.marketManager.UpdateMoneyOverallOwnedValue();
        TimeManager.marketManager.UpdateRentValue();
        TimeManager.marketManager.UpdateFishValues();
        TimeManager.marketUIManager.UpdateRentValueText();
        TimeManager.marketUIManager.UpdateFeedPriceText();
        TimeManager.marketUIManager.UpdateValueTexts();
        TimeManager.marketUIManager.UpdateMoneyOwnedText();
        TimeManager.marketUIManager.UpdateMoneyOverallText();
        TimeManager.dayTransitionText.gameObject.SetActive(true);
        TimeManager.moneyEarnedTransitionText.gameObject.SetActive(true);
        
        yield return new WaitForSecondsRealtime(3f);
        TimeManager.StartCoroutine(FadeOutTransitionScreen());
    }

    private IEnumerator FadeOutTransitionScreen()
    {
        fadingOut = true;
        TimeManager.dayTransitionText.gameObject.SetActive(false);
        TimeManager.moneyEarnedTransitionText.gameObject.SetActive(false);
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
        TimeManager.fishingRod.Caster.Fsm.ChangeState(TimeManager.fishingRod.Caster.IdleState);
        Fsm.ChangeState(TimeManager.DayStartState);
    }
}
