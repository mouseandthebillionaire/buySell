using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Stock[]		tradingStocks;
	// set codes manually right now
	public string[]		stockCodes = new [] {"41", "32", "77", "14", "13", "99", "68", "24", "81"};
	public float		tradingSpeed;
	public float		gameLength;
	public float		interrupt = 11;
	public GameObject	minigame;
	public GameObject	countdown;
	public Text			countdownText;
	private int			countdownLength = 3;
	public AudioSource	blip;

	private float		resetTime, interruptTime;

	public static GameManager S;

	// Use this for initialization
	void Awake () {
		S = this;
	}

	void Start() {
		resetTime = Time.time;
		
		// set all the stocks manually for now
		SetStockCodes();
		
		GlobalVariables.S.gameState = 2;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - (resetTime + interruptTime) > interrupt) {
			StartCoroutine("LaunchMinigame");
		}
		
		if (Time.time - resetTime > gameLength) {
			if (GlobalVariables.S.gameState == 2) {
				StartCoroutine("CountDown");
				GlobalVariables.S.gameState = 4;
			}
		}
		
	}

	private void SetStockCodes() {
		for (int i = 0; i < tradingStocks.Length; i++) {
			tradingStocks[i].SetStockCode(stockCodes[i]);
		}
	}

	public void EffectStock(string direction, int stockNum) {
		if (direction == "up") {
			//tradingStocks[stockNum].Change("up");
			tradingStocks[stockNum].SupplyChange("up");
		}

		if (direction == "down") {
			//tradingStocks[stockNum].Change("down");
			tradingStocks[stockNum].SupplyChange("down");
		}
		// else we send stable
		else {
			return;
		}
		
	}

	public IEnumerator LaunchMinigame() {
		interruptTime = Time.time;
		GlobalVariables.S.gameState = 3;
		GlobalVariables.S.trading = false;
		minigame.SetActive(true);
		Minigame.S.ButtonMasher();
		
		yield return new WaitForSeconds(10);
		
		// Score minigame
		
		GlobalVariables.S.gameState = 2;
		GlobalVariables.S.trading = true;
		minigame.SetActive(false);

		yield return null;
	}
	
	public IEnumerator CountDown() {
		yield return new WaitForSeconds(1);
		countdown.SetActive(true);
		countdownText.text = countdownLength.ToString();
		countdownLength -= 1;
		blip.Play();
		if (countdownLength > 0) {
			StartCoroutine("CountDown");
		}
		else {
			GlobalVariables.S.GetWinner();
			yield return new WaitForSeconds(1);
			SceneManager.LoadScene("End");
		}
	}
}
