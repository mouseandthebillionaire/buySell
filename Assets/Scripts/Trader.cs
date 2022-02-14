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
    private float				totalEarnings, roundEarnings, playerFunds, earnedFunds;
    public int                  stocksBought, stocksSold;
    public Text                 stocksBoughtText, stocksSoldText;
    public Image                earningsMeter;

    // Old version in which we Dynamically add stocks as they are bought
    public GameObject           holdingsParent;
    public GameObject           holdingPrefab;
    public List<GameObject>     holdings = new List<GameObject>();
    
    // New version constantly keeps track of all stocks
    public GameObject[]         stockHoldingsDisplay = new GameObject[GlobalVariables.S.totalStocks];
    // Keep a list of the purchase cost of all 9 stocks
    private float[]             stockHoldings = new float[GlobalVariables.S.totalStocks];
    public Image[]              diffDisplays;
        
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

        // for (int i = 0; i < 5; i++) {
        //     Debug.Log(StockManager.S.stockIndexes[i]);
        // }
        
        // Dim the stocks that aren't being traded this round
        for (int i = 0; i < stockHoldingsDisplay.Length; i++) {
            // stockIndexes stores the StockNumber for each day's active stocks
            if (StockManager.S.stockIndexes.Contains(i)) Debug.Log(i);
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
        totalEarnings = GlobalVariables.S.traderWorth[traderNum];
        roundEarnings = GameManager.S.roundWorth[traderNum];

        earnedFunds = 0;

        StartCoroutine(UpdateTodaysStocks());
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
                
                // print out the holdings for each trader
                for (int i = 0; i < GlobalVariables.S.totalStocks; i++)
                {
                    //Debug.Log(stockHoldings[i]);
                }
        } 
        
        // Keep Track of the Difference between each stocks purchasePrice and current price
        for (int i = 0; i < GlobalVariables.S.totalStocks; i++) {
            if (StockManager.S.stockIndexes.Contains(i)) {
                if (GlobalVariables.S.traderHoldings[traderNum, i] != 0)
                {
                    float f = StockManager.S.stockValues[i] - GlobalVariables.S.traderHoldings[traderNum, i];
                    // Then show that visually
                    if (f > 0) diffDisplays[i].color = Color.green;
                    else diffDisplays[i].color = Color.red;
                    diffDisplays[i].fillAmount = Mathf.Abs(f / 5f);
                }
                else
                {
                    diffDisplays[i].fillAmount = 0;
                }
            }
        }
    }

    // version of PhoneSlam that takes an inputString
    public void PhoneSlammed(string codeEntered) {
                
        bool stockExists = false;
        int stockEntered = 99;
        int stockNum = 99;

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
        stockHoldings[_stockNum] = purchasePrice;
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
        
        // Dynamically add the stock to the list of owned stocks
        // GameObject go = GameObject.Instantiate(holdingPrefab) as GameObject;
        // go.transform.parent = holdingsParent.transform;
        // Text name = go.transform.Find("stockName").GetComponent<Text>();
        // name.text = s.stockName;
        // Text value = go.transform.Find("stockAMT").GetComponent<Text>();
        // float displayPrice = purchasePrice;
        // value.text = " AT $"+ displayPrice.ToString("F1");
        //holdings.Add(go);
        
        
        // Get the global Stock Number
        int globalStockNum = StockManager.S.roundStocks[_stockNum].GetComponent<Stock>().stockNumber;

        Text[] t = stockHoldingsDisplay[globalStockNum-1].GetComponentsInChildren<Text>();
        // Update Price in the UI
        t[1].text = "@"+purchasePrice.ToString("F1");
        // And save to the Global Variable
        GlobalVariables.S.traderHoldings[traderNum, globalStockNum - 1] = purchasePrice;
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
            
            roundEarnings += sellPrice;
            // too high
            //earnedFunds += (netGain * 100f);
            earnedFunds += netGain;
            GameManager.S.roundWorth[traderNum] += earnedFunds;
            
            // Dynamically remove
            // int holdingsLocation = 0;
            // for(int i=0; i<holdings.Count; i++) {
            //     if(holdings[i].transform.Find("stockName").GetComponent<Text>().text == stockName){
            //         holdingsLocation = i;
            //     }
            // }
            // holdings.RemoveAt(holdingsLocation);
            // Destroy (holdingsParent.transform.GetChild (holdingsLocation).gameObject);
            
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
            stockHoldings[_stockNum] = 0;
            stocksSold++;
            GlobalVariables.S.traderRoundStats[traderNum, 1]++;
            
            // Get the global stock number
            int globalStockNum = StockManager.S.roundStocks[_stockNum].GetComponent<Stock>().stockNumber;
            Text[] t = stockHoldingsDisplay[globalStockNum-1].GetComponentsInChildren<Text>();
            // Update Price in the UI
            t[1].text = "NONE";
            // And Save to the Global Variable
            GlobalVariables.S.traderHoldings[traderNum, globalStockNum - 1] = 0;
            
            UpdateDisplay();
        } 
    }
    
    private void UpdateDisplay() {		
        // Amount earned
        // too low with non inflated stocks
        //earningsMeter.fillAmount = earnedFunds / 200f;
        earningsMeter.fillAmount = earnedFunds / 10f;

        int holdingIndex = 0;

        // Dynamically adding stocks to the UI
        // foreach (GameObject go in holdings) {
        //     go.transform.localPosition = new Vector3(90, -40 * holdingIndex, 0);
        //     go.transform.localScale = new Vector3(1, 1, 1);
        //     holdingIndex++;
        // }
        
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

    private IEnumerator UpdateTodaysStocks() {
        yield return new WaitForSeconds(0.1f);
        
        for (int i = 0; i < GlobalVariables.S.totalStocks; i++) {
            // stockIndexes stores the StockNumber for each day's active stocks
            Text[] t = stockHoldingsDisplay[i].GetComponentsInChildren<Text>();
            if (StockManager.S.stockIndexes.Contains(i)) {
                // Name
                t[0].color = new Color(1, 1, 0, 1);
                // Price
                t[1].color = Color.white;
            }
            else {
                // Name
                t[0].color = new Color(1, 1, 1, .25f);
                // Price
                t[1].color = new Color(1, 1, 1, .25f);
            }
            // Make sure price is current
            if (GlobalVariables.S.traderHoldings[traderNum, i] != 0) t[1].text = "@" + GlobalVariables.S.traderHoldings[traderNum, i];
            else t[1].text = "NONE";
        }

        yield return null;
    }
    
}
