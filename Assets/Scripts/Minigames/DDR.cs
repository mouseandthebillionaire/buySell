using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDR : MonoBehaviour {
    private KeyCode[] correctKeys = new KeyCode[3];
    public GameObject[] nextKeys = new GameObject[3];
    public Sprite[] keyImages = new Sprite[14];
    public GameObject goalBoxes;

    private bool gameRunning;

    public AudioSource conga;

    public GameObject key, DDR_keys;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplainGame());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator ExplainGame()
    {
        GameObject directions  = GameObject.Find("Directions");
        GameObject bg  = GameObject.Find("Background");
        directions.SetActive(true);
        bg.SetActive(true);
        // How much time do they need to read the instructions
        yield return new WaitForSeconds(3);
        directions.SetActive(false);
        bg.SetActive(false);
        BetweenerManager.S.announcerSprite.SetActive(false);

        StartCoroutine(RunGame());

        yield return null;
    }

    private IEnumerator RunGame()
    {
        gameRunning = true;
        conga.Play();
        goalBoxes.SetActive(true);

        StartCoroutine(CreateKeys());
        
        // Run only for as long as the music
        yield return new WaitForSeconds(conga.clip.length);
        
        // Play a symbolcrash
        
        // Kill All the DDR Keys
        foreach (Transform key in DDR_keys.transform)
        {
            Destroy(key.gameObject);
        }
        // And the target boxes
        goalBoxes.SetActive(false);
        
        // Then stop the game
        gameRunning = false;
        //  and End the game via the MinigameManager
        MinigameManager.S.EndGame();
    }

    private IEnumerator CreateKeys()
    {
        if (gameRunning)
        {
            for (int i = 0; i < GlobalVariables.S.numTraders; i++)
            {
                int r = Random.Range(0, 100);
                if (r > 20) {
                    GetNextKey(i);
                }
            }

            yield return new WaitForSeconds(1);
            StartCoroutine(CreateKeys());
        }
    }

    private void GetNextKey(int _playerNum)
    {
        int    r    = Random.Range(0, 14);
        // Assign it the right name
        string name = GlobalVariables.S.keyNames[r];
        int xLoc = 420 + (_playerNum * 400);
        GameObject go = GameObject.Instantiate(key, new Vector2(xLoc, 1000), Quaternion.identity) as GameObject;
        go.GetComponent<Image>().sprite = keyImages[r];
        go.GetComponent<DDR_key>().myKey = r;
        go.GetComponent<DDR_key>().playerNum = _playerNum;
        go.transform.parent = DDR_keys.transform;
    }
}
