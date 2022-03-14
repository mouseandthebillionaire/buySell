using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    private int         winnerNum;
    public GameObject   winner;
    public Image        winnerPanel;
    public Text         winnerName, snarkText;
    public AudioSource  endSong;
    private TextAsset   snarkCollection;
    private string[]    snark;

    public static GameEnd S;

    void Awake()
    {
        if (S == null) {
            S = this;
        } else {
            DestroyObject(gameObject);
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Menu");
        if(winner) winner.SetActive(true);
        StartCoroutine(GetSnarkFromFile());
        
        //Save to a file if we are doing that
        FundManager.S.SaveWorth();
        
        // Announce Winner
        StartCoroutine(AnnounceWinner());

    }
    
    private IEnumerator GetSnarkFromFile() {
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
            // Update LastWeeksEarning to prep for Menu screen
            GlobalVariables.S.lastWeeksEarnings[i] = GlobalVariables.S.weeklyEarnings[i];
            
            // Calculate Highest Score
            if (GlobalVariables.S.weeklyEarnings[i] > max) {
                max = GlobalVariables.S.weeklyEarnings[i];
                winnerNum = i;
            }
        }

        return winnerNum;
    }

    public IEnumerator AnnounceWinner()
    {
        StartCoroutine(GetSnarkFromFile());

        //Save to a file if we are doing that
        FundManager.S.SaveWorth();

        // Who won?!
        winnerName.text = GlobalVariables.S.teamNames[GetWinner()];
        winnerPanel.color = GlobalVariables.S.traderColors[winnerNum];

        endSong.Play();
        
        GameManager.S.Reset();
        yield return new WaitForSeconds(5);
        winner.SetActive(false);

    }
}
