using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Trader : MonoBehaviour {
    public float				playerFunds, earnedFunds;
    public int                  traderNum;
    public Color                traderColor;
    
	
    public bool[]				hasStock = new bool[9];
    private float[]				stockPrice = new float[9];
    public int                  stockSelected;
    public Text					fundsDisplay, portfolioDisplay;
    public Text[]               transactionUI;
    public Text[]				holdingsDisplay;
    public Text                 traderTransaction;
    private String              ttt;
    
    // Transaction UI
    public GameObject           t_UI;
    
    // Audio
    public AudioSource          buy_sound, sP_sound, sL_sound;
    
    
    
    // Temp
    public KeyCode[]			stockKeys;
    public KeyCode buyKey;
    public KeyCode sellKey;
    public KeyCode bsKey;
    
    
    public static Trader        S;
    
    
    
    
    // Start is called before the first frame update
    void Awake() {
        S = this;
    }
    
    public void Start() {
        playerFunds = 10f;
        UpdateDisplay();
        for (int i = 0; i < GameManager.S.tradingStocks.Length; i++) {
            hasStock[i] = false;
        }

        GlobalVariables.S.traderWorth[traderNum] = playerFunds;
    }

    void Update() {
        Transaction(1);
        
        for (int i = 0; i < stockKeys.Length; i++) {
            if (Input.GetKey(stockKeys[i])) {
                stockSelected = i;
                t_UI.SetActive(true);
                //StartCoroutine("NoVoiceHack");
            }
            
        }

        if (stockSelected != 99) {
            //if (Input.GetKey(buyKey)) Buy(stockSelected);
            //if (Input.GetKey(sellKey)) Sell(stockSelected);

            // Slam down the phone to buy OR sell
            if (Input.GetKey(bsKey)) PhoneSlam(stockSelected);
        }
    }

    public void PhoneSlam(int stock) {
        int _stockNum = stock;

        if(hasStock[_stockNum]) Sell(_stockNum);
        else Buy(_stockNum);

        stockSelected = 99;
    }
    
    public IEnumerator NoVoiceHack() {
        yield return new WaitForSeconds(1);
        
        if(hasStock[stockSelected]) Sell(stockSelected);
        else Buy(stockSelected);

        stockSelected = 99;
    }
    
    public void Buy(int stock) {
        int _stockNum = stock;

        Stock _stb = GameManager.S.tradingStocks[_stockNum];
        float purchasePrice = _stb.stockValue;

        playerFunds -= purchasePrice;
        stockPrice[_stockNum] = _stb.stockValue;
        hasStock[_stockNum] = true;

        GameManager.S.EffectStock("up", _stockNum);
        UpdateDisplay();

        buy_sound.Play();

    }
    
    public void Sell(int stock) {
        int _stockNum = stock;
        
        if (hasStock[_stockNum]) {
            Stock _stb = GameManager.S.tradingStocks[_stockNum];
            float sellPrice = _stb.stockValue;
            float netGain = sellPrice - stockPrice[_stockNum];
            ttt = "Sold " + GameManager.S.tradingStocks[_stockNum].displayName.text + " at " + sellPrice * 100f + " / Earned " + netGain * 100f;
            playerFunds += sellPrice;
            earnedFunds += (netGain * 100f);
            hasStock[_stockNum] = false;
            
            if (netGain > 0) {
                sP_sound.Play();
            }
            else {
                sL_sound.Play();
            }
            
            GameManager.S.EffectStock("down", _stockNum);
            UpdateDisplay();
        } else {
            ttt = "Tried to sell a stock they didn't actually own";
        }
    }
    
    private void UpdateDisplay() {		
        float displayFunds = playerFunds * 100;
        //float portfolioFunds = displayFunds;
        // Overall funds
        //GlobalVariables.S.traderWorth[traderNum] = displayFunds;
        
        // Ammount earned
        GlobalVariables.S.traderWorth[traderNum] = earnedFunds;

        for (int i = 0; i < GameManager.S.tradingStocks.Length; i++) {
            if (hasStock[i]) {
                float displayPrice = stockPrice[i] * 100;
                Debug.Log("Stock:" + i + ":" + displayPrice);
                holdingsDisplay[i].text = "$" + displayPrice.ToString("F2");
            }
            else {
                holdingsDisplay[i].text = "$0";
            }

        }

        traderTransaction.text = ttt;
        fundsDisplay.text = "Available: $" + displayFunds.ToString("F2");
        portfolioDisplay.text = "Earned: $" + earnedFunds.ToString("F2");

    }
    
    // Transaction UI
    private void Transaction(int _numSelected) {
        int numSelected = _numSelected - 1;
        Stock _stb = GameManager.S.tradingStocks[numSelected];
        transactionUI[0].text = _numSelected + "-" + _stb.stockName;
        transactionUI[1].text = "$" + _stb.stockValue.ToString("F1");
        transactionUI[2].text = "$" + _stb.stockValue.ToString("F1");
    }
    
}
