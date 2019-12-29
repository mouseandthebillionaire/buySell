using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class NewsFeed : MonoBehaviour {

    private string		    newsFeedMaster;
    private TextAsset       newsFeedMasterText; 
    private string[]        feedItems;
    private List<String>    feedItemsDisplay = new List<string>();
    private string[]         newItemColl;
    private Text            newsFeedText;

    public float             newsUpdateTime = 2.5f;
    public float             itemsToDisplay = 5;
    
    private string url = "http://www.mouseandthebillionaire.com/games/gameFiles/buySell/newsFeed.txt";
    

    
    
    // Start is called before the first frame update
    void Start(){ 
        newsFeedText = GetComponentInChildren<Text>();

        //StartCoroutine(GetTextFromWeb());
        StartCoroutine(GetTextFromFile());
        
    }
 
    IEnumerator GetTextFromWeb() {
        UnityWebRequest.ClearCookieCache();
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(url);
            Debug.Log(www.error);
        }
        else {
            // Import into the New Feed
            newsFeedMaster = www.downloadHandler.text;
            feedItems = newsFeedMaster.Split('#');
            StartCoroutine("UpdateNews");
        }
    }

    IEnumerator GetTextFromFile() {
        newsFeedMasterText = Resources.Load("newsFeed") as TextAsset;
        feedItems = newsFeedMasterText.text.Split ('#');
        
        //StartCoroutine("UpdateNews");
        StartCoroutine("InitialNews");

        yield return null;
    }

    private IEnumerator InitialNews() {
        if (feedItems != null) {
            for (int i = 0; i < itemsToDisplay; i++) {
                int newItem = UnityEngine.Random.Range(0, feedItems.Length - 1);
                newItemColl = feedItems[newItem].Split('>');
                string newItemText = newItemColl[1];
            
                // Remove newlines
                newItemText = newItemText.Replace("\n", String.Empty);
                newItemText += ("        \t" + System.DateTime.Now.ToString("HH:m:ss"));
                feedItemsDisplay.Add(newItemText);
                newsFeedText.text = "";
            
                foreach(var f in feedItemsDisplay)
                {
                    newsFeedText.text += f.ToString() + "\n";
                }
            }
        }
        yield return new WaitForSeconds(newsUpdateTime);
        StartCoroutine("UpdateNews");
    }

    private IEnumerator UpdateNews() {
        if (feedItems != null) {
            int newItem = UnityEngine.Random.Range(0, feedItems.Length - 1);
            newItemColl = feedItems[newItem].Split('>');
            string newItemText = newItemColl[1];
            
            // Remove newlines
            newItemText = newItemText.Replace("\n", String.Empty);
            newItemText += ("        \t" + System.DateTime.Now.ToString("HH:m:ss"));
            
            StartCoroutine("EffectStock");            
            
            feedItemsDisplay.Add(newItemText);
            if (feedItemsDisplay.Count > 5) {
                feedItemsDisplay.RemoveAt(0);
            }
            newsFeedText.text = "";
            
            foreach(var f in feedItemsDisplay)
            {
                newsFeedText.text += f.ToString() + "\n";
            }
        }

        yield return new WaitForSeconds(newsUpdateTime);
        StartCoroutine("UpdateNews");
    }

    public IEnumerator EffectStock() {
        // Get the stock affected
        int stockNum = int.Parse(newItemColl[0]) - 1;      
        // And how is it affected?
        string stockDir = newItemColl[2];
        // Wait a few seconds to send to the Game Manager
        yield return new WaitForSeconds(3f);
        // Make the change!
        GameManager.S.EffectStock(stockDir, stockNum);
    }

}
