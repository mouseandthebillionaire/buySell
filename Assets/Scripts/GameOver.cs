using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public Text winner;

    private string[] teamName = new string[] {"Necktie, Necktie and Fleece", "Young Upstarts", "Rose and Rosen Rose"};
    
    // Start is called before the first frame update
    void Start() {
        winner.text = teamName[GlobalVariables.S.winner];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period)) {
            SceneManager.LoadScene("Menu");
        }
    }
}
