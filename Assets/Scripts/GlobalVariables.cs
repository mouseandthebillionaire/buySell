using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

    public float[]         traderWorth;
    public int             winner;
    public bool            trading = true;
    public int             gameState; // 0 = menu, 1 = loading, 2 = playing, 3 = minigame, 4 = gameOver
    
    public static GlobalVariables S;
    
    
    // Start is called before the first frame update
    void Awake()
    {
       DontDestroyOnLoad(this);
       if (S == null) {
           S = this;
       } else {
           DestroyObject(gameObject);
       }
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
}
