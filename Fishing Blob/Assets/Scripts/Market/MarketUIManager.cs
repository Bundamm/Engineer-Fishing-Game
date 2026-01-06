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
    private List<MainButton> _feedButtonsObjects;
    private MainButton _sellButtonObject;
    private MainButton _closeButtonObject;
    private AudioSource _marketSource;
    private bool _currentMarketUIValue;
    #endregion
    
    #region
    public bool canInteractWithHouse;
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
        _feedButtonsObjects = new List<MainButton>();
        
        
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
            _feedButtonsObjects.Add(newFeedButton);
        }

        _sellButtonObject = new MainButton(sellButton.GetComponent<Image>(), clickedButton);
        _closeButtonObject = new MainButton(closeButton.GetComponent<Image>(), clickedCloseButton);
        _marketSource = GetComponent<AudioSource>();
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
        if (_feedButtonsObjects[index].fishTypeEnum == inventoryManager.GetFishSlots(index).fishType)
        {
            Sprite temp = _feedButtonsObjects[index].mainButtonImage.sprite;
            _feedButtonsObjects[index].mainButtonImage.sprite =  _feedButtonsObjects[index].buttonClickedImage.sprite;
            yield return new WaitForSecondsRealtime(0.1f);
            _feedButtonsObjects[index].mainButtonImage.sprite = temp;
            yield return new WaitForSecondsRealtime(0.1f);
            marketManager.PayAndFeedFish(fishType);
            UpdateMoneyOwnedText();
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MeowFeed, _marketSource);
        }
    }
    
    public IEnumerator ClickCloseButton()
    {
        Sprite temp = _closeButtonObject.mainButtonImage.sprite;
        _closeButtonObject.mainButtonImage.sprite = _closeButtonObject.buttonClickedImage.sprite;
        yield return new WaitForSecondsRealtime(0.1f);
        _closeButtonObject.mainButtonImage.sprite = temp;
        yield return new WaitForSecondsRealtime(0.1f);
        timeManager.PauseUnpause();
        ToggleMarketUI();
        AudioManager.Instance.PlaySound(AudioManager.SoundType.Close,  AudioManager.Instance.ManagerSource);
    }

    public IEnumerator ClickSellButton()
    {
        Sprite temp = _sellButtonObject.mainButtonImage.sprite;
        _sellButtonObject.mainButtonImage.sprite = _sellButtonObject.buttonClickedImage.sprite;
        yield return new WaitForSecondsRealtime(0.1f);
        _sellButtonObject.mainButtonImage.sprite = temp;
        yield return new WaitForSecondsRealtime(0.1f);
        marketManager.SellFish();
        UpdateMoneyOwnedText();
        UpdatePredictedValueText();
        if (timeManager.HoursValue >= 22 && marketManager.MoneyOwnedValue < marketManager.RentValue)
        {
            ToggleMarketUI();
            timeManager.PauseUnpause();
            yield return new WaitForSecondsRealtime(0.1f);
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MeowAngry, _marketSource);
            timeManager.marketIndicator.gameObject.SetActive(false);
            timeManager.Fsm.ChangeState(timeManager.TransitionDaysState);
        }
        else if (timeManager.HoursValue >= 22 && marketManager.MoneyOwnedValue >= marketManager.RentValue)
        {
            timeManager.marketIndicator.gameObject.SetActive(false);
            canInteractWithHouse = true;
            timeManager.houseIndicator.gameObject.SetActive(true);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.SoundType.MeowNormal, _marketSource);
        }
    }
    #endregion
    
    
    #region Helper Methods
    public void ToggleMarketUI()
    {
        _currentMarketUIValue = !_currentMarketUIValue;
        marketManager.UpdateValueOfInventory();
        UpdateFeedPriceText();
        UpdatePredictedValueText();
        marketCanvas.gameObject.SetActive(_currentMarketUIValue);
    }

    public bool GetCurrentMarketUIValue()
    {
        return _currentMarketUIValue;
    }
    #endregion
}
