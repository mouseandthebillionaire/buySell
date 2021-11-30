using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class TickerContent : MonoBehaviour {
    float tickerWidth;
    float pixelsPerSecond;
    RectTransform rt;

    public float GetXPosition { get { return rt.anchoredPosition.x; } }
    public float GetWidth { get { return rt.rect.width; } }

    public void Initialize(float tickerWidth, float pixelsPerSecond, string message) {
        this.tickerWidth = tickerWidth;
        this.pixelsPerSecond = pixelsPerSecond;
        rt = GetComponent<RectTransform>();
        GetComponent<Text>().text = "    |     " + message;
    }


    void Update(){
        rt.position += Vector3.left * pixelsPerSecond * Time.deltaTime;
        if (GetXPosition <= 0 - tickerWidth - GetWidth) {
            Destroy(gameObject);
        }
    }
}
