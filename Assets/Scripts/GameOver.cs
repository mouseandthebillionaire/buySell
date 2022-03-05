using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    private int         winnerNum;
    public Text         winner, snarkText;
    private TextAsset   snarkCollection;
    private string[]    snark;

    private float       timePassed;
    public float        delayTime;
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(GetSnarkFromFile());
        
        //Save to a file if we are doing that
        FundManager.S.SaveWorth();

        // Who won?!
        winner.text = GlobalVariables.S.teamNames[GetWinner()];
        winner.color = GlobalVariables.S.traderColors[winnerNum];

        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Period))
        {
            StartCoroutine(Restart());
        }

        timePassed += Time.deltaTime;

        if (timePassed >= delayTime)
        {
            StartCoroutine(Restart());
        }

    }

    private IEnumerator Restart() {
        // Publish the final scores to the Text file
        FundManager.S.SaveWorth();
        yield return new WaitForSeconds(0.5f);
        // And Go to the Menu
        GameManager.S.Reset();
        SceneManager.LoadScene("Menu");
        yield return null;
    }
    
    IEnumerator GetSnarkFromFile() {
        snarkCollection = Resources.Load("endSnark") as TextAsset;
        snark = snarkCollection.text.Split ('#');
        
        int i = UnityEngine.Random.Range(0, snark.Length - 1);
        snarkText.text = snark[i];

        yield return null;
    }
    
        
    public int GetWinner() {
        float max = GlobalVariables.S.weeklyEarnings[0];
        winnerNum = 0;
        for (int i = 1; i < GlobalVariables.S.weeklyEarnings.Length; i++) {
            if (GlobalVariables.S.weeklyEarnings[i] > max) {
                max = GlobalVariables.S.weeklyEarnings[i];
                winnerNum = i;
            }
        }

        return winnerNum;
    }
}
