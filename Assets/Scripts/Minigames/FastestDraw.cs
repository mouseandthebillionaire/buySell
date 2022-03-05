using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastestDraw : MonoBehaviour
{
	public AudioSource ring;
	
	// Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(RunGame());
    }

    private IEnumerator RunGame() {
		bool[] inThis = new bool[3];
		for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
			inThis[i] = true;
		}
		int holdSeconds = Random.Range(5, 10);
		int timeToAnswer = 5;
		
		float duration = Time.time + holdSeconds;
		
		while (duration > Time.time){
			for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
				if (MinigameManager.S.inputKeys[i] == 12 && inThis[i]) {
					inThis[i] = false;
					MinigameManager.S.traderInput.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
					BetweenerManager.S.zilch.Play();
					Debug.Log("Too soon!");
				}
			}
			yield return null;
		}
		
		bool ringing = true;
		ring.Play();
		Debug.Log("Riiiiiiiing!");
		
		while (ringing){
			for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
				if (MinigameManager.S.inputKeys[i] == 12 && inThis[i]) {
					MinigameManager.S.traderInput.transform.GetChild(i).transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
					ringing = false;
					ring.Stop();
					BetweenerManager.S.AnnounceBonusWinner(i);
				}
			}

			if (Time.time > (duration + timeToAnswer))
			{
				ringing = false;
				ring.Stop();
				BetweenerManager.S.AnnounceBonusWinner(99);
			}
			
			yield return null;
		}
	}
}
