using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.IO;
using UnityEngine.Networking;
using System.Text;

public class FundManager : MonoBehaviour {
    private string         url = "http://mouseandthebillionaire.com/buySell/www/";
    private string         fileName = "traderFunds.xml";

    public static FundManager S;
    
    

    // Start is called before the first frame update
    void Awake() {
        S = this;

    }

    public void LoadWorth() {
        StartCoroutine(GetWorthFromWeb());
    }

    public void SaveWorth() {
        // If we are saving the worth to web
        StartCoroutine(UploadWorthToWeb());
    }

    public IEnumerator GetWorthFromWeb() {
        UnityWebRequest.ClearCookieCache();
        UnityWebRequest www = UnityWebRequest.Get(url + fileName);
        yield return www.SendWebRequest();
        
        Debug.Log("uploading");
        
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
    

}