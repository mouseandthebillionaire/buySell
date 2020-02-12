using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour {
    public int                  traderNum;
    public Color                traderColor;
    private float               traderLuck = 0.5f; // trader starts with average luck
    
	
    public bool[]				hasStock = new bool[GlobalVariables.S.roundStocks];
    public float[]				stockPrice = new float[GlobalVariables.S.roundStocks];
    public int                  stockSelected;
    public Text					codeInput;
    public Text[]               transactionUI;
    public GameObject           transactionWheel;
    public Color[]              transactionColors;
    public Text[]				holdingsDisplay;
    
    // Trader Statistics
    private float				playerFunds, earnedFunds;
    public int                  stocksBought, stocksSold;
    public Text                 stocksBoughtText, stocksSoldText;
    public Image                earningsMeter;
    
    // Dynamically add stocks as they are bought
    public GameObject           holdingsParent;
    public GameObject           holdingPrefab;
    public List<GameObject>     holdings = new List<GameObject>();
        
    // Transaction UI
    public GameObject           t_UI;
    
    // Audio
    public AudioSource          buy_sound, sP_sound, sL_sound;  
    
    // Input Setup 2.0 - Two keys to Select / Hanging Up Buys & Sells
    private KeyCode[]			inputKeys = new KeyCode[15]; // QWERTY values of each phone's keypad
    public string               inputString = "";
    public string               bonusString = "";
    private KeyCode             bsKey;

    
    public static Trader        S;
    
    
    
    
    // Start is called before the first frame update
    void Awake() {
        S = this;
    }
    
    public void Start() {

        // Set the stockKeys
        for (int i = 0; i < inputKeys.Length; i++) {
            inputKeys[i] = GlobalVariables.S.inputKeys[traderNum, i];
        }
        // Set the length of code
        for (int i = 0; i < GlobalVariables.S.stockCodeLength; i++) {
            inputString += "-";
        }

        stocksBought = 0;
        stocksSold = 0;

        bsKey = inputKeys[14];

        stockSelected = 99;
        UpdateDisplay();
        for (int i = 0; i < StockManager.S.tradingStocks.Length; i++) {
            hasStock[i] = false;
        }
        
        

        // Set up player overall and round funds
        playerFunds = GlobalVariables.S.traderWorth[traderNum];

        earnedFunds = 0;
    }

    void Update() {   
        
//        // Can we look for special any-length code entered outside the main loop
//        for (int i = 0; i < inputKeys.Length-3; i++) {
//            // Looking for pound
//            if(Input.GetKeyDown(inputKeys[11]))
//            if (Input.GetKeyDown(inputKeys[i])) {
//                if(in)
//                bonusString += i.ToString();
//                // only get the the last two digits the user has entered
//                inputString = inputString.Substring(inputString.Length-GlobalVariables.S.stockCodeLength, GlobalVariables.S.stockCodeLength);
//                codeInput.text = "#" + inputString;
//            }
//        }
        
        if(GlobalVariables.S.trading){
                for (int i = 0; i < inputKeys.Length-5; i++) {
                    if (Input.GetKeyDown(inputKeys[i])) {
                        inputString += i.ToString();
                        // only get the the last two digits the user has entered
                        inputString = inputString.Substring(inputString.Length-GlobalVariables.S.stockCodeLength, GlobalVariables.S.stockCodeLength);
                        codeInput.text = "#" + inputString;
                    }
                }

                // version of PhoneSlam that takes an inputString
                if (Input.GetKeyDown(bsKey)) {
                    PhoneSlammed(inputString);
                    inputString = "-";
                    codeInput.text = "#" + inputString;
                }  
                
        // otherwise, we're in a minigame       
        } else {
            for (int i = 0; i < inputKeys.Length; i++) {
                if (Input.GetKeyDown(inputKeys[i])) {
                    //Minigame.S.ReceiveKey(traderNum, i);
                }
            }
        }
    }

    // version of PhoneSlam that takes an inputString
    public void PhoneSlammed(string codeEntered) {
                
        bool stockExists = false;
        int stockEntered = 99;

        for(int i=0; i < GlobalVariables.S.roundStocks; i++){
            
                if (codeEntered == StockManager.S.roundStocks[i].GetComponent<Stock>().stockCode) {
                    stockExists = true;
                    stockEntered = i;                    
                }
        }

        if (stockExists) {
            Transaction(stockEntered);
            if(hasStock[stockEntered]) {StartCoroutine(TransactionProcessing(stockEntered, "sell"));}
            else StartCoroutine(TransactionProcessing(stockEntered, "buy"));         
        } else {
            Debug.Log("Not a valid Stock Code!");
        }
    }
    
    public void Buy(int stock) {
        int _stockNum = stock;

        GameObject _stb = StockManager.S.roundStocks[_stockNum];
        Stock s = _stb.GetComponent<Stock>();
        float purchasePrice = s.stockValue;

        playerFunds -= purchasePrice;
        stockPrice[_stockNum] = purchasePrice;
        hasStock[_stockNum] = true;
        stocksBought++;
        GlobalVariables.S.traderRoundStats[traderNum, 0]++;

        // Stock is influenced by...
        // a) traderLuck
        // b) add something else?
        
        float dirChance = UnityEngine.Random.value;
        if (dirChance < traderLuck) {
            StockManager.S.EffectStock("up", _stockNum);
        } else {
            StockManager.S.EffectStock("down", _stockNum);
        }
        
        // Add the stock to the list of owned stocks

        GameObject go = GameObject.Instantiate(holdingPrefab) as GameObject;
        go.transform.parent = holdingsParent.transform;
        Text name = go.transform.Find("stockName").GetComponent<Text>();
        name.text = s.stockName;
        Text value = go.transform.Find("stockAMT").GetComponent<Text>();
        float displayPrice = purchasePrice * 100;
        value.text = " AT $"+ displayPrice.ToString("F1");
  
        holdings.Add(go);
        
        UpdateDisplay();
        buy_sound.Play();

    }
    
    public void Sell(int stock) {
        int _stockNum = stock;
        
        if (hasStock[_stockNum]) {
            
            GameObject _stb = StockManager.S.roundStocks[_stockNum];
            Stock s = _stb.GetComponent<Stock>();
            float sellPrice = s.stockValue;
            float netGain = sellPrice - stockPrice[_stockNum];
            string stockName = s.stockName;
            
            playerFunds += sellPrice;
            earnedFunds += (netGain * 100f);
            GlobalVariables.S.traderWorth[traderNum] += earnedFunds;
            
            int holdingsLocation = 0;
            for(int i=0; i<holdings.Count; i++) {
                if(holdings[i].transform.Find("stockName").GetComponent<Text>().text == stockName){
                    holdingsLocation = i;
                }
            }
            holdings.RemoveAt(holdingsLocation);
            Destroy (holdingsParent.transform.GetChild (holdingsLocation).gameObject);
            
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
                StockManager.S.EffectStock("up", _stockNum);
            } else {
                StockManager.S.EffectStock("down", _stockNum);
            }
            
            hasStock[_stockNum] = false;
            stocksSold++;
            GlobalVariables.S.traderRoundStats[traderNum, 1]++;
            
            UpdateDisplay();
        } 
    }
    
    private void UpdateDisplay() {		
        float displayFunds = playerFunds * 100;
        //float portfolioFunds = displayFunds;
        // Overall funds
        
        // Ammount earned
        earningsMeter.fillAmount = earnedFunds / 200f;

        int holdingIndex = 0;

        foreach (GameObject go in holdings) {
            go.transform.localPosition = new Vector3(90, -40 * holdingIndex, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            holdingIndex++;
        }
        
        stocksBoughtText.text = "BOUGHT: " + stocksBought.ToString();
        stocksSoldText.text = "| SOLD: " + stocksSold.ToString();
              
        
        
 
        // Show The Fund Amount that Each Trader Has
        //traderTransaction.text = ttt;
        //fundsDisplay.text = "Available: $" + displayFunds.ToString("F2");
        //portfolioDisplay.text = "Earned: $" + earnedFunds.ToString("F2");

    }
    
    // Transaction UI
    private void Transaction(int _numSelected) {
                
        int stockToProcess = _numSelected;
        GameObject _stb = StockManager.S.roundStocks[_numSelected];
        Stock s = _stb.GetComponent<Stock>();
        t_UI.SetActive(true);
        transactionUI[0].text = s.stockName;
        if (hasStock[stockToProcess]) {
            transactionUI[1].text = "$" + s.stockValue.ToString("F1");
            transactionUI[2].text = "$" + stockPrice[stockToProcess].ToString("F1");
        }
        else {
            transactionUI[1].text = "--";
            transactionUI[2].text = "$" + s.stockValue.ToString("F1");

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

    public void ClearInput() {
        
    }
    
}
