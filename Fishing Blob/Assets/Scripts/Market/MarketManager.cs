using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    #region Other Objects
    [SerializeField]
    private List<FishTypes> fishTypes;
    public List<FishTypes> FishTypes => fishTypes;
    [SerializeField]
    private InventoryManager inventoryManager;
    #endregion
    
    private int inventoryValue = 0;
    public int InventoryValue => inventoryValue;
    private int moneyOwnedValue = 0;
    public int MoneyOwnedValue => moneyOwnedValue;
    private int overallMoneyValue = 0;
    public int OverallMoneyValue => overallMoneyValue;

    [SerializeField]
    private float rentValue = 200;

    public float RentValue => rentValue;
    
    [SerializeField]
    private float feedPrice = 100;

    public float FeedPrice => feedPrice;
    

    private float rentPriceIncrease = 0;
    private float feedPriceIncrease = 0;
    [SerializeField]
    private float feedPriceMultiplier = 0.01f;
    [SerializeField]
    private float rentPriceMultiplier = 0.2f;
    [SerializeField]
    private float moneyOwnedRentMultiplier = 0.3f;

    public void SetStartingValues()
    {
        foreach (FishTypes fishType in fishTypes)
        {
            fishType.FishValue = fishType.ConstantFishValue;
        }
    }

    public void GenerateFishValues()
    {
        List<decimal> multiplierValues = new List<decimal>();
        for (int i = 1; i <= fishTypes.Count; i++)
        {
            multiplierValues.Add(i / 2);
        }

        foreach (FishTypes fishType in fishTypes)
        {
            int randomListPos = Random.Range(0, multiplierValues.Count);
            decimal r = multiplierValues[randomListPos];
            multiplierValues.RemoveAt(randomListPos);
            fishType.FishValue += fishType.ConstantFishValue * r;
        }
    }

    public int UpdateValueOfInventory()
    {
        for (int i = 0; i < fishTypes.Count; i++)
        {
            inventoryValue += inventoryManager.GetFishSlots(i).fishAmount * (int)fishTypes[i].FishValue;
        }
        return inventoryValue;
    }
    
    public void ResetInventoryValue()
    {
        inventoryValue = 0;
    }

    public void ResetMoneyOwnedValue()
    {
        moneyOwnedValue = 0;
    }

    public void SellAllFish()
    {
        moneyOwnedValue += inventoryValue;
        ResetInventoryValue();
    }

    public void UpdateMoneyOverallOwnedValue()
    {
        overallMoneyValue += moneyOwnedValue;
        ResetMoneyOwnedValue();
    }

    public void UpdateFeedPrice()
    {
        feedPriceIncrease = overallMoneyValue * feedPriceMultiplier;
        feedPrice += feedPriceIncrease;
        
    }

    public void UpdateRentValue()
    {
        rentPriceIncrease = overallMoneyValue * moneyOwnedRentMultiplier / (rentValue *  rentPriceMultiplier);
        rentValue *= rentPriceIncrease;
    }
}
