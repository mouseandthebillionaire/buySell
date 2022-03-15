using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Walkthrough : MonoBehaviour
{
    public GameObject[] dimmers = new GameObject[6];
    public GameObject[] labels = new GameObject[6];

    public VideoPlayer v;

    private bool waiting;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunWalkthrough());
    }

    void Update()
    {
        if (waiting)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.L))
            {
                waiting = false;
                SceneManager.LoadScene("Menu");
            }
        }

    }

    private IEnumerator RunWalkthrough()
    {
        waiting = false;
        
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < dimmers.Length; i++)
        {
            dimmers[i].SetActive(true);
        }
        // Show "How To" Text
        labels[0].SetActive(true);
        // Pause the Video 
        v.Pause();
        yield return new WaitForSeconds(3);
        labels[0].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show the "Check Your Holdings" Text
        dimmers[0].SetActive(false);
        labels[1].SetActive(true);
        yield return new WaitForSeconds(2);
        // Then Hide Them
        dimmers[0].SetActive(true);
        labels[1].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        v.Play();
        
        // Show the "Dial Your Broker" Text
        dimmers[3].SetActive(false);
        labels[3].SetActive(true);
        yield return new WaitForSeconds(2);
        // Then Hide Them
        dimmers[3].SetActive(true);
        labels[3].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show the Trader Dial Again to Highlight the Transaction
        dimmers[0].SetActive(false);
        labels[5].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        // Then Hide Them
        dimmers[0].SetActive(true);
        labels[5].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show Check the News
        dimmers[2].SetActive(false);
        labels[4].SetActive(true);
        yield return new WaitForSeconds(2f);
        // Then Hide Them
        dimmers[2].SetActive(true);
        labels[4].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show Watch the Charts
        dimmers[1].SetActive(false);
        labels[2].SetActive(true);
        yield return new WaitForSeconds(2f);
        // Then Hide Them
        dimmers[1].SetActive(true);
        labels[2].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show the "Dial Your Broker" Text Again
        dimmers[3].SetActive(false);
        labels[3].SetActive(true);
        yield return new WaitForSeconds(2);
        // Then Hide Them
        dimmers[3].SetActive(true);
        labels[3].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show the Trader Dial Again to Highlight the Sale
        dimmers[0].SetActive(false);
        labels[6].SetActive(true);
        yield return new WaitForSeconds(3);
        // Then Hide Them
        dimmers[0].SetActive(true);
        labels[6].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        // Show End Text and Menu Option
        labels[7].SetActive(true);
        yield return new WaitForSeconds(3);

        waiting = true;

    }
}
