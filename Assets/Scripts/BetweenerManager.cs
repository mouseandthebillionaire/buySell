using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetweenerManager : MonoBehaviour
{
    public bool skipScoring;
    
    public GameObject	scoreManager;
    public GameObject   minigameManager;
    public GameObject	bonusAnnouncement, winner, loading;
    private int         currSection;

    // Announcer
    public GameObject announcerSprite;
    private SpriteRenderer announcer_sr;
    public AudioSource  announcer;
    public AudioClip    intro;
    public AudioClip    bonus;
    
    // Sound Effects
    public AudioSource  bonusOpportunity, bonusWon, blip, zilch, coin;

    public static BetweenerManager	S;

    void Awake() {
        S = this;
    }
    
    // Start is called before the first frame update
    void Start() {
        announcer_sr = announcerSprite.GetComponent<SpriteRenderer>();
        
        if (skipScoring) currSection = 2;
        else currSection = 0;
        StartCoroutine(Control(currSection));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Control(int _section) {
        
        if (_section == 0) {
            // Great work, let's see how we did
            announcer.clip = intro;
            announcer.Play();
            yield return new WaitForSeconds(intro.length);
            announcer_sr.enabled = true;
            // Launch Score Manager
            scoreManager.SetActive(true);
        }

        if (_section == 1)
        {
            // Incoming Bonus Opportunity coming
            announcer.clip = bonus;
            announcer.Play();
            // yield return new WaitForSeconds(bonus.length);

            int blinkTimes = 4;
            while (blinkTimes >= 0)
            {
                bonusAnnouncement.SetActive(true);
                bonusOpportunity.Play();
                yield return new WaitForSeconds(0.75f);
                bonusAnnouncement.SetActive(false);
                yield return new WaitForSeconds(0.25f);
                blinkTimes--;
            }

            _section++;
            StartCoroutine(Control(2));
        }

        if (_section == 2) {
            // Launch the Minigame
            minigameManager.SetActive(true);
        }

        yield return null;
    }
    
    public void AnnounceBonusWinner(int traderNum) {
        string winnerText;
        if (traderNum == 99) {
            winner.GetComponent<Image>().color = Color.gray;
            winnerText = "NOBODY WON THE BONUS TODAY!";
        }
        else {
            winner.GetComponent<Image>().color = GlobalVariables.S.traderColors[traderNum];
            winnerText = GlobalVariables.S.shortTeamNames[traderNum] + " HAS WON THE BONUS!";
        }
        
        winner.transform.GetChild(0).GetComponent<Text>().text = winnerText;
        winner.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
        float panelSize = 1.5f;
        winner.transform.localScale = new Vector3(panelSize, panelSize);
        bonusWon.Play();
        winner.SetActive(true);
        while (panelSize > 1f) { 
            panelSize -= .05f; 
            winner.transform.localScale = new Vector3(panelSize, panelSize);
        }

        StartCoroutine(BackToGame());
    }

    public void Next() {
        currSection++;
        StartCoroutine(Control(currSection));
    }

    private IEnumerator BackToGame()
    {
        // Start Countdown
        loading.SetActive(true);
        blip.pitch = 1;
        int countdownLength = 5;
        while (countdownLength > 0)
        {
            loading.transform.GetChild(1).GetComponent<Text>().text = countdownLength.ToString();
            countdownLength -= 1;
            blip.Play();
            yield return new WaitForSeconds(1);
        }

        GameManager.S.NextRound();
    }
}
