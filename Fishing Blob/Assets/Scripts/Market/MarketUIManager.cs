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
    private TextMeshProUGUI feedPriceText;
    #endregion
    
    #region Helpers
    private List<MainButton> feedButtonsObjects;
    private MainButton sellButtonObject;
    private MainButton closeButtonObject;
    private bool currentMarketUIValue;
    #endregion
    
    
    public class MainButton
    {
        public Button mainButton;
        public Image mainButtonImage;
        public Image buttonClickedImage;
        public FishTypeEnum? fishTypeEnum;
        public MainButton(Button mainButton, Image mainButtonImage, Image buttonClickedImage)
        {
            this.mainButton = mainButton;
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
            MainButton newFeedButton = new MainButton(mainButton, mainButtonImage, buttonClickedImage)
            {
                    fishTypeEnum = inventoryManager.GetFishSlots(i).fishType
            };
            feedButtonsObjects.Add(newFeedButton);
        }

        sellButtonObject = new MainButton(sellButton, sellButton.GetComponent<Image>(), clickedButton);
        closeButtonObject = new MainButton(closeButton, closeButton.GetComponent<Image>(), clickedCloseButton);
    }

    private void Update()
    {
        //TODO: UPDATE PREDICTED VALUE OF INVENTORY
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
        }
    }

    private void UpdateMoneyOwnedText()
    {
        moneyOwnedText.text = $"Money: {marketManager.MoneyOwnedValue}$";
    }

    private void UpdatePredictedValueText()
    {
        predictedValueText.text = $"Predicted Value: {marketManager.InventoryValue}$";
    }

    private void UpdateFeedPriceText()
    {
        feedPriceText.text = $"Cost: {marketManager.FeedPrice}$";
    }


    
    public IEnumerator ClickCloseButton()
    {
        Sprite temp = closeButtonObject.mainButtonImage.sprite;
        closeButtonObject.mainButtonImage.sprite = closeButtonObject.buttonClickedImage.sprite;
        yield return new WaitForSecondsRealtime(0.1f);
        closeButtonObject.mainButtonImage.sprite = temp;
        yield return new WaitForSecondsRealtime(0.1f);
        
    }

    public IEnumerator ClickSellButton()
    {
        Sprite temp = sellButtonObject.mainButtonImage.sprite;
        sellButtonObject.mainButtonImage.sprite = sellButtonObject.buttonClickedImage.sprite;
        yield return new WaitForSecondsRealtime(0.1f);
        sellButtonObject.mainButtonImage.sprite = temp;
        yield return new WaitForSecondsRealtime(0.1f);
        marketManager.SellAllFish();
        UpdateMoneyOwnedText();
        UpdatePredictedValueText();
    }
    
    public void UpdateValueTexts()
    {
        for (int i = 0; i < valueTexts.Count; i++)
        {
            valueTexts[i].text = $"{marketManager.FishTypes[i].FishValue}$";
        }
    }
    
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
}
