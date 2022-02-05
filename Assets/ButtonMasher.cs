using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMasher : MonoBehaviour {
	public Text number;
	private int[] inputTimes = new int[3];
	
	// Start is called before the first frame update
    void Start() {
		StartCoroutine(StartMiniGame());
	}

	private IEnumerator StartMiniGame() {
		// How long are we playing this game for?
		int holdSeconds = 5;
		//int correctKey  = Random.Range(0, 9);
		int correctKey = 1;

		float duration = Time.time + holdSeconds;

		number.text = correctKey.ToString();


		while (duration > Time.time) {
			for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
				if (MinigameManager.S.inputKeys[i] == correctKey) {
					BetweenerManager.S.blip.pitch = 1 + (i / 5);
					BetweenerManager.S.blip.Play();
					inputTimes[i] += 1;
					MinigameManager.S.inputDisplay[i].text = inputTimes[i].ToString();
					MinigameManager.S.inputKeys[i] = 99;
				}
			}

			yield return null;
		}

		int highest = inputTimes[0];
		int winner  = 0;
		for (int i = 1; i < inputTimes.Length; i++) {
			if (inputTimes[i] > highest) {
				highest = inputTimes[i];
				winner = i;
			}
		}

		BetweenerManager.S.AnnounceBonusWinner(winner);
	}
}
