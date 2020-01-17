using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GlobalVariables : MonoBehaviour {

    public float[]         traderWorth;
    public int             winner;
    public bool            trading = true;
    public int             numRounds = 3;


    [Header("Set Dynamically")] 
    
    public int             stockCodeLength; // Set by round #
    public string[,]	   stockCodes  = new string[,] {
        {"1", "2", "3", "4", "5", "6", "7", "8", "9"},
        {"13", "37", "28", "87", "42", "91", "71", "57", "65"},
        {"143", "256", "312", "417", "562", "671", "714", "818", "945"}
    };

    public KeyCode[,]      inputKeys = new KeyCode[,] {
        {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O},
        {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L},
        {KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Comma, KeyCode.Period}
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

    public void Start() {
        Reset();
    }
    
    
    public void GetWinner() {
        float max = traderWorth[0];
        winner = 0;
        for (int i = 1; i < traderWorth.Length; i++) {
            if (traderWorth[i] > max) {
                max = traderWorth[i];
                winner = i;
            }
        }
        Debug.Log("The winner is " + winner);
    }

    public void Reset() {
                        
//        // Randomized Codes
//        // fill the possible stock codes for the current game
//        for (int i = 0; i < numRounds; i++) {
//            for (int j = 0; j < 9; j++) {
//                string randomizedCode = "";
//                for (int k = 0; k < stockCodeLength; k++) {
//                    float f = UnityEngine.Random.Range(0, 9);
//                    randomizedCode += f.ToString();
//                }
//                Debug.Log("Set " + i + " / Code " + j + " = " + randomizedCode);
//                stockCodes[i, j] = randomizedCode;
//            }
//            stockCodeLength++;
//        }

        gameState = 2;
    }
}
