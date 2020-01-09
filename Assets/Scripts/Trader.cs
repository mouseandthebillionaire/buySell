using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour {
    public float				playerFunds, earnedFunds;
    public int                  traderNum;
    public Color                traderColor;
    private float               traderLuck = 0.5f; // trader starts with average luck
    
	
    public bool[]				hasStock = new bool[9];
    private float[]				stockPrice = new float[9];
    public int                  stockSelected;
    public Text					fundsDisplay, portfolioDisplay;
    public Text[]               transactionUI;
    public GameObject           transactionWheel;
    public Color[]              transactionColors;
    public Text[]				holdingsDisplay;
    public Text                 traderTransaction;
    private String              ttt;
    
    
    // Transaction UI
    public GameObject           t_UI;
    
    // Audio
    public AudioSource          buy_sound, sP_sound, sL_sound;  
    
    // Input Setup 2.0 - Two keys to Select / Hanging Up Buys & Sells
    private KeyCode[]			inputKeys; // QWERTY values of each phone's keypad
    public string               inputString = "--"; 
    public KeyCode              bsKey;

    
    public static Trader        S;
    
    
    
    
    // Start is called before the first frame update
    void Awake() {
        S = this;
    }
    
    public void Start() {
        // Set the stockKeys
        if (traderNum == 0) {
            inputKeys = new []
                {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O};
        } else if (traderNum == 1) {
            inputKeys = new []
                {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L};
        } else if (traderNum == 2) {
            inputKeys = new[] {
                KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Comma,
                KeyCode.Period
            };
        }
        
        playerFunds = 10f;
        stockSelected = 99;
        UpdateDisplay();
        for (int i = 0; i < GameManager.S.tradingStocks.Length; i++) {
            hasStock[i] = false;
        }

        GlobalVariables.S.traderWorth[traderNum] = playerFunds;
    }

    void Update() {
        if(GlobalVariables.S.trading){
                for (int i = 0; i < inputKeys.Length; i++) {
                    if (Input.GetKeyDown(inputKeys[i])) {
                        inputString += (i + 1).ToString();
                    }
                }
                // only get the the last two digits the user has entered
                inputString = inputString.Substring(inputString.Length-2, 2);

                // make sure the price displayed is always up-to-date
                //transactionUI[2].text = "$" + GameManager.S.tradingStocks[stockSelected].stockValue.ToString("F1");

                // version of PhoneSlam that takes an inputString
                if (Input.GetKeyDown(bsKey)) PhoneSlammed(inputString);  
                
                
                // Single key value denotes stockSelected
//                if (stockSelected != 99) {
//                    //if (Input.GetKey(buyKey)) Buy(stockSelected);
//                    //if (Input.GetKey(sellKey)) Sell(stockSelected);
//
//                    // Slam down the phone to buy OR sell
//                    if (Input.GetKey(bsKey)) PhoneSlam(stockSelected);    
//                }
        // otherwise, we're in a minigame       
        } else {
            for (int i = 0; i < inputKeys.Length; i++) {
                if (Input.GetKeyDown(inputKeys[i])) {
                    Minigame.S.ReceiveKey(traderNum, i);
                }
            }
        }
    }

//    public void PhoneSlam(int stock) {
//        int _stockNum = stock;
//        Debug.Log("we selling?");
//
//        if (hasStock[_stockNum]) {
//            StartCoroutine(TransactionProcessing(_stockNum, "sell"));
//        }
//        else {
//            
//            StartCoroutine(TransactionProcessing(_stockNum, "buy"));
//
//        }
//
//        stockSelected = 99;
//    }

    // version of PhoneSlam that takes an inputString
    public void PhoneSlammed(string codeEntered) {

        bool stockExists = false;
        int stockSelected = 99;

        for(int i=0; i < GameManager.S.stockCodes.Length; i++){
            if (codeEntered == GameManager.S.stockCodes[i]){
                stockExists = true;
                stockSelected = i;
            }
        }

        if (stockExists) {
            Debug.Log("The user wants to buy/sell stock #" + stockSelected);
            if (hasStock[stockSelected]) {
                Buy(stockSelected);
                StartCoroutine(TransactionProcessing(stockSelected, "sell"));
            }
            else {
                Sell(stockSelected);
                StartCoroutine(TransactionProcessing(stockSelected, "buy"));
            }         
        } else {
            Debug.Log("Not a valid Stock Code!");
        }
    }
    
    public void Buy(int stock) {
        int _stockNum = stock;

        Stock _stb = GameManager.S.tradingStocks[_stockNum];
        float purchasePrice = _stb.stockValue;

        playerFunds -= purchasePrice;
        stockPrice[_stockNum] = _stb.stockValue;
        hasStock[_stockNum] = true;

        // Stock is influenced by...
        // a) traderLuck
        // b) add something else?
        
        float dirChance = UnityEngine.Random.value;
        if (dirChance < traderLuck) {
            GameManager.S.EffectStock("up", _stockNum);
        } else {
            GameManager.S.EffectStock("down", _stockNum);
        }
        
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
                transactionWheel.GetComponent<Image>().color = transactionColors[1];
                sP_sound.Play();
                // increase the trader's 'luck' as they become more successful
                traderLuck += 0.05f;
            }
            else {
                transactionWheel.GetComponent<Image>().color = transactionColors[2];
                sL_sound.Play();
                // decrease the trader's 'luck' as they become less successful
                traderLuck -= 0.05f;
            }
            
            // There's always a 50% chance that a Stock will go up/down after a sale
            float dirChance = UnityEngine.Random.value;
            if (dirChance < 0.5f) {
                GameManager.S.EffectStock("up", _stockNum);
            } else {
                GameManager.S.EffectStock("down", _stockNum);
            }
            
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
        
        stockSelected = _numSelected;
        Stock _stb = GameManager.S.tradingStocks[stockSelected];
        t_UI.SetActive(true);
        transactionUI[0].text = _numSelected+1 + "-" + _stb.stockName;
        if (hasStock[stockSelected]) {
            transactionUI[1].text = "$" + stockPrice[stockSelected].ToString("F1");
        }
        else {
            transactionUI[1].text = "--";
        }
    }

    private IEnumerator TransactionProcessing(int _stockNum, string action) {
        transactionWheel.GetComponent<Image>().color = transactionColors[0];
        Animator a = t_UI.GetComponent<Animator>();
        a.Play("transactionProcessing");
        yield return new WaitForSeconds(1);
        if(action == "buy") Buy(_stockNum);
        if(action == "sell") Sell(_stockNum);
   
    }
    
}
