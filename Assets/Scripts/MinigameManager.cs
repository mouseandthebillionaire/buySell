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
        
        // If it's 99 choice a random one, otherwise we're testing a specific one
        if(minigameChoice == 99) minigameChoice = UnityEngine.Random.Range(0, minigames.Length);

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
