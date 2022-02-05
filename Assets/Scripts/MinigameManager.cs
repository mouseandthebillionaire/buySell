using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour {
    
    public GameObject[] minigames;
    public GameObject   traderInput;
    
    public int[]		inputKeys = new int[GlobalVariables.S.numTraders];
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
        }
        
        StartCoroutine(StartMinigame());
    }

    void Update() {
        GetKeys();
    }

    private IEnumerator StartMinigame() {
        // int minigameChoice = UnityEngine.Random.Range(0, minigames.Length);
        // for testing - force minigame choice
        int minigameChoice = 1;
        minigames[minigameChoice].SetActive(true);

        traderInput.SetActive(true);

        yield return null;

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
