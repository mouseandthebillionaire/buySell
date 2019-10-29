//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class CommScript : MonoBehaviour
//{
//
//	public OSC[] 		osc;
//	public int 			score;
//	public int      	winner;
//	public int			winningScore;
//	public bool 		thermoState; // false=cold | true=hot
//
//	private int			playerNum;
//
//	public static CommScript S;
//
//	// Use this for initialization
//	void Start () {
//		S = this;
//		osc = GetComponentsInChildren<OSC>();
//		playerNum = PlayerManager.S.playerNum;
//		if (playerNum == 0) {
//			osc [0].SetAddressHandler ("/score1", OnReceiveScore1);
//			osc [1].SetAddressHandler ("/score2", OnReceiveScore2);
//			osc [2].SetAddressHandler ("/score3", OnReceiveScore3);
//		} else {
//			osc [playerNum + 2].SetAddressHandler ("/state", OnReceiveState);
//			osc [playerNum + 2].SetAddressHandler ("/winner", OnReceiveGameOver);
//			osc [playerNum + 2].SetAddressHandler ("/countdown", OnReceiveCountdown);
//			osc [playerNum + 2].SetAddressHandler ("/reset", OnReceiveReset);
//		}
//	}
//
//	public void SendState(int state){
//		OscMessage message = new OscMessage ();
//		GameManagementScript.S.gameState = state;
//		PlayerManager.S.gameState = state;
//		message.address = "/state";
//		message.values.Add (state);
//		for (int i = 0; i < 10; i++) {
//			osc [0].Send (message);
//			osc [1].Send (message);
//			osc [2].Send (message);
//		}
//	}
//
//	public void SendGameReset(){
//        OscMessage message = new OscMessage ();
//        GameManagementScript.S.gameState = 0;
//		PlayerManager.S.Reset();
//        message.address = "/reset";
//        for (int i = 0; i < 10; i++) {
//            osc [0].Send (message);
//            osc [1].Send (message);
//            osc [2].Send (message);
//        }
//    }
//
//	public void StartCountdown(){
//		OscMessage message = new OscMessage ();
//		message.address = "/countdown";
//		message.values.Add (1);
//		for (int i = 0; i < 10; i++) {
//			osc [0].Send (message);
//			osc [1].Send (message);
//			osc [2].Send (message);
//		}
//		PlayerManager.S.countingDown = true;
//	}
//
//	public void SendGameOver(int whoWon){
//		winner = whoWon;
//		OscMessage message = new OscMessage ();
//		message.address = "/winner";
//		message.values.Add (winner);
//		for (int i = 0; i < 10; i++) {
//			osc [0].Send (message);
//			osc [1].Send (message);
//			osc [2].Send (message);
//		}
//		PlayerManager.S.gameState = 4;
//		PlayerManager.S.GameOver();
//	}
//
//	public void UpdateScore(int score){
//		OscMessage message = new OscMessage ();
//		message.address = "/score" + playerNum;
//		message.values.Add (score);
//		osc [playerNum + 2].Send (message);
//	}
//
//	void OnReceiveScore1(OscMessage message){
//		GameManagementScript.S.score [1]++;
//		Debug.Log ("Employee #1: " + GameManagementScript.S.score [1]);
//	}
//
//	void OnReceiveScore2(OscMessage message){
//		GameManagementScript.S.score [2]++;
//		Debug.Log ("Employee #2: " + GameManagementScript.S.score [2]);
//	}
//
//	void OnReceiveScore3(OscMessage message){
//		GameManagementScript.S.score [3]++;
//		Debug.Log ("Employee #3: " + GameManagementScript.S.score [3]);
//	}
//
//	void OnReceiveState(OscMessage message){
//		int state = message.GetInt (0);
//		PlayerManager.S.gameState = state;
//	}
//
//	void OnReceiveReset(OscMessage message){
//		int state = -1;
//		PlayerManager.S.gameState = 0;
//		PlayerManager.S.Reset();
//    }
//
//	void OnReceiveCountdown(OscMessage message){
//		if (message.GetInt (0) == 1) {
//			PlayerManager.S.countingDown = true;
//		}
//	}
//
//	void OnReceiveWinnerScore(OscMessage message){
//		winningScore = message.GetInt (0);
//	}
//
//	void OnReceiveGameOver(OscMessage message){
//		winner = message.GetInt (0);
//		SceneScript.S.GameOver ();
//	}
//
//
//}
//
