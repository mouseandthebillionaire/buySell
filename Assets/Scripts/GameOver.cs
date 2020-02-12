using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public Text         winner, snarkText;
    private TextAsset   snarkCollection;
    private string[]    snark;

    private float       timePassed;
    public float        delayTime;

    private string[] teamName = new string[] {"Necktie, Necktie and Fleece", "Young Upstarts", "Rose and Rosen Rose"};
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(GetSnarkFromFile());

        //Debug
        winner.text = teamName[0];
        
        // Actual
//        winner.text = teamName[GlobalVariables.S.winner];
        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Period)) {
            Menu();
        }

        timePassed += Time.deltaTime;

        if (timePassed >= delayTime) {
            Menu();
        }

    }

    private void Menu() {
        GameManager.S.Reset();
        SceneManager.LoadScene("Menu");
    }
    
    IEnumerator GetSnarkFromFile() {
        snarkCollection = Resources.Load("endSnark") as TextAsset;
        snark = snarkCollection.text.Split ('#');
        
        int i = UnityEngine.Random.Range(0, snark.Length - 1);
        snarkText.text = snark[i];

        yield return null;
    }
}
