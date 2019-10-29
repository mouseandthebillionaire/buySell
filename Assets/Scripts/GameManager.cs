using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Stock[]		tradingStocks;
	public float		tradingSpeed;
	public float		gameLength;
	public GameObject	countdown;
	public Text			countdownText;
	private int			countdownLength = 3;
	public AudioSource	blip;

	private float		resetTime;

	public static GameManager S;

	// Use this for initialization
	void Awake () {
		S = this;
	}

	void Start() {
		resetTime = Time.time;
		GlobalVariables.S.gameState = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - resetTime > gameLength) {
			if (GlobalVariables.S.gameState == 2) {
				StartCoroutine("CountDown");
				GlobalVariables.S.gameState = 3;
			}
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
