using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GlobalFeed : MonoBehaviour {

    private string		    newsFeedMaster;
    private TextAsset       newsFeedMasterText; 
    private string[]        feedItems;
    private List<String>    feedItemsDisplay = new List<string>();
    private string[]         newItemColl;
    private Text            newsFeedText;

    private float            newsUpdateTime = 1.5f;
    private int              itemsToDisplay = 5;
    
    

    
    
    // Start is called before the first frame update
    void Start(){ 
        newsFeedText = GetComponentInChildren<Text>();

        //StartCoroutine(GetTextFromWeb());
        StartCoroutine(GetTextFromFile());
        
    }

    IEnumerator GetTextFromFile() {
        newsFeedMasterText = Resources.Load("globalFeed") as TextAsset;
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
                
                string newItemText = newItemColl[0];
                // Add sign
                float r = UnityEngine.Random.Range(0, 100);
                string sign = "";
                if (r < 50) sign = " -";
                else sign = " +";
                newItemText += sign;
                float ammount = UnityEngine.Random.Range(0.1f, 19.0f);
                newItemText += ammount.ToString("f2");
                
                // Remove newlines
                newItemText = newItemText.Replace("\n", String.Empty);
                
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
            
            string newItemText = newItemColl[0];
            // Add sign
            float  r    = UnityEngine.Random.Range(0, 100);
            string sign = "";
            if (r < 50) sign = " -";
            else sign = " +";
            newItemText += sign;
            float ammount = UnityEngine.Random.Range(0.1f, 19.0f);
            newItemText += ammount.ToString("f2");
            // Remove newlines
            newItemText = newItemText.Replace("\n", String.Empty);
                        
            feedItemsDisplay.Add(newItemText);
            if (feedItemsDisplay.Count > itemsToDisplay) {
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


}

