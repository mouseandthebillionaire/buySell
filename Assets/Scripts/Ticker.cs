using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour {
    public TickerContent        tickerContent_prefab;
    [Range(1f, 10f)]
    public float                contentDuration = 6.0f;
    public string[]             tickerItems;

    private float                width;
    private float                pixelsPerSecond;
    private TickerContent        currentItem;
    
    // Start is called before the first frame update
    void Start() {
        width = GetComponent<RectTransform>().rect.width;
        pixelsPerSecond = width / contentDuration;
        AddTickerItem(tickerItems[0]);
    }

    // Update is called once per frame
    void Update(){
        if (currentItem.GetXPosition <= -currentItem.GetWidth) {
            AddTickerItem(tickerItems[Random.Range(0, tickerItems.Length)]);
        }
    }

    void AddTickerItem(string message) {
        currentItem = Instantiate(tickerContent_prefab, transform);
        currentItem.Initialize(width, pixelsPerSecond, message);
    }
}
