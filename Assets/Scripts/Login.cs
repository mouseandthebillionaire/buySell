using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
    public GameObject      loading;
    public Text            countdownText;
    public AudioSource     blip, login;

    private int           traderCount;
    private int           countdownLength;

    public static Login S;
    
    
    // Start is called before the first frame update
    void Start() {
        Reset();
        S = this;
    }

    public void Update() {
        // Only on the load screen, we have an option to completely reset all Trader Worth
        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftControl)) {
            FundManager.S.CompleteResetOfAllWorth();
            for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
                GameObject.Find(i.ToString()).GetComponent<TraderLogin>().DisplayWorth();
            }
        }
    }

    public IEnumerator CountDown() {
        yield return new WaitForSeconds(1);
        loading.SetActive(true);
        countdownText.text = countdownLength.ToString();
        countdownLength -= 1;
        blip.Play();
        if (countdownLength > 0) {
            StartCoroutine("CountDown");
        }
        else {
            yield return new WaitForSeconds(1);
            GameManager.S.StartGame();
        }
    }

    public void AddTrader() {
        login.Play();
        login.pitch += 0.25f;
        if (traderCount < 2) {
            traderCount += 1;
        }
        else {
            // Build in 
            StartCoroutine("CountDown");
        }
       
    }
    
    public void Reset() {
        countdownLength = 3;
        loading.SetActive(false);
        
    }
}
