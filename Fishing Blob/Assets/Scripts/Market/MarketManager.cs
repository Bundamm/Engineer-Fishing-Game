using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    //TODO: ADD READING PREVIOUS RECORD
    
    #region Other Objects
    [Header("Other Objects")]
    [SerializeField]
    private List<FishTypes> fishTypes;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField] 
    private FishSpawner fishSpawner;
    [SerializeField]
    private TimeManager timeManager;
    [SerializeField]
    private SaveSystem saveSystem;
    #endregion
    
    #region Helper Variables
    private float _rentPriceIncrease = 0;
    private float _feedPriceIncrease = 0;
    private float _startRentValue;
    private float _startFeedPrice;
    #endregion

    #region Rent And Feed Values
    [Header("Rent And Feed Values")]
    [SerializeField]
    private float rentValue = 200;
    [SerializeField]
    private float feedPrice = 100;
    [SerializeField]
    private float maxRentValue = 20000;
    [SerializeField]
    private float maxFeedValue = 500;
    [SerializeField]
    private float feedPriceMultiplier = 0.01f;
    [SerializeField]
    private float moneyOwnedRentMultiplier = 0.3f;
    [SerializeField]
    private float rentPriceMultiplier = 0.2f;
    [SerializeField]
    private float finalScoreMultiplier = 0.5f;
    [SerializeField]
    private int dayCheckValue = 5;
    
    #endregion
    
    #region Properties
    public List<FishTypes> FishTypes => fishTypes;
    public int MoneyOwnedValue { get; private set; } = 0;
    public int OverallMoneyValue { get; private set; } = 0;
    public int HighScoreOverallMoneyValue { get; private set; } = 0;
    public int InventoryValue { get; private set; } = 0;
    public float RentValue => rentValue;
    public float FeedPrice => feedPrice;
    public int FinalScore { get; private set;  }
    public int HighScore { get; private set; }

    #endregion
    
    private void Start()
    {
        _startRentValue = rentValue;
        _startFeedPrice = feedPrice;
        HighScoreOverallMoneyValue = saveSystem.MoneyGainedOverallHighScore;
        HighScore = saveSystem.HighScore;
    }
    
    public void SetStartingValues()
    {
        foreach (FishTypes fishType in fishTypes)
        {
            fishType.FishValue = fishType.ConstantFishValue;
        }
    }

    public void UpdateFishValues()
    {
        List<decimal> multiplierValues = new List<decimal>();
        for (int i = 1; i <= fishTypes.Count; i++)
        {
            var item = i / 2;
            multiplierValues.Add(item);
        }

        foreach (FishTypes fishType in fishTypes)
        {
            int randomListPos = Random.Range(0, multiplierValues.Count);
            decimal r = multiplierValues[randomListPos];
            multiplierValues.RemoveAt(randomListPos);
            if (timeManager.DayCounterValue % dayCheckValue == 0)
            {
                fishType.FishValue -= fishType.ConstantFishValue * r;
            }
            else
            {
                fishType.FishValue += fishType.ConstantFishValue * r;
            }
            
        }
    }

    public void ResetFishValues()
    {
        foreach (FishTypes fishType in fishTypes)
        {
            fishType.FishValue = fishType.ConstantFishValue;
        }
    }
    

    public void UpdateValueOfInventory()
    {
        for (int i = 0; i < fishTypes.Count; i++)
        {
            InventoryValue += inventoryManager.GetFishSlots(i).fishAmount * (int)fishTypes[i].FishValue;
        }
    }
    
    public void ResetInventoryValue()
    {
        InventoryValue = 0;
    }

    public void ResetMoneyOwnedValue()
    {
        MoneyOwnedValue /= 3;
    }

    public void UpdateAllValues()
    {
        UpdateMoneyOverallOwnedValue();
        UpdateRentValue();
        UpdateFishValues();
        UpdateFeedPrice();
    }
    
    public void ResetAllValues()
    {
        InventoryValue = 0;
        MoneyOwnedValue = 0;
        OverallMoneyValue = 0;
        rentValue = _startRentValue;
        feedPrice = _startFeedPrice;
        ResetFishValues();
        
    }

    public void SellFish()
    {
        MoneyOwnedValue += InventoryValue;
        ResetInventoryValue();
        inventoryManager.ClearInventory();
    }

    public void UpdateMoneyOverallOwnedValue()
    {
        OverallMoneyValue += MoneyOwnedValue;
        ResetMoneyOwnedValue();
    }

    public void UpdateFeedPrice()
    {
        _feedPriceIncrease = OverallMoneyValue * feedPriceMultiplier;
        if (timeManager.DayCounterValue % dayCheckValue == 0)
        {
            feedPrice -= _feedPriceIncrease;
            feedPrice /= 2;
        }
        else
        {
            feedPrice += _feedPriceIncrease;
        }
        feedPrice = Mathf.Clamp(feedPrice, 0, maxFeedValue);
        feedPrice = (int)Mathf.Round(feedPrice);
        
    }

    public void UpdateRentValue()
    {
        _rentPriceIncrease = OverallMoneyValue * moneyOwnedRentMultiplier / (rentValue *  rentPriceMultiplier);
        if (timeManager.DayCounterValue % dayCheckValue == 0)
        {
            rentValue /= _rentPriceIncrease;
        }
        else
        {
            rentValue *= _rentPriceIncrease;
        }
        rentValue = Mathf.Clamp(rentValue, 0, maxRentValue);
        rentValue = (int)Mathf.Round(rentValue);
    }

    public void PayAndFeedFish(FishTypeEnum fishType)
    {
        if (MoneyOwnedValue >= feedPrice)
        {
            MoneyOwnedValue -= (int)feedPrice;
            fishSpawner.FeedFish(fishType);   
        }
    }

    public void CalculateScore()
    {
        int scoreHelper = OverallMoneyValue * timeManager.DayCounterValue;
        FinalScore = (int)(scoreHelper * finalScoreMultiplier);
    }

    public void CheckHighscore()
    {
        if (FinalScore > HighScore)
        {
            HighScore = FinalScore;
            HighScoreOverallMoneyValue = OverallMoneyValue;
            timeManager.DayCounterValue = timeManager.DayCounterValue;
            timeManager.isNewHighscore = true;
        }
        else
        {
            timeManager.isNewHighscore = false;
        }
    }
}
