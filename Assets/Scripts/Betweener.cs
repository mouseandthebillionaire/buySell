// using System.Collections;
// using UnityEngine;
// using UnityEngine.Analytics;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
//
// public class Betweener: MonoBehaviour {
//
// 	public int			correctKey;
// 	public GameObject	traderInput, winner, loading, bonusAnnouncement;
// 	public Text[]		inputDisplay;
// 	public GameObject[]	miniGames;
// 	public Sprite[]		splats;
//
// 	public GameObject	scoreCalculator;
// 		
// 	private int			minigameChoice;
// 	private int			breakTime = 10;
// 	private float		startTime;
//
// 	private bool		breakin;
// 	//private bool[]		handsetUp = new bool[GlobalVariables.S.numTraders];
// 	public int[]		inputTimes;
// 	//public GameObject[]	traderWorthDisplay;
// 	
// 	//private int[]		inputKeys = new int[GlobalVariables.S.numTraders];
//
// 	// Maths Setup
// 	private string[]	equations = new string[10] {
// 		"(4 - (2 x 2)) x (12 x 6 / 3)",
// 		"((1 + 1) x 100) / (50 x 4)",
// 		"((40 / 8) x 2) / 5",
// 		"(1 + 2 + 3) / 2",
// 		"4 + (12/2) - 6",
// 		"2 + 1 + 1 + 1",
// 		"1 + 2 + 2 + 1",
// 		"1 + 1 + 1 + 1 + 1 + 2",
// 		"(1 + 1) x 4",
// 		"2 + 2 + 2 + 2 + 1"
// 	};
//
// 	// BugSquashing Setup
// 	public GameObject[]	bugKeys = new GameObject[12];
//
// 	public AudioSource	ring, blip, splatSound, error, ding, bonusOpportunity, bonusWon, minigame, zilch;
//
// 	public static Betweener	S;
//
// 	void Awake() {
// 		S = this;
// 	}
//
// 	void Start() {
// 		// Clear Trader Input
// 		for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
// 			inputKeys[i] = 99;
// 		}
// 		correctKey = Random.Range(0, 9);	
// 		
// 		// Run the Score Calculator
// 		scoreCalculator.SetActive(true);
// 	}
// 	
// 	// Update is called once per frame
//     void Update() {
//
// 		//GetKeys();
// 	}
//
// 	// 
// 	//
// 	// private IEnumerator Maths() {
// 	// 	// Unlike the button masher, no timer is used here. Just who answers first.
// 	// 	bool unanswered = true;
// 	// 	int numAnswered = 0;
// 	// 	
// 	// 	// Don't let them answer if they have entered the wrong answer
// 	// 	bool[] answered = new bool[GlobalVariables.S.numTraders];
// 	// 	for (int i = 0; i < answered.Length; i++) {
// 	// 		answered[i] = false;
// 	// 		inputKeys[i] = 99;
// 	// 	}
// 	// 	
// 	// 	GameObject go_0 = GameObject.Find(miniGames[minigameChoice].name + "/Directions");
// 	// 	GameObject go_1 = GameObject.Find(miniGames[minigameChoice].name + "/Equation");
// 	// 	Text directions = go_0.GetComponent<Text>();
// 	// 	Text equation = go_1.GetComponent<Text>();
// 	// 	directions.text = "EQUATION SENT: SOLVE THE FOLLOWING. IT MAY UNCOVER AN INVESTMENT OPPORTUNITY \n\n";
// 	// 	equation.text = equations[correctKey];
// 	// 	
// 	// 	while(unanswered) {
// 	// 		for (int i = 0; i < inputKeys.Length; i++) {
// 	// 			if (!answered[i] && inputKeys[i] == correctKey) {
// 	// 				Debug.Log("Trader #" + i + " enters the correct answer!");
// 	// 				unanswered = false;
// 	// 				StartCoroutine(AnnounceBonusWinner(i));
// 	// 			} else {
// 	// 				if (!answered[i] && 
// 	// 					inputKeys[i] != 99 && 
// 	// 					inputKeys[i] != 11 &&
// 	// 					inputKeys[i] != 12 &&
// 	// 					inputKeys[i] != 13) {
// 	// 					// They answered incorrectly
// 	// 					answered[i] = true;
// 	// 					traderInput.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
// 	// 					zilch.Play();
// 	// 					numAnswered++;
// 	// 					// they all go it wrong
// 	// 					if (numAnswered == GlobalVariables.S.numTraders) {
// 	// 						// Trader 3 means no one won
// 	// 						StartCoroutine(AnnounceBonusWinner(3));
// 	// 					}
// 	// 				}
// 	// 			}
// 	// 		}
// 	// 		
// 	// 		yield return null;
// 	// 	}
// 	// }
// 	//
// 	// private IEnumerator BugSquasher() {
// 	// 	// overall time based
// 	// 	int minigameLength = 20;
// 	// 	float duration = Time.time + minigameLength;
// 	// 	
// 	// 	// game variables
// 	// 	float bugFrequency = 1f;
// 	// 	bool bugPresent = false;
// 	// 	for (int i = 0; i < bugKeys.Length; i++) {
// 	// 		GameObject go = GameObject.Find(bugKeys[i].name + "/bug");
// 	// 		go.GetComponent<Image>().color = Color.clear;
// 	// 	}
// 	//
// 	// 	while (duration > Time.time) {
// 	// 		if (!bugPresent) {
// 	// 			Debug.Log("We're bugging");
// 	// 			correctKey = Random.Range(0, bugKeys.Length);
// 	// 			GameObject go = GameObject.Find(bugKeys[correctKey].name + "/bug");
// 	// 			go.GetComponent<Image>().color = Color.white;
// 	// 			bugPresent = true;
// 	// 		}
// 	// 		
// 	// 		for (int i = 0; i < inputKeys.Length; i++) {
// 	// 			if (inputKeys[i] == correctKey) {
// 	// 				inputTimes[i] += 1;
// 	// 				inputDisplay[i].text = inputTimes[i].ToString();
// 	// 				GameObject go = GameObject.Find(bugKeys[correctKey].name + "/bug");
// 	// 				go.GetComponent<Image>().color = Color.clear;
// 	//
// 	// 				// Splat
// 	// 				GameObject splat = GameObject.Find(bugKeys[correctKey].name + "/splat");
// 	// 				splat.GetComponent<Image>().sprite = splats[Random.Range(0, splats.Length)];
// 	// 				splat.GetComponent<Image>().color = GlobalVariables.S.traderColors[i];
// 	// 				splatSound.Play();
// 	// 				
// 	// 				inputKeys[i] = 99;
// 	//
// 	// 				// Only show splay for half a second
// 	// 				yield return new WaitForSeconds(0.5f);
// 	// 				splat.GetComponent<Image>().color = Color.clear;
// 	// 				
// 	// 				yield return new WaitForSeconds(bugFrequency);
// 	// 				bugFrequency -= 0.05f;
// 	// 				bugPresent = false;
// 	// 			}
// 	// 			else {
// 	// 				inputKeys[i] = 99;
// 	// 			}
// 	// 		}
// 	// 		yield return null;
// 	// 	}
// 	//
// 	// 	int highest = inputTimes[0];
// 	// 	int winner = 0;
// 	// 	for (int i = 1; i < inputTimes.Length; i++) {
// 	// 		if (inputTimes[i] > highest) {
// 	// 			highest = inputTimes[i];
// 	// 			winner = i;
// 	// 		}
// 	// 	}
// 	// 	StartCoroutine(AnnounceBonusWinner(winner));
// 	// }
// 	//
// 	// private IEnumerator FastestDraw() {
// 	// 	bool[] inThis = new bool[3];
// 	// 	for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
// 	// 		inThis[i] = true;
// 	// 	}
// 	// 	int holdSeconds = Random.Range(5, 10);
// 	// 	
// 	// 	float duration = Time.time + holdSeconds;
// 	// 	
// 	// 	while (duration > Time.time){
// 	// 		for (int i = 0; i < inputKeys.Length; i++) {
// 	// 			if (inputKeys[i] == 12) {
// 	// 				inThis[i] = false;
// 	// 				traderInput.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
// 	// 				zilch.Play();
// 	// 				Debug.Log("Too soon!");
// 	// 			}
// 	// 		}
// 	// 		yield return null;
// 	// 	}
// 	// 	
// 	// 	bool ringing = true;
// 	// 	ring.Play();
// 	// 	Debug.Log("Riiiiiiiing!");
// 	// 	
// 	// 	while (ringing){
// 	// 		for (int i = 0; i < inputKeys.Length; i++) {
// 	// 			if (inputKeys[i] == 12 && inThis[i]) {
// 	// 				Debug.Log("Answered!");
// 	// 				traderInput.transform.GetChild(i).transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
// 	// 				ringing = false;
// 	// 				ring.Stop();
// 	// 				Debug.Log(i);
// 	// 				StartCoroutine(AnnounceBonusWinner(i));
// 	// 			}
// 	// 		}
// 	// 		yield return null;
// 	// 	}
// 	// }
// 	//
// 	// private IEnumerator Buy2Buy()
// 	// {
// 	// 	// Set the correct input "key" to yell
// 	// 	correctKey = 14;
// 	// 	
// 	// 	// How long are we playing this game for?
// 	// 	int holdSeconds = 5;
// 	// 	
// 	// 	float duration = Time.time + holdSeconds;
// 	// 	
// 	//
// 	// 	while (duration > Time.time) {
// 	// 		for (int i = 0; i < inputKeys.Length; i++) {
// 	// 			if (inputKeys[i] == correctKey) {
// 	// 				blip.pitch = 1 + (i / 5);
// 	// 				blip.Play();
// 	// 				inputTimes[i] += 1;
// 	// 				inputDisplay[i].text = inputTimes[i].ToString();
// 	// 				inputKeys[i] = 99;
// 	// 			}
// 	// 		}
// 	// 		yield return null;
// 	// 	}
// 	// 	
// 	// 	int highest = inputTimes[0];
// 	// 	int winner  = 0;
// 	// 	for (int i = 1; i < inputTimes.Length; i++) {
// 	// 		if (inputTimes[i] > highest) {
// 	// 			highest = inputTimes[i];
// 	// 			winner = i;
// 	// 		}
// 	// 	}
// 	// 	StartCoroutine(AnnounceBonusWinner(winner));
// 	// }
// 	//
// 	// // private IEnumerator ScoreCalculation() {
// 	// // 	scoreCalculators[scoresCalculated].transform.localPosition = new Vector3(-100, 200, 0);
// 	// // 	StartCoroutine(scoreCalculators[scoresCalculated].CalculateScore());
// 	// // 	yield return null;
// 	// //
// 	// // }
// 	//
// 	// // public IEnumerator PostScore(float score) {
// 	// // 	traderWorthDisplay[scoresCalculated].transform.localPosition = new Vector3(-300 + (300 * scoresCalculated), 50, 0);
// 	// // 	Text t = traderWorthDisplay[scoresCalculated].GetComponentInChildren<Text>();
// 	// // 	t.text = "$" + score.ToString("F2");
// 	// // 	scoreCalculators[scoresCalculated].transform.localPosition = new Vector3(-100, 1000, 0);
// 	// //
// 	// // 	yield return new WaitForSeconds(0.5f);
// 	// //
// 	// // 	scoresCalculated++;
// 	// // 	if (scoresCalculated == GlobalVariables.S.numTraders) {
// 	// // 		//Save to the web if we are doing that
// 	// // 		//FundManager.S.SaveWorth();
// 	// // 		if (GameManager.S.gameRound == GlobalVariables.S.numRounds - 1) {
// 	// // 			SceneManager.LoadScene("End");
// 	// // 		} else {
// 	// // 			StartCoroutine(PrepMinigame());
// 	// // 		}
// 	// // 	} else {
// 	// // 		StartCoroutine(ScoreCalculation());
// 	// // 	}
// 	// //
// 	// // 	yield return null;
// 	// //
// 	// // }
// 	//
// 	// 
// 	//
// 	// 
// 	//
// 	// private IEnumerator BackToGame() {		
// 	// 	// Start Countdown
// 	// 	loading.SetActive(true);
// 	// 	blip.pitch = 1;
// 	// 	int countdownLength = 5;
// 	// 	while (countdownLength > 0) {
// 	// 		loading.transform.GetChild(1).GetComponent<Text>().text = countdownLength.ToString();
// 	// 		countdownLength -= 1;
// 	// 		blip.Play();
// 	// 		yield return new WaitForSeconds(1);
// 	// 	}
// 	//
// 	// 	StartCoroutine(LoadNextRound());
// 	// }
// 	//
// 	// Get Trader key inputs for MiniGames
// 	private void GetKeys() {
// 		for (int i = 0; i < 3; i++) {
// 			for(int j=0; j < 15; j++){
// 				if (Input.GetKeyDown(GlobalVariables.S.inputKeys[i, j])) {
// 					inputKeys[i] = j;
// 				}
// 			}
// 		}
// 	}
// 	//
// 	// private IEnumerator LoadNextRound() {
// 	// 	GameManager.S.NextRound();
// 	// 	yield return null;
// 	//
// 	// }
//
// }
