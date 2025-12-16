using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketUIManager : MonoBehaviour
{
    #region Other Objects
    [Header("Other Objects")] 
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private MarketManager marketManager;
    [SerializeField]
    private TimeManager timeManager;
    #endregion
    
    #region UI Elements

    [Header("UI Elements")] 
    [SerializeField]
    private List<TextMeshProUGUI> valueTexts;
    [SerializeField]
    private Canvas marketCanvas;
    [SerializeField]
    private List<Button> feedButtons;
    [SerializeField]
    private Image clickedButton;
    [SerializeField] 
    private Button sellButton;
    [SerializeField] 
    private Button closeButton;
    [SerializeField] 
    private Image clickedCloseButton;
    [SerializeField] 
    private TextMeshProUGUI rentValueText;
    [SerializeField]
    private TextMeshProUGUI predictedValueText;
    [SerializeField] 
    private TextMeshProUGUI moneyOwnedText;
    [SerializeField]
    private TextMeshProUGUI moneyOverallText;
    [SerializeField] 
    private TextMeshProUGUI feedPriceText;
    #endregion
    
    #region Helper Parameters
    private List<MainButton> feedButtonsObjects;
    private MainButton sellButtonObject;
    private MainButton closeButtonObject;
    private AudioSource marketSource;
    private bool currentMarketUIValue;
    #endregion
    
    
    public class MainButton
    {
        public Image mainButtonImage;
        public Image buttonClickedImage;
        public FishTypeEnum? fishTypeEnum;
        public MainButton(Image mainButtonImage, Image buttonClickedImage)
        {
            this.mainButtonImage = mainButtonImage;
            this.buttonClickedImage = buttonClickedImage;
        }
    }

    #region Basic Unity Methods
    private void Awake()
    {
        feedButtonsObjects = new List<MainButton>();
        
        
    }

    private void Start()
    {
        for (int i = 0; i < feedButtons.Count; i++)
        {
            Button mainButton = feedButtons[i];
            Image mainButtonImage = mainButton.GetComponent<Image>();
            Image buttonClickedImage = clickedButton;
            MainButton newFeedButton = new MainButton(mainButtonImage, buttonClickedImage)
            {
                    fishTypeEnum = inventoryManager.GetFishSlots(i).fishType
            };
            feedButtonsObjects.Add(newFeedButton);
        }

        sellButtonObject = new MainButton(sellButton.GetComponent<Image>(), clickedButton);
        closeButtonObject = new MainButton(closeButton.GetComponent<Image>(), clickedCloseButton);
        marketSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Button Wrappers
    public void ClickFeedButtonWrapper(int index)
    {
        FishTypeEnum fishType = (FishTypeEnum)index;
        StartCoroutine(ClickFeedButtonCoroutine(index, fishType));
    }

    public void ClickCloseButtonWrapper()
    {
        StartCoroutine(ClickCloseButton());
    }

    public void ClickSellButtonWrapper()
    {
        StartCoroutine(ClickSellButton());  
    }
    #endregion
    
    #region Update Text
    public void UpdateMoneyOwnedText()
    {
        moneyOwnedText.text = $"Money: {marketManager.MoneyOwnedValue}$";
    }

    private void UpdatePredictedValueText()
    {
        predictedValueText.text = $"Value Of Fish In Inventory: {marketManager.InventoryValue}$";
    }

    public void UpdateMoneyOverallText()
    {
        moneyOverallText.text = $"Money Earned Overall: {marketManager.OverallMoneyValue}";
    }
    
    public void UpdateRentValueText()
    {
        
        rentValueText.text = $"Today's Rent: {marketManager.RentValue}$";
    }

    public void UpdateFeedPriceText()
    {
        feedPriceText.text = $"Fish Feeding Cost: {marketManager.FeedPrice}$";
    }
    
    public void UpdateValueTexts()
    {
        for (int i = 0; i < valueTexts.Count; i++)
        {
            valueTexts[i].text = $"{marketManager.FishTypes[i].FishValue}$";
        }
    }

    public void UpdateAllTexts()
    {
        UpdateMoneyOwnedText();
        UpdateMoneyOverallText();
        UpdateRentValueText();
        UpdateFeedPriceText();
        UpdateValueTexts();
    }
    #endregion
    
    #region Button Clicking Coroutines
    public IEnumerator ClickFeedButtonCoroutine(int index, FishTypeEnum fishType)
    {
        if (feedButtonsObjects[index].fishTypeEnum == inventoryManager.GetFishSlots(index).fishType)
        {
            Sprite temp = feedButtonsObjects[index].mainButtonImage.sprite;
            feedButtonsObjects[index].mainButtonImage.sprite =  feedButtonsObjects[index].buttonClickedImage.sprite;
            yield return new WaitForSecondsRealtime(0.1f);
            feedButtonsObjects[index].mainButtonImage.sprite = temp;
            yield return new WaitForSecondsRealtime(0.1f);
            marketManager.PayAndFeedFish(fishType);
            UpdateMoneyOwnedText();
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MeowFeed, marketSource);
        }
    }
    
    public IEnumerator ClickCloseButton()
    {
        Sprite temp = closeButtonObject.mainButtonImage.sprite;
        closeButtonObject.mainButtonImage.sprite = closeButtonObject.buttonClickedImage.sprite;
        yield return new WaitForSecondsRealtime(0.1f);
        closeButtonObject.mainButtonImage.sprite = temp;
        yield return new WaitForSecondsRealtime(0.1f);
        timeManager.PauseUnpause();
        ToggleMarketUI();
        AudioManager.Instance.PlaySound(AudioManager.SoundType.Close,  AudioManager.Instance.ManagerSource);
    }

    public IEnumerator ClickSellButton()
    {
        Sprite temp = sellButtonObject.mainButtonImage.sprite;
        sellButtonObject.mainButtonImage.sprite = sellButtonObject.buttonClickedImage.sprite;
        yield return new WaitForSecondsRealtime(0.1f);
        sellButtonObject.mainButtonImage.sprite = temp;
        yield return new WaitForSecondsRealtime(0.1f);
        marketManager.SellFish();
        UpdateMoneyOwnedText();
        UpdatePredictedValueText();
        if (timeManager.HoursValue >= 22 && marketManager.MoneyOwnedValue < marketManager.RentValue)
        {
            ToggleMarketUI();
            timeManager.PauseUnpause();
            yield return new WaitForSecondsRealtime(0.1f);
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MeowAngry, marketSource);
            timeManager.Fsm.ChangeState(timeManager.TransitionDaysState);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MeowNormal, marketSource);
        }
    }
    #endregion
    
    
    #region Helper Methods
    public void ToggleMarketUI()
    {
        currentMarketUIValue = !currentMarketUIValue;
        marketManager.UpdateValueOfInventory();
        UpdateFeedPriceText();
        UpdatePredictedValueText();
        marketCanvas.gameObject.SetActive(currentMarketUIValue);
    }

    public bool GetCurrentMarketUIValue()
    {
        return currentMarketUIValue;
    }
    #endregion
}
