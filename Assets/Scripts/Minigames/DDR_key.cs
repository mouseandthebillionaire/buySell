using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDR_key : MonoBehaviour
{
    public int myKey;
    public int playerNum;
    

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(
            this.transform.position.x,
            this.transform.position.y - (Time.deltaTime * 120),
            this.transform.position.z
        );
        
        if (this.transform.position.y < 0) Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (MinigameManager.S.inputKeys[playerNum] == myKey)
        {
            MinigameManager.S.UpdatePlayerScore(playerNum);
        }
        if (MinigameManager.S.inputKeys[playerNum] != myKey)
        {
            BetweenerManager.S.zilch.Play();
        }
    }
}
