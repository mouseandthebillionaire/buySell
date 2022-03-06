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

    // Minigame Components
    private Image       logo, background;
    private Text        description;

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

        if (GlobalVariables.S.minigameIndexes.Count == 0)
        {
            // fill with an index of minigames
            for (int i = 0; i < minigames.Length; i++)
            {
                GlobalVariables.S.minigameIndexes.Add(i);
            }
        }

        StartCoroutine(ChooseMinigame());
        
    }

    void Update() {
        // We're constantly looking to see what keys the players are pressing
        GetKeys();

    }
    
    
    private IEnumerator StartMinigame() {
        // Announce the game
        
        // Explain the game

        // Run the game

        traderInput.SetActive(true);

        yield return null;

    }

    private IEnumerator ChooseMinigame()
    {
        // Choose the game
        // If it's 99 choice a random one, otherwise we're testing a specific one
        if (minigameChoice == 99)
        {
            int i = Random.Range(0, GlobalVariables.S.minigameIndexes.Count);
            // Get the Index Number of the chosen minigame
            minigameChoice = GlobalVariables.S.minigameIndexes[i];
            GlobalVariables.S.minigameIndexes.Remove(minigameChoice);
        }
        
        // Activate the minigame GameObject
        minigames[minigameChoice].SetActive(true);
        
        // Get all the Minigame Components
        Image[] ims = minigames[minigameChoice].GetComponentsInChildren<Image>();

        // Announce the game
        StartCoroutine(AnnounceMinigame());

        yield return null;
    }
    
    private IEnumerator AnnounceMinigame() {
        
        // Start the game
        StartCoroutine(StartMinigame());
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
        }
        return winner;
    }

    private void GetKeys() {
        for (int i = 0; i < 3; i++) {
            for(int j=0; j < 15; j++){
                if (Input.GetKeyDown(GlobalVariables.S.inputKeys[i, j])) {
                    inputKeys[i] = j;
                }
            }
        }
    }
}
