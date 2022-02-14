using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockManager : MonoBehaviour {
    
    public Stock[]		    tradingStocks;
    public GameObject       stockPrefab;
    private GameObject      stockParent;
    public GameObject[]     roundStocks;
    public bool[]           stocksActive;
    // the stockIndexes keeps track of what stockNumbers are actually alive each day
    public List<int>        stockIndexes;
    
    // Keep Track of all the stock values
    public float[]          stockValues = new float[9];

    public static StockManager S;

    private void Awake() {
            S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetStockValues();
        LoadStocks();
        //SetStockCodes(GameManager.S.gameRound);

    }

    private void ResetStockValues()
    {
        for (int i = 0; i < GlobalVariables.S.totalStocks; i++)
        {
            stockValues[i] = 0;
        }
    }

    private void LoadStocks() {
        
        stockIndexes = new List<int>(GlobalVariables.S.roundStocks);
        roundStocks = new GameObject[GlobalVariables.S.roundStocks];

        
        for (int i = 0; i < GlobalVariables.S.roundStocks; i++) {
            int Rand = Random.Range(0, tradingStocks.Length);
            while (stockIndexes.Contains(Rand)){
                    Rand = Random.Range(0, tradingStocks.Length);
                }
            stockIndexes.Add(Rand);
            GameObject s = GameObject.Instantiate(stockPrefab) as GameObject;
            s.transform.parent = this.transform;
            Stock ss = s.GetComponent<Stock>();
            ss.stockName = GlobalVariables.S.stockNames[Rand];
            ss.stockNumber = Rand + 1;
            ss.SetStockCode(GlobalVariables.S.stockCodes[GameManager.S.gameRound, Rand]);
            
            s.transform.localPosition = new Vector3(140 * i - 700, 0, 0);
            s.transform.localScale = new Vector3(1, 1, 1);
            
            
            roundStocks[i] = s;
            
        }
    }
    
    private void SetStockCodes(int _round) {
        for (int i = 0; i < roundStocks.Length; i++) {
            tradingStocks[i].SetStockCode(GlobalVariables.S.stockCodes[_round, i]);
        }

        GlobalVariables.S.stockCodeLength = (_round + 1);
    }
    
    public IEnumerator EffectStock(string direction, int stockNum) {
        if (stockIndexes.Contains(stockNum)) {
            int i = stockIndexes.IndexOf(stockNum);
            //tradingStocks[stockNum].makeHot(direction);
            Stock s = roundStocks[i].GetComponent<Stock>();
            StartCoroutine(s.newsEffect(direction));
        }

        yield return null;

    }
}
