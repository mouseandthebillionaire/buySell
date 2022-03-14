using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Text;

public class FundManager : MonoBehaviour {
    public bool            fundsLoaded;
    
    private string         url = "http://mouseandthebillionaire.com/buySell/www/";
    private string         fileName = "traderFunds.xml";

    public static FundManager S;
    
    

    // Start is called before the first frame update
    void Awake() {
        if (S == null) {
            S = this;
        }
        else {
            DestroyObject(gameObject);
        }

    }

    public void LoadWorth() {
        fundsLoaded = false;
        // From the Web
        //StartCoroutine(GetWorthFromWeb());
        // From a file
        StartCoroutine(GetWorthFromFile());
    }

    public void SaveWorth() {
        // If we are saving the worth to the web
        //StartCoroutine(UploadWorthToWeb());
        // Or from a file
        StartCoroutine(SaveWorthToFile());
    }

    public IEnumerator GetWorthFromWeb() {
        UnityWebRequest.ClearCookieCache();
        UnityWebRequest www = UnityWebRequest.Get(url + fileName);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(url);
            Debug.Log(www.error);
        }
                
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(www.downloadHandler.text);

        var baseNode = xmlDoc.DocumentElement;
                
        for (int i = 0; i < GlobalVariables.S.numTraders; i++)
        {
            var childNode = baseNode.ChildNodes[i];
            Debug.Log(float.Parse(childNode.SelectSingleNode("score").InnerText));
            GlobalVariables.S.traderWorth[i] = float.Parse(childNode.SelectSingleNode("score").InnerText);
        }

        UnityWebRequest.Delete(url + fileName);
    }

    private IEnumerator GetWorthFromFile() {
        // Refresh the file
        AssetDatabase.Refresh();
        TextAsset fundsFile = Resources.Load("traderFunds") as TextAsset;
        string[] traderFunds = fundsFile.text.Split ('#');
        
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            GlobalVariables.S.traderWorth[i] = float.Parse(traderFunds[i]);
        }

        fundsLoaded = true;
        yield return null;
    }

    IEnumerator UploadWorthToWeb() {
        //making a dummy xml level file
        XmlDocument map = new XmlDocument();
        string fundContent = "<Traders>";
        
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            string tempNum = GlobalVariables.S.traderWorth[i].ToString("#.00");
            fundContent += "<Trader_" + i + "><score>" + tempNum + "</score></Trader_" + i +
                           ">";
        }

        fundContent += "</Traders>";
        map.LoadXml(fundContent);

        //converting the xml to bytes to be ready for upload
        byte[] levelData = Encoding.UTF8.GetBytes(map.OuterXml);

        WWWForm form = new WWWForm();

        form.AddField("action", "level upload");
        form.AddField("file", "file");
        form.AddBinaryData ( "file", levelData, fileName, "text/xml");

        //change the url to the url of the php file
        WWW w = new WWW(url + "traderFunds.php", form);

        yield return w;
        if (w.error != null) {
            print ( w.error );
        }
        else { 
            //this part validates the upload, by waiting 5 seconds then trying to retrieve it from the web
            if (w.uploadProgress == 1 || w.isDone)
            {
                yield return new WaitForSeconds(5);
                //change the url to the url of the folder you want it the levels to be stored, the one you specified in the php file
                WWW w2 = new WWW(url + fileName);
                yield return w2;
                if (w2.error != null) {
                    print("error 2");
                    print ( w2.error );
                }
            }
        }

    }

    private IEnumerator SaveWorthToFile() {
        string file = "Assets/Resources/traderFunds.txt";
        string text = "";
        
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            string tempNum = GlobalVariables.S.traderWorth[i].ToString("#.00");
            text += tempNum.ToString() + "#";
        }
        File.WriteAllText(file, text);
        // Add a quick delay to make sure this happens
        yield return new WaitForSeconds(0.5f);
    }

    public void CompleteResetOfAllWorth() {
        string file = "Assets/Resources/traderFunds.txt";
        string text = "";
        
        for (int i = 0; i < GlobalVariables.S.numTraders; i++) {
            GlobalVariables.S.traderWorth[i] = 0;
            string tempNum = "0";
            text += tempNum.ToString() + "#";
        }
        File.WriteAllText(file, text);
        Debug.Log("Funds Reset");
    }
    

}