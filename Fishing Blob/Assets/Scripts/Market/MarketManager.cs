using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    #region Other Objects
    [Header("Other Objects")]
    [SerializeField]
    private List<FishTypes> fishTypes;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField] 
    private FishSpawner fishSpawner;
    #endregion
    
    #region Money Value Helpers
    private int _inventoryValue = 0;
    private int _moneyOwnedValue = 0;
    private int _overallMoneyValue = 0;
    private float _rentPriceIncrease = 0;
    private float _feedPriceIncrease = 0;
    #endregion

    #region Rent And Feed Values
    [Header("Rent And Feed Values")]
    [SerializeField]
    private float rentValue = 200;
    [SerializeField]
    private float feedPrice = 100;
    [SerializeField]
    private float feedPriceMultiplier = 0.01f;
    [SerializeField]
    private float rentPriceMultiplier = 0.2f;
    [SerializeField]
    private float moneyOwnedRentMultiplier = 0.3f;
    #endregion
    
    #region Properties
    public List<FishTypes> FishTypes => fishTypes;
    public int MoneyOwnedValue => _moneyOwnedValue;
    public int OverallMoneyValue => _overallMoneyValue;
    public int InventoryValue => _inventoryValue;
    public float RentValue => rentValue;
    public float FeedPrice => feedPrice;
    #endregion

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
            fishType.FishValue += fishType.ConstantFishValue * r;
        }
    }

    public void UpdateValueOfInventory()
    {
        for (int i = 0; i < fishTypes.Count; i++)
        {
            _inventoryValue += inventoryManager.GetFishSlots(i).fishAmount * (int)fishTypes[i].FishValue;
        }
    }
    
    public void ResetInventoryValue()
    {
        _inventoryValue = 0;
    }

    public void ResetMoneyOwnedValue()
    {
        _moneyOwnedValue /= 3;
    }

    public void SellFish()
    {
        _moneyOwnedValue += _inventoryValue;
        ResetInventoryValue();
        inventoryManager.ClearInventory();
    }

    public void UpdateMoneyOverallOwnedValue()
    {
        _overallMoneyValue += _moneyOwnedValue;
        ResetMoneyOwnedValue();
    }

    public void UpdateFeedPrice()
    {
        _feedPriceIncrease = _overallMoneyValue * feedPriceMultiplier;
        feedPrice += _feedPriceIncrease;
        feedPrice = (int)feedPrice;
        
    }

    public void UpdateRentValue()
    {
        _rentPriceIncrease = _overallMoneyValue * moneyOwnedRentMultiplier / (rentValue *  rentPriceMultiplier);
        rentValue *= _rentPriceIncrease;
        rentValue = (int)rentValue;
    }

    public void PayAndFeedFish(FishTypeEnum fishType)
    {
        if (_moneyOwnedValue >= feedPrice)
        {
            _moneyOwnedValue -= (int)feedPrice;
            fishSpawner.FeedFish(fishType);   
        }
    }
}
