using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//public Stock[]		tradingStocks;

	public float		tradingSpeed;
	public int			gameRound;
	public float[]      roundWorth = new float[3];
	public int			roundLength;
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

	void Start()
	{
		UnityEngine.Cursor.visible = false;
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalVariables.S.trading) {
			if (Time.time - resetTime > roundLength && !countingDown) {
				StartCoroutine("CountDown");	
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Reset();
			SceneManager.LoadScene("Menu");
		}
	}

	public void Reset(){
		gameRound = 0;
		GlobalVariables.S.NewWeek();
	}

	public void StartGame() {
		StartCoroutine(PrepForRound());
	}


	public void RoundOver() {
		GlobalVariables.S.trading = false;
		SceneManager.LoadScene("Betweener");
	}

	public void NextRound() {
		gameRound++;
		GlobalVariables.S.stockCodeLength = gameRound + 1;		
		StartCoroutine(PrepForRound());
	}

	// This happens every time we are going into a "Trading" phase
	private IEnumerator PrepForRound() {
		// Reset Trader's Current Worth
		for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
			roundWorth[i] = 0;
			for(int j=0; j < 2; j++){
				// Reset the round stats
				GlobalVariables.S.traderRoundStats[i, j] = 0;
			}
		}
		resetTime = Time.time;
		countingDown = false;
		countdownLength = 3;
		GlobalVariables.S.trading = true;
			
		SceneManager.LoadScene("Main");
		yield return null;
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

	// End of the Week - Announce Winner and Reset
	public void WeekEnd(){
		GameEnd.S.EndGame();
	}
}
