using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {

	public float				playerFunds;
	
	private bool[]				hasStock = new bool[3];
	private float[]				stockPrice = new float[3];

	public Text					fundsDisplay;
	public Text					holdingsDisplay;

	public KeyCode[]			buyKeys;
	public KeyCode[]			sellKeys;

	public static PlayerInput	S;
	
	// Use this for initialization
	void Start () {
		S = this;
		playerFunds = 10f;
		for (int i = 0; i < GameManager.S.tradingStocks.Length; i++) {
			hasStock[i] = false;
			UpdateDisplay();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < GameManager.S.tradingStocks.Length; i++) {
			if (Input.GetKeyDown(buyKeys[i])) {
				Buy(i);
			}
			if (Input.GetKeyDown(sellKeys[i])) {
				Sell(i);

			}
		}
		
		

		// Temporary Watson Control
//		if(Watson_STT.S.spokenText == "buy"){
//			Buy(0);
//		}

		
		
	}

	public void Buy(int _stockNum) {
		Stock _stb = GameManager.S.tradingStocks[_stockNum];
		float purchasePrice = _stb.value;
		playerFunds -= purchasePrice;
		stockPrice[_stockNum] = _stb.value;
		hasStock[_stockNum] = true;
		UpdateDisplay();
	}
	
	public void Sell(int _stockNum) {
		if (hasStock[_stockNum]) {
			Stock _stb = GameManager.S.tradingStocks[_stockNum];
			float sellPrice = _stb.value;
			float netGain = sellPrice - stockPrice[_stockNum];
			playerFunds += netGain;
			hasStock[_stockNum] = false;
			UpdateDisplay();
		} else {
			Debug.Log("You don't have any to sell");
		}
	}

	private void UpdateDisplay() {		
		float displayFunds = playerFunds * 100;

		string displayTextString = "";

		for (int i = 0; i < GameManager.S.tradingStocks.Length; i++) {
			displayTextString += GameManager.S.tradingStocks[i].displayName.text;
			if (hasStock[i]) {
				float displayPrice = stockPrice[i] * 100;
				displayTextString += " atttt $" + displayPrice.ToString("F2");
			}
			else {
				displayTextString += " @ NONE";
			}

			displayTextString += "	//	";
		}

		holdingsDisplay.text = displayTextString;

		fundsDisplay.text = "Fund: $" + displayFunds.ToString("F2");

	}
}
