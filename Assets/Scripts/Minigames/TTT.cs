using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTT : MonoBehaviour {
    private string words = "DOGGOS,FRIEND,MONKEY,BANANA,SECRET,CIRCUS,SCHOOL,TURTLE,POTATO,PIRATE,DRAGON,PICKLE";
    private string wordCodes = "364467,374363,666539,226262,732738,247287,724665,887853,768286,747283,372466,742553";
    public int timeLimit;
    private float timer;

    private int wordLength = 6;
    private int[] currChar = new int[3];
    private bool[] erred = new bool[3];
    private string[] wordLetters = new string[6];
    private int[] wordNumbers = new int[6];
    private int[,] wordCodeInputs = new int[3,6];
    private GameObject[] displayLetters = new GameObject[6];
    private bool spelling;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Choose the word
        string[] wordList = words.Split(',');
        string[] codeList = wordCodes.Split(',');
        int wordIndex = Random.Range(0, wordList.Length);
        string word = wordList[wordIndex];
        string code = codeList[wordIndex];
        Debug.Log(code);
        // Split the word
        for (int i = 0; i<wordLength; i++) {
            wordLetters[i] = word[i].ToString();
            wordNumbers[i] = int.Parse(code[i].ToString());
        }
        
        // Reset everyone
        for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
            MinigameManager.S.inputKeys[i] = 99;
            currChar[i] = 0;
            erred[i] = false;
        }

        spelling = true;
        
        // Display the letters
        for (int i = 0; i < wordLength; i++)
        {
            displayLetters[i] = GameObject.Find("letter_" + i);
            Text t = displayLetters[i].GetComponent<Text>();
            t.text = wordLetters[i];
        }

        StartCoroutine(RunGame());
    }

    // Update is called once per frame
    private IEnumerator RunGame() {
        int numOut = 0;

        // Get the time this launched
        float startTime = Time.time;

        while (timer < timeLimit && spelling){

            timer  = Time.time - startTime;

            for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
                if (!erred[i] && MinigameManager.S.inputKeys[i] != 99) {

                    if (MinigameManager.S.inputKeys[i] == wordNumbers[currChar[i]]) {
                        // Display that they got it
                        string     obj_loc = "Word/letter_" + currChar[i] + "/p" + i;
                        GameObject p       = GameObject.Find(obj_loc);
                        p.GetComponent<Image>().color = GlobalVariables.S.traderColors[i];
                        // Reset the key
                        MinigameManager.S.inputKeys[i] = 99;
                        MinigameManager.S.UpdatePlayerScore(i);
                        // Move to the next letter
                        currChar[i]++;
                    }
                    else {
                        // They typed the wrong letter! Kick 'em out!
                        erred[i] = true;
                        MinigameManager.S.traderInput.transform.GetChild(i).GetComponent<Image>().color =
                            Color.grey;
                        BetweenerManager.S.zilch.Play();
                        numOut++;
                    }

                    if (currChar[i] == wordLength) {
                        spelling = false;
                        BetweenerManager.S.AnnounceBonusWinner(i);
                        
                    }
                }

            }

            if (numOut >= 3 || timer > timeLimit) {
                    spelling = false;
                    BetweenerManager.S.AnnounceBonusWinner(99);
            }

            yield return null;
        }

    }
}
