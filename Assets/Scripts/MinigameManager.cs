using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{

    [Header("99 = random / Any other # launches that particular minigame")]
    public int minigameChoice = 99;
    
    public GameObject[] minigames;
    public GameObject   traderInput;
    
    public int[]		inputKeys = new int[3];
    public int[] inputTimes = new int[3];
    public Text[]		inputDisplay;

    public static MinigameManager S;

    void Awake() {
        S = this;
    }
    
    
    // Start is called before the first frame update
    void Start() {
        // Clear the Keys
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            inputKeys[i] = 99;
            inputTimes[i] = 0;
        }
        
        // Choose the game
        // If it's 99 choice a random one, otherwise we're testing a specific one
        if(minigameChoice == 99) minigameChoice = UnityEngine.Random.Range(0, minigames.Length);

        // Start the game
        StartCoroutine(StartMinigame());
    }

    void Update() {
        // We're constantly looking to see what keys the players are pressing
        GetKeys();
    }
    
    
    private IEnumerator StartMinigame() {
        // Announce the game
        
        // Explain the game

        // Run the game
        minigames[minigameChoice].SetActive(true);

        traderInput.SetActive(true);

        yield return null;

    }
    
    private IEnumerator AnnounceMinigame() {
        yield return null;
    }
    
    private IEnumerator ExplainMinigame() {
        yield return null;
    }

    public void EndGame()
    {
        int winner = CalculateScore();
        BetweenerManager.S.AnnounceBonusWinner(winner);
    }
    
    public void UpdatePlayerScore(int _playerNum)
    {
        inputTimes[_playerNum]++;
        inputDisplay[_playerNum].text = inputTimes[_playerNum].ToString();
        BetweenerManager.S.coin.Play();

    }

    private int CalculateScore()
    {
        int highest = 0;
        int winner  = 99;
        for (int i = 0; i < inputTimes.Length; i++) {
            if (inputTimes[i] > highest) {
                highest = inputTimes[i];
                winner = i;
            }

            Debug.Log(winner);
        }
        return winner;
    }

    private void GetKeys() {
        for (int i = 0; i < 3; i++) {
            for(int j=0; j < 15; j++){
                if (Input.GetKeyDown(GlobalVariables.S.inputKeys[i, j])) {
                    inputKeys[i] = j;
                    Debug.Log(inputKeys[i]);
                }
            }
        }
    }
}
