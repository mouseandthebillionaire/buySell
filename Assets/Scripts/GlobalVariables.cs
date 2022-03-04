using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

    public float[]         traderWorth;
    public int             winner;
    public bool            trading = true;
    public int             numRounds = 3;
    public int             totalStocks = 9;
    public int             roundStocks = 5;
    public int             numTraders = 3;
    public Color[]         traderColors;
    
    // Hold Round specific variables [stocksBought / stocksSold]
    public int[,]          traderRoundStats = new int[3, 2];
    // Keep track of Trader Holdings
    public float[,]        traderHoldings = new float[3, 9];
    // Keep track of each playthrough's earnings
    public float[]        weeklyEarnings = new float[3];

    [Header("Set Dynamically")] 
    public int             stockCodeLength; // Set by round #
    
    public string[,]	   stockCodes  = new string[3,9]{
        {"1", "2", "3", "4", "5", "6", "7", "8", "9"},
        {"13", "37", "28", "87", "42", "91", "71", "57", "65"},
        {"143", "256", "312", "417", "562", "671", "714", "818", "945"}
    };

    public string[]        stockNames = new string[9]{
        "???", "APLS", "DMND", "GLD", "JOOC", "MOMS", "PZA", "TKOS", "WSHS"
    };
    
    public string[]        shortTeamNames = new string[3]{"NNF", "YU", "RRR"};

    public string[]        teamNames = new string[3]{
        "Necktie, Necktie and Fleece", "Young Upstarts", "Rose and Rosen Rose"
    };


    // Input Keys
    // 0-9 tied to numbers
    // Star (10), Pound(11), Pickup(12), Hangup(13), Yell(14)

    public string[]        keyNames = new string[]
        {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "#", "⬆", "⬇", "BUY!"};
        
    public KeyCode[,]      inputKeys = new KeyCode[,] {
        {KeyCode.X, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Q,    // 0-4
         KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D,                   // 5-9
         KeyCode.Z, KeyCode.C, KeyCode.Alpha0, KeyCode.P, KeyCode.Semicolon},     // Star, Pound, Up, Down, Yell
        
        {KeyCode.B, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.R, 
         KeyCode.T, KeyCode.Y, KeyCode.F, KeyCode.G, KeyCode.H, 
         KeyCode.V, KeyCode.N, KeyCode.Less, KeyCode.LeftBracket, KeyCode.Slash},
        
        
        {KeyCode.Comma, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.U, 
         KeyCode.I, KeyCode.O, KeyCode.J, KeyCode.K, KeyCode.L, 
         KeyCode.M, KeyCode.Period, KeyCode.Equals, KeyCode.RightBracket, KeyCode.BackQuote}
    };

    public int             gameState; // 0 = menu, 1 = loading, 2 = playing, 3 = minigame, 4 = gameOver
    
    public static GlobalVariables S;
    
    
    // Start is called before the first frame update
    void Awake()
    {
       if (S == null) {
           S = this;
       } else {
           DestroyObject(gameObject);
       }
    }

    public void NewGame() {
        gameState = 0;
        stockCodeLength = 1;
        for (int i = 0; i < numTraders; i++) {
            // Reset trader's weekly earnings to $0
            weeklyEarnings[i] = 0;
            // Pull their overall funds from a saved file...
            FundManager.S.LoadWorth();
            // And remove all of their holdings
            for (int j = 0; j < 9; j++){
                // Reset the HOLDINGS
                traderHoldings[i, j] = 0;
            }

            Debug.Log("Trader #" + i + "is worth " + traderWorth[i]);
        }
    }

    // We need to do some management when there's a new day
    public void NewDay()
    {
        gameState = 2;
        for (int i = 0; i < numTraders; i++) {
            // Reset the Round Stats for BOUGHT and SOLD
            traderRoundStats[i, 0] = 0;
            traderRoundStats[i, 1] = 0;
        }
       
    }
}
