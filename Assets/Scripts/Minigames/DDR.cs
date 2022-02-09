using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDR : MonoBehaviour {
    private KeyCode[] correctKeys = new KeyCode[3];
    public GameObject[] nextKeys = new GameObject[3];
    
    // Start is called before the first frame update
    void Start() {
        GetNextKey();
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < nextKeys.Length; i++) {
            nextKeys[i].transform.position = new Vector3(
                nextKeys[i].transform.position.x,
                nextKeys[i].transform.position.y - (Time.deltaTime * 120),
                nextKeys[i].transform.position.z
                );
        }
    }

    private void GetNextKey() {
        for (int i = 0; i < 3; i++) {
            // Pick a Random Key For this Player
            int    r    = Random.Range(0, 14);
            // Assign it the right name
            string name = GlobalVariables.S.keyNames[r];
            nextKeys[i].GetComponent<Text>().text = r.ToString();
            correctKeys[i] = GlobalVariables.S.inputKeys[i, r];
            // Assign it to the keys
            nextKeys[i].GetComponent<Text>().text = name;
        }
    }
}
