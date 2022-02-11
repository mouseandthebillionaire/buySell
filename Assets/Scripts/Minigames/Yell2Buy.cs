using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yell2Buy : MonoBehaviour
{
	private int[] inputTimes = new int[3];
	
	// Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(RunGame());
    }

    private IEnumerator RunGame() {
	    // How long are we playing this game for?
	    int holdSeconds = 5;
	    // Correct Key is "Yell"
	    int correctKey = 14;

	    float duration = Time.time + holdSeconds;

	    while (duration > Time.time) {
		    for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
			    if (MinigameManager.S.inputKeys[i] == correctKey) {
					MinigameManager.S.UpdatePlayerScore(i);
					MinigameManager.S.inputKeys[i] = 99;
			    }
		    }

		    yield return null;
	    }

		MinigameManager.S.EndGame();
    }
}