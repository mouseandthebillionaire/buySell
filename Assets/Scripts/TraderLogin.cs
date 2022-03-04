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
        int traderNum = int.Parse(this.name);
        Debug.Log(traderNum);

        GameObject funds = GameObject.Find("CurrentFunds_" + traderNum);
        Text fundsDisplay = funds.GetComponent<Text>();
        fundsDisplay.text = "Current Funds: \n$" + GlobalVariables.S.traderWorth[traderNum];        
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
}
