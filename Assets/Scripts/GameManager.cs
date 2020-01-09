using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Stock[]		tradingStocks;
	// set codes manually right now
	public string[,]	stockCodes = new [,] {
		{"1", "2", "3", "4", "5", "6", "7", "8", "9"},
		{"41", "32", "77", "14", "13", "99", "68", "24", "81"},
		{"187", "576", "369", "562", "714", "917", "213", "143", "818"}
	};
	public float		tradingSpeed;
	public float		gameLength;
	public int			gameRound;
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
		SetStockCodes(gameRound);

		GlobalVariables.S.Reset();
		
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

	private void SetStockCodes(int _round) {
		for (int i = 0; i < tradingStocks.Length; i++) {
			tradingStocks[i].SetStockCode(stockCodes[_round, i]);
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
