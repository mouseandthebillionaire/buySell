using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//public Stock[]		tradingStocks;

	public float		tradingSpeed;
	public float		gameLength;
	public int			gameRound;
	public float		interrupt = 11;
	//public GameObject	minigame;
	public GameObject	countdown;
	public Text			countdownText;
	private int			countdownLength = 3;
	public AudioSource	blip;

	private float		resetTime, interruptTime;

	public static GameManager S;

	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad(this);
		if (S == null) {
			S = this;
		} else {
			DestroyObject(gameObject);
		}
       
	}

	void Start() {
		resetTime = Time.time;
		gameRound = 1;		
		
		// set all the stocks codes
		//SetStockCodes(gameRound);

		//GlobalVariables.S.Reset();
		
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


	public IEnumerator LaunchMinigame() {
		interruptTime = Time.time;
		GlobalVariables.S.gameState = 3;
		GlobalVariables.S.trading = false;
		//minigame.SetActive(true);
		Minigame.S.ButtonMasher();
		
		yield return new WaitForSeconds(10);
		
		// Score minigame
		
		GlobalVariables.S.gameState = 2;
		GlobalVariables.S.trading = true;
		//minigame.SetActive(false);

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
