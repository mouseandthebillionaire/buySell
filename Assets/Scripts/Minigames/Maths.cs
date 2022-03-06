using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maths : MonoBehaviour
{
    private string[]	equations = new string[10] {
		"(4 - (2 x 2)) x (12 x 6 / 3)",
		"((1 + 1) x 100) / (50 x 4)",
		"((40 / 8) x 2) / 5",
		"(1 + 2 + 3) / 2",
		"4 + (12/2) - 6",
		"2 + 1 + 1 + 1",
		"1 + 2 + 2 + 1",
		"1 + 1 + 1 + 1 + 1 + 2",
		"(1 + 1) x 4",
		"2 + 2 + 2 + 2 + 1"
	};
    
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(RunGame());
    }

    private IEnumerator RunGame() {
	    int correctKey  = Random.Range(0, 9);
	    Debug.Log(correctKey);
	    
		// Unlike the button masher, no timer is used here. Just who answers first.
		bool unanswered = true;
		int numAnswered = 0;
		
		// Don't let them answer if they have entered the wrong answer
		bool[] answered = new bool[GlobalVariables.S.numTraders];
		for (int i = 0; i < answered.Length; i++) {
			answered[i] = false;
			MinigameManager.S.inputKeys[i] = 99;
		}
		
		GameObject go_0 = GameObject.Find("Directions");
		GameObject go_1 = GameObject.Find("Equation");
		Text directions = go_0.GetComponent<Text>();
		Text equation = go_1.GetComponent<Text>();
		directions.text = "EQUATION SENT: SOLVE THE FOLLOWING. IT MAY UNCOVER AN INVESTMENT OPPORTUNITY \n\n";
		equation.text = equations[correctKey];
		
		while(unanswered) {
			for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
				if (!answered[i] && MinigameManager.S.inputKeys[i] == correctKey) {
					unanswered = false;
					BetweenerManager.S.AnnounceBonusWinner(i);
				} else {
					if (!answered[i] && 
					    MinigameManager.S.inputKeys[i] != 99 && 
					    MinigameManager.S.inputKeys[i] != 11 &&
					    MinigameManager.S.inputKeys[i] != 12 &&
					    MinigameManager.S.inputKeys[i] != 13) {
						// They answered incorrectly
						answered[i] = true;
						MinigameManager.S.traderInput.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
						BetweenerManager.S.zilch.Play();
						numAnswered++;
						// they all go it wrong
						if (numAnswered == GlobalVariables.S.numTraders) {
							// Trader 3 means no one won
							BetweenerManager.S.AnnounceBonusWinner(99);
						}
					}
				}
			}
			
			yield return null;
		}
	}
}
