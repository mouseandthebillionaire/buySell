using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GlobalData : MonoBehaviour {
    
    private string[] companyNames = new string[] {
        "APLS", "CHNA", "DMND", "TKOS", "FRTS", "LEGO", "CARS", "BRBI", "BLOK", "GOLD", "GLUE", "PPR", "DORA", "PZA", "TVS", "BIKE", "JUIC", "BEES", "JPN", "USA", "POOL", "CATS", "DOGS", "DIRT"
    };

    private int companyNum;

    public Text globalDataText;
    
    private List<string>    dataList = new List<string>();
    
    
    // Start is called before the first frame update
    void Start() {
        companyNum = Random.Range(0, companyNames.Length);
        for (int i = 0; i < 5; i++) {
            AddGlobalData();
        }
        
        foreach(var f in dataList){
            globalDataText.text += f + "\n";
        }

        StartCoroutine("UpdateData");


    }

    public IEnumerator UpdateData() {
        yield return new WaitForSeconds(1);
        AddGlobalData();
        dataList.RemoveAt(0);
        globalDataText.text = "";
        foreach(var f in dataList){
            globalDataText.text += f + "\n";
        }

        StartCoroutine("UpdateData");
    }
    
    

    public void AddGlobalData() {
        float value = Random.Range(2f, 2000f);
        float growth = Random.Range(0.25f, 15f);
        float change = Random.Range(0.01f, 3.99f);
        bool dirChance = (Random.value > 0.5f);
        string dir;
        if (dirChance) dir = "+";
        else dir = "-";
        string globalData = companyNames[companyNum] + "        "
                                                     + value.ToString("F2") + "        "
                                                     + growth.ToString("F2") + "        "
                                                     + dir + change.ToString("F2") + "%";
        dataList.Add(globalData);
        companyNum = (companyNum + 1) % companyNames.Length;
    }
}
