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
            Debug.Log("Load it!");
            SceneManager.LoadScene("Main");
        }
    }

    public void AddTrader() {
        login.Play();
        login.pitch += 0.25f;
        if (traderCount < 2) {
            traderCount += 1;
        }
        else {
            StartCoroutine("CountDown");
        }
       
    }
    
    public void Reset() {
        countdownLength = 3;
        loading.SetActive(false);
        
    }
}
