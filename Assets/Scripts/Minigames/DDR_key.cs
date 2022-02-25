using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDR_key : MonoBehaviour
{
    public int myKey;
    public int playerNum;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(
            this.transform.position.x,
            this.transform.position.y - (Time.deltaTime * 240),
            this.transform.position.z
        );

        // Success based on Position rather than Collisions
        if (this.transform.localPosition.y < -145 && this.transform.localPosition.y > -160) {
            if (MinigameManager.S.inputKeys[playerNum] == myKey)
            {
                MinigameManager.S.UpdatePlayerScore(playerNum);
                MinigameManager.S.inputKeys[playerNum] = 99;
                // Destroy for now, but let's do an animation at some point!
                Destroy(this.gameObject);
            }
            if (MinigameManager.S.inputKeys[playerNum] != myKey && MinigameManager.S.inputKeys[playerNum] !=99)
            {
                BetweenerManager.S.zilch.Play();
                MinigameManager.S.inputKeys[playerNum] = 99;
            }
        }
        
        if (this.transform.localPosition.y < -160) Destroy(this.gameObject);
    }
}
