using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TraderLogin : MonoBehaviour {
    public KeyCode enter;
    private Image panel;
    public Color logged_onColor;

    private bool entered;
    
    // Start is called before the first frame update
    void Start() {
        panel = GetComponentInChildren<Image>();
        panel.color = new Color(1, 1, 1, 0.5f);

        StartCoroutine(DisplayWorth());
        
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(enter)) {
            if (!entered) {
                panel.color = logged_onColor;
                Login.S.AddTrader();
                entered = true;
            }
        }
    }

    public IEnumerator DisplayWorth() {
        yield return new WaitForSeconds(0.05f);
        int traderNum = int.Parse(this.name);

        // Finding the Text to display funds based on its name
        GameObject funds        = GameObject.Find(traderNum + "/CurrentFunds");
        Text       fundsDisplay = funds.GetComponent<Text>();
        fundsDisplay.text = "TOTAL FUND VALUE: $" + GlobalVariables.S.traderWorth[traderNum];
        
        // Finding the Text to display last weeks earnings
        GameObject lastWeek        = GameObject.Find(traderNum + "/LastWeek$");
        Text       lastWeekText = lastWeek.GetComponent<Text>();
        float LWE = GlobalVariables.S.lastWeeksEarnings[traderNum];
        if (LWE >= 0) lastWeekText.text = "+" + LWE.ToString(".0##");
        else lastWeekText.text = LWE.ToString(".0##");
        
        // Stocks Bought
        GameObject b        = GameObject.Find(traderNum + "/Bought$");
        Text       bDisplay = b.GetComponent<Text>();
        bDisplay.text = GlobalVariables.S.lastWeekStats[traderNum, 0].ToString();
        Debug.Log(GlobalVariables.S.lastWeekStats[traderNum, 0].ToString());
        
        // Stocks Sold
        GameObject s        = GameObject.Find(traderNum + "/Sold$");
        Text       sDisplay = s.GetComponent<Text>();
        sDisplay.text = GlobalVariables.S.lastWeekStats[traderNum, 1].ToString();
        
        // Random "Volume" Number
        GameObject volume        = GameObject.Find(traderNum + "/Volume$");
        Text       volumeDisplay = volume.GetComponent<Text>();
        float vol = Random.Range(1000000, 9999999);
        volumeDisplay.text = vol.ToString("#,###0");

        yield return null;
    }
}
