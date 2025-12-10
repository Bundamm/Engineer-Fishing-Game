using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

[System.Serializable]
public class FishSlot
{
    public FishTypeEnum fishType;
    public int fishAmount;
}

public class InventoryManager : MonoBehaviour
{
    #region Inventory Slot Identifiers
    [Header("Inventory Slot Object List")]
    [SerializeField]
    private List<FishSlot> fishSlots;
    #endregion
    
    #region Canvas Values
    [Header("Canvas Values")]
    [SerializeField]
    private List<Image> fishIcons;
    [SerializeField] 
    private List<TextMeshProUGUI> fishAmountTexts;
    #endregion
    
    #region Tuple Lists
    [Header("Icon Sprite Lists")]
    [SerializeField] 
    private List<Sprite> fishNoneIconsSprites;
    [SerializeField]
    private List<Sprite> fishGainedIconsSprites;

    private List<(FishTypeEnum fishType, Sprite NoneSprite, Sprite GainedSprite)> tupleListOfFishSprites;
    private List<(FishTypeEnum fishType, Image ImageSlot, TextMeshProUGUI TextSlot)> tupleListOfInventorySlotValues;
    #endregion  
    
    private void Start()
    {
        PopulateTupleLists();
    }

    private void PopulateTupleLists()
    {
        if (fishNoneIconsSprites.Count != fishGainedIconsSprites.Count) throw new Exception("There are " + fishGainedIconsSprites.Count + " gained icons and "  + fishNoneIconsSprites.Count + " none icons. The number should be the same.");
        if(fishIcons.Count != fishAmountTexts.Count) throw new Exception("There are " + fishIcons.Count + "Image slots and " + fishAmountTexts.Count + " text slots for amount of Fish. The number should be the same.");
        tupleListOfFishSprites = new List<(FishTypeEnum, Sprite, Sprite)>();
        tupleListOfInventorySlotValues = new List<(FishTypeEnum, Image, TextMeshProUGUI)>();
        if (fishNoneIconsSprites.Count == fishIcons.Count)
        {
            for (int i = 0; i < fishNoneIconsSprites.Count; i++)
            {
                tupleListOfFishSprites.Add((fishSlots[i].fishType, fishNoneIconsSprites[i], fishGainedIconsSprites[i]));
                tupleListOfInventorySlotValues.Add((fishSlots[i].fishType, fishIcons[i], fishAmountTexts[i]));
            }
        }
        else
        {
            throw new Exception("Cannot finish initializing tuples as the all the lists dont have the exact same amount of items.");
        }
    }

    private void UpdateInventorySlot(FishTypeEnum fishType)
    {
        var foundTuple = tupleListOfInventorySlotValues.Find(x => x.fishType == fishType);
        var spriteTuple = tupleListOfFishSprites.Find(x => x.fishType == fishType);
        FishSlot foundSlot = fishSlots.Find(x => x.fishType == fishType);
        
        foundTuple.TextSlot.text = $"x{foundSlot.fishAmount}";
        if (foundSlot.fishAmount > 0)
        {
            foundTuple.ImageSlot.sprite = spriteTuple.GainedSprite;
            foundTuple.ImageSlot.color = new Color(255, 255, 255, 1f);
        }
        else if (foundSlot.fishAmount <= 0)
        {
            foundTuple.ImageSlot.sprite = spriteTuple.NoneSprite;
            foundTuple.ImageSlot.color = new Color(70, 70, 70, 0.4f);
        }
    }
    
    public void IncreaseAmountOfFish(Fish fish, int amount)
    {
        fishSlots.Find(x => x.fishType == fish.fishType.FishTypeEnum).fishAmount += amount;
        UpdateInventorySlot(fish.fishType.FishTypeEnum);
    }

    public FishSlot GetSpecificFishSlot(FishTypeEnum fishTypeEnum)
    {
        return fishSlots.Find(x => x.fishType == fishTypeEnum);
    }

    public void SellAllFish()
    {
        foreach (var fishSlot in fishSlots)
        {
            fishSlot.fishAmount = 0;
            UpdateInventorySlot(fishSlot.fishType);
            //TODO: ADD SCORE TO THE SCORE MANAGER
        }
    }

    public FishSlot GetFishSlots(int index)
    {
        return fishSlots[index];
    }
}
