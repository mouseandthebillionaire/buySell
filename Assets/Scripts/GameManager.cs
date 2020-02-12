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
	public int			roundLength;
	//public GameObject	minigame;
	public GameObject	countdown;
	public Text			countdownText;
	private int			countdownLength = 3;
	private bool		countingDown;
	public AudioSource	blip;	

	private float		resetTime;
	
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
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalVariables.S.trading) {
			if (Time.time - resetTime > roundLength && !countingDown) {
				StartCoroutine("CountDown");	
			}
		}

	}

	public void Reset() {
		resetTime = Time.time;
		countingDown = false;
		gameRound = 0;
		GlobalVariables.S.Reset();	
	}


	public void RoundOver() {
		GlobalVariables.S.trading = false;
		if (gameRound == GlobalVariables.S.numRounds - 1) {
			SceneManager.LoadScene("End");
		}
		else {
			SceneManager.LoadScene("Betweener");
		}
	}

	public void NextRound() {
		gameRound++;
		GlobalVariables.S.stockCodeLength = gameRound + 1;
		resetTime = Time.time;
		countingDown = false;
		countdownLength = 3;
		GlobalVariables.S.trading = true;
		SceneManager.LoadScene("Main");

	}
	
	public IEnumerator CountDown() {
		countingDown = true;
		yield return new WaitForSeconds(1);
		countdown.SetActive(true);
		countdownText.text = countdownLength.ToString();
		blip.Play();
		if (countdownLength > 0) {
			StartCoroutine("CountDown");
		}
		else {
			countdown.SetActive(false);
			RoundOver();
		}
		countdownLength -= 1;
	}
}
