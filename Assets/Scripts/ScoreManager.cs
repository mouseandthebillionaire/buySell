using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    // Score Calculators
    public ScoreCalculator[]	scoreCalculators;
    public int					scoresCalculated;
    
    public GameObject[]	traderWorthDisplay;
    
    public static ScoreManager S;

    void Awake() {
        S = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        scoresCalculated = 0;
        StartCoroutine(ScoreCalculation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator ScoreCalculation() {
        scoreCalculators[scoresCalculated].transform.localPosition = new Vector3(-100, 200, 0);
        StartCoroutine(scoreCalculators[scoresCalculated].CalculateScore());
        yield return null;

    }
    
    public IEnumerator PostScore(float score) {
        traderWorthDisplay[scoresCalculated].transform.localPosition = new Vector3(-300 + (300 * scoresCalculated), 50, 0);
        Text t = traderWorthDisplay[scoresCalculated].GetComponentInChildren<Text>();
        t.text = "$" + score.ToString("F2");
        scoreCalculators[scoresCalculated].transform.localPosition = new Vector3(-100, 1000, 0);

        yield return new WaitForSeconds(0.5f);

        scoresCalculated++;
        if (scoresCalculated == GlobalVariables.S.numTraders) {
            //Save to a file if we are doing that
            FundManager.S.SaveWorth();
            if (GameManager.S.gameRound == GlobalVariables.S.numRounds - 1) {
                SceneManager.LoadScene("End");
            } else {
                BetweenerManager.S.Next();
            }
        } else {
            StartCoroutine(ScoreCalculation());
        }

        yield return null;

    }
}
