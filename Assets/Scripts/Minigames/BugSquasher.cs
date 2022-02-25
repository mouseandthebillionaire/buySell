using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugSquasher : MonoBehaviour
{
	public Sprite[]		splats;
	public GameObject[]	bugKeys = new GameObject[12];
	public AudioSource splatSound;

	private int correctKey;
	private int[] inputTimes = new int[3];
	
	// Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(RunGame());
    }

	private IEnumerator MoveBug(GameObject _bug) {
		while (_bug.GetComponent<Image>().color == Color.white) {
			_bug.transform.position = new Vector3(
				_bug.transform.position.x + Random.Range(-2f, 2f),
				_bug.transform.position.y + Random.Range(-2f, 2f), 0);
			yield return new WaitForSeconds(0.1f);
		}
		
	}

    private IEnumerator RunGame() {
		// overall time based
		int minigameLength = 20;
		float duration = Time.time + minigameLength;
		
		// game variables
		float bugFrequency = 1f;
		bool bugPresent = false;
		for (int i = 0; i < bugKeys.Length; i++) {
			GameObject go = GameObject.Find(bugKeys[i].name + "/bug");
			go.GetComponent<Image>().color = Color.clear;
		}
	
		while (duration > Time.time) {
			if (!bugPresent) {
				correctKey = Random.Range(0, bugKeys.Length);
				Debug.Log(correctKey);
				GameObject go = GameObject.Find(bugKeys[correctKey].name + "/bug");
				go.GetComponent<Image>().color = Color.white;
				// Move the bug around
				StartCoroutine(MoveBug(go));
				bugPresent = true;
			}
			
			for (int i = 0; i < MinigameManager.S.inputKeys.Length; i++) {
				if (MinigameManager.S.inputKeys[i] == correctKey) {
					MinigameManager.S.UpdatePlayerScore(i);
					GameObject go = GameObject.Find(bugKeys[correctKey].name + "/bug");
					go.GetComponent<Image>().color = Color.clear;
	
					// Splat
					GameObject splat = GameObject.Find(bugKeys[correctKey].name + "/splat");
					splat.GetComponent<Image>().sprite = splats[Random.Range(0, splats.Length)];
					splat.GetComponent<Image>().color = GlobalVariables.S.traderColors[i];
					splatSound.Play();
					
					MinigameManager.S.inputKeys[i] = 99;
	
					// Only show splay for half a second
					yield return new WaitForSeconds(0.5f);
					splat.GetComponent<Image>().color = Color.clear;
					
					yield return new WaitForSeconds(bugFrequency);
					bugFrequency -= 0.05f;
					bugPresent = false;
				}
				else {
					MinigameManager.S.inputKeys[i] = 99;
				}
			}
			yield return null;
		}
	
		MinigameManager.S.EndGame();
	}
}
