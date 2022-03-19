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
	private int					 currentItemNumber; 
    
    // Start is called before the first frame update
    void Start() {
        width = GetComponent<RectTransform>().rect.width;
        pixelsPerSecond = width / contentDuration;
		currentItemNumber = Random.Range(0, tickerItems.Length);
        AddTickerItem(tickerItems[currentItemNumber]);
    }

    // Update is called once per frame
    void Update(){
        if (currentItem.GetXPosition <= -currentItem.GetWidth) {
			currentItemNumber = (currentItemNumber + 1) % tickerItems.Length;
            AddTickerItem(tickerItems[currentItemNumber]);
        }
    }

    void AddTickerItem(string message) {
        currentItem = Instantiate(tickerContent_prefab, transform);
        currentItem.Initialize(width, pixelsPerSecond, message);
    }
}
