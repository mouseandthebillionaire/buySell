﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = System.Random;

public class GlobalVariables : MonoBehaviour {

    public float[]         traderWorth;
    public int             winner;
    public bool            trading = true;
    public int             numRounds = 3;
    public int             numTraders = 3;


    [Header("Set Dynamically")] 
    
    public int             stockCodeLength; // Set by round #
    public string[,]	   stockCodes  = new string[,] {
        {"1", "2", "3", "4", "5", "6", "7", "8", "9"},
        {"13", "37", "28", "87", "42", "91", "71", "57", "65"},
        {"143", "256", "312", "417", "562", "671", "714", "818", "945"}
    };

    // Input Keys
    // 0-9 tied to numbers
    // Star (10), Pound(11), Pickup(12), Hangup(13), Yell(14)
    
    
    public KeyCode[,]      inputKeys = new KeyCode[,] {
        {KeyCode.X, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Q,    // 0-4
         KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D,                   // 5-9
         KeyCode.Z, KeyCode.C, KeyCode.Exclaim, KeyCode.At, KeyCode.Hash},        // Star, Pound, Up, Down, Yell
        
        {KeyCode.B, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.R, 
         KeyCode.T, KeyCode.Y, KeyCode.F, KeyCode.G, KeyCode.H, 
         KeyCode.V, KeyCode.N, KeyCode.Dollar, KeyCode.Percent, KeyCode.Caret},
        
        
        {KeyCode.Comma, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.U, 
         KeyCode.I, KeyCode.O, KeyCode.J, KeyCode.K, KeyCode.L, 
         KeyCode.M, KeyCode.Period, KeyCode.Ampersand, KeyCode.Asterisk, KeyCode.LeftParen}
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
