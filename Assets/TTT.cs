using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTT : MonoBehaviour {
    public string words = "DOGGOS";
    public string wordCodes = "364467";
    public int timeLimit = 60;

    private int wordLength = 6;
    private int[,] wordCodeInputs = new int[3, 6];
    private int[] currChar = new int[3];
    private bool[] erred = new bool[3];

    private string[] inputString = new string[3];

    
    // Start is called before the first frame update
    void Start()
    {
        // Reset everyone
        for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
            MinigameManager.S.inputKeys[i] = 99;
            inputString[i] = "";
            currChar[i] = 0;
            erred[i] = false;
            
            for (int j = 0; j < wordLength; j++) {
                wordCodeInputs[i, j] = int.Parse(wordCodes[j].ToString());
            }
        }


        StartCoroutine(RunGame());
    }

    // Update is called once per frame
    private IEnumerator RunGame() {
        string currentWord = words;
        
        float duration = Time.time + timeLimit;

        while (duration > Time.time) {
            for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
                if (!erred[i] && MinigameManager.S.inputKeys[i] != 99) {
                    
                    if (MinigameManager.S.inputKeys[i] == wordCodeInputs[i, currChar[i]]) {
                        Debug.Log("Correct Letter");
                        // Reset the key
                        MinigameManager.S.inputKeys[i] = 99;
                        MinigameManager.S.UpdatePlayerScore(i);
                        // Move to the next letter
                        currChar[i]++;
                    }
                    else {
                        // They typed the wrong letter! Kick 'em out!
                        erred[i] = true;
                        MinigameManager.S.traderInput.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
                        BetweenerManager.S.zilch.Play();
                    }

                    if (currChar[i] == wordLength) {
                        BetweenerManager.S.AnnounceBonusWinner(i);
                    }    
                }
            }

            yield return null;
        }
    }
}
