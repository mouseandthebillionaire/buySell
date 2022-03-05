using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculator : MonoBehaviour {
    
    public GameObject[]    scoreItems;

    private float          bonusProfit;
    public GameObject      totalBonus;
    public Text            profit, previousEarnings, score;

    private AudioSource    itemDisplayed, blip_1, counter, zilch, scorePosted;

    private int            traderNum;

    public static ScoreCalculator S;

    public void Awake() {
        S = this;
    }

    public void Start() {
        itemDisplayed = GameObject.Find("Coin").GetComponent<AudioSource>();
        blip_1 = GameObject.Find("Blip_1").GetComponent<AudioSource>();
        counter = GameObject.Find("Counter").GetComponent<AudioSource>();
        zilch = GameObject.Find("Zilch").GetComponent<AudioSource>();
        scorePosted = GameObject.Find("ScorePosted").GetComponent<AudioSource>();
    }

    public IEnumerator CalculateScore() {
        if(blip_1) blip_1.Play(); // sometimes it can't find this?
        traderNum = ScoreManager.S.scoresCalculated;
        bonusProfit = 0;
        
        yield return new WaitForSeconds(0.5f); 

        // Award "Bonus" profit for #bought and #sold
        for (int i = 0; i < scoreItems.Length; i++) {
            Text item = scoreItems[i].transform.GetChild(0).GetComponent<Text>();
            item.text = GlobalVariables.S.traderRoundStats[traderNum, i].ToString() + " =";
            float sum = GlobalVariables.S.traderRoundStats[traderNum, i] * (0.5f * (i + 1));
            bonusProfit += sum;
            Text value = scoreItems[i].transform.GetChild(1).GetComponent<Text>();
            value.text = "$" + sum.ToString("f2");
            if(bonusProfit > 0) itemDisplayed.Play();
            else zilch.Play();
            scoreItems[i].SetActive(true);   
            yield return new WaitForSeconds(0.5f); 

        }
                
        // total Bonus profit
        Text itemTotal = totalBonus.transform.GetChild(0).GetComponent<Text>();
        itemTotal.text = "$" + bonusProfit.ToString("F2");
        if(bonusProfit > 0) itemDisplayed.Play();
        else zilch.Play();
        totalBonus.SetActive(true);
        yield return new WaitForSeconds(0.5f); 
        
        // Round Profit
        float roundProfit = GameManager.S.roundWorth[traderNum];
        float displayProfit = 0;
        float increment = roundProfit / Random.Range(9.25f, 10f);
        while (displayProfit < roundProfit){
            displayProfit += increment;
            profit.text = "$" + displayProfit.ToString("F2");
            counter.Play();
            yield return new WaitForSeconds(.05f);
        }
        profit.text = "$" + roundProfit.ToString("F2");
        if(roundProfit <= 0) zilch.Play();
        yield return new WaitForSeconds(0.5f);
        
        
        // Previous profit
        float previousProfit = GlobalVariables.S.traderWorth[traderNum];
        displayProfit = 0;
        increment = previousProfit / Random.Range(9.25f, 10f);
        while (displayProfit < previousProfit){
            displayProfit += increment;
            previousEarnings.text = "$" + displayProfit.ToString("F2");
            counter.Play();
            yield return new WaitForSeconds(.05f);
        }
        
        previousEarnings.text = "$" + previousProfit.ToString("F2");
        if (previousProfit <= 0) zilch.Play();
        yield return new WaitForSeconds(0.5f);
        


        // Get Final Score
        float totalProfit = (roundProfit + bonusProfit + previousProfit);
        displayProfit = 0;
        increment = totalProfit / Random.Range(9.25f, 10f);
        while (displayProfit < totalProfit){
            displayProfit += increment;
            score.text = "$" + displayProfit.ToString("F2");
            counter.Play();
            yield return new WaitForSeconds(.05f);
        }
    
        score.text = "$" + totalProfit.ToString("F2");
        if(totalProfit <= 0) zilch.Play();
        yield return new WaitForSeconds(0.5f);

        // Add to the weekly profit
        GlobalVariables.S.weeklyEarnings[traderNum] += roundProfit;
        // Add to the overall profit
        GlobalVariables.S.traderWorth[traderNum] = totalProfit;
        // Add a delay to make sure this actually happens
        yield return new WaitForSeconds(0.5f);
        scorePosted.Play();
        StartCoroutine(ScoreManager.S.PostScore(totalProfit));
        
    }
}
