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
		resetTime = Time.time;

		//eventually the game will start in round 0, but for testing comment out to launch in different rounds
		//stockCodeLength is dynamically set to gameRound + 1;
		//gameRound = 0;		

		//GlobalVariables.S.Reset();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalVariables.S.trading) {
			if (Time.time - resetTime > roundLength) {
				GlobalVariables.S.trading = false;	
				RoundOver();
			}
			
			if (Time.time - resetTime > gameLength) {
				if (GlobalVariables.S.gameState == 2) {
					StartCoroutine("CountDown");
					GlobalVariables.S.gameState = 4;
				}
			}
		}

		
	}


	public void RoundOver() {
		SceneManager.LoadScene("Betweener");
	}

	public void NextRound() {
		Debug.Log("did this happen?");
		gameRound++;
		resetTime = Time.time;
		GlobalVariables.S.trading = true;
		SceneManager.LoadScene("Main");

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
