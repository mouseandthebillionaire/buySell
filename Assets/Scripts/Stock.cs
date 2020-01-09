using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IBM.Watson.DeveloperCloud.Logging;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Stock : MonoBehaviour {

	public string			stockName;
	private int				stockNumber;
	public string			stockCode; // each stock has a unique 2-digit code that needs to be dialed

	private List<float> 	valueHistory = new List<float>();
	public float			value;
	public int				direction;
	public float			volatility = 1;
	public float			stockPrice;
	public float 			targetPrice;
	public float			minPrice, maxPrice;
	public float			movementSpeed;
	public int				hotness;
	public GameObject		LinePixel;
	public Text				displayName;
	public Text				displayPrice;
	private float			displayPriceAmount;
	public Text				openingPrice;
	public Color			stockColor;

	public GameObject		lineGraphParent;
	private GameObject		lineGraph;
	
	public int				stockState; // 0 - stable, 1 - growing, 2 - falling, 3 - cycling
	
	// try new stock method
	public List <float>		stockHistory = new List<float>();
	public LineRenderer		lr;
	public SpriteRenderer	stockNumberIcon;
	public float			stockValue;
	private float			variance;
	public float			minValue, maxValue;
	
	// Use this for initialization
	void Start () {
		stockNumber = transform.GetSiblingIndex();

		//StartCoroutine(SwitchStates());
		Stable();
		displayName.color = stockColor;
		lineGraph = GameObject.Find("LineGraph");
		volatility = Random.Range(0.25f, 1.5f);
		stockPrice = Random.Range(0.5f, 4.5f);
		targetPrice = stockPrice;
//		minPrice = 0.5f;
//		maxPrice = 4.5f;
		//float opening = stockPrice * 100f;
		//openingPrice.text = opening.ToString();
		
		// try new stock method
		// randomly fill our list with stock prices
		for (int i = 0; i < 14; i++) {
			stockHistory.Add(Random.Range(-3, 4));
			lr.SetPosition(i, new Vector3(i, stockHistory[i], 0));
		}
		// make a line instead of a pixel
		InvokeRepeating("UpdateStock", 0.25f, 1f);
		// Get Number Icon

		lr.sortingLayerName = "Stocks";
		lr.sortingOrder = 20;
		stockNumberIcon = lr.GetComponentInChildren<SpriteRenderer>();
		string spriteLoc = "_stockNumberIcons/stockNumber_" + stockNumber;
		Sprite s = Resources.Load<Sprite>(spriteLoc);
		stockNumberIcon.sprite = s;

		minValue = 1;
		maxValue = 7;
		stockValue = Random.Range(minValue, maxValue);
		displayPriceAmount = stockPrice * 100f;
		openingPrice.text = displayPriceAmount.ToString();
		StartCoroutine(Fluctuate());

	}
	
	public void MakePixel(){

		GameObject go;
		go = Instantiate(LinePixel, new Vector2(400f, value - 3f), Quaternion.identity) as GameObject;
		SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
		sr.color = stockColor;
		go.transform.SetParent(lineGraphParent.transform);
		go.transform.localPosition = new Vector2(4.9f, value - 3f);

	}

	public void SetStockCode(string _code) {
		stockCode = _code;
	}

	public void UpdateStock() {
		stockHistory.RemoveAt(0);
		if((stockValue + variance) > maxValue || (stockValue + variance) < minValue) Debug.Log("Too much");
		else stockValue = stockValue + variance;
		stockHistory.Add(stockValue - 4f);
		displayPriceAmount = stockValue * 100f;
		displayPrice.text = "$" + displayPriceAmount.ToString("00");


		MakeLine();
	}
	
	public void MakeLine() {
		for (int i = 0; i < stockHistory.Count; i++) {
			lr.SetPosition(i, new Vector2(i, stockHistory[i]));
			stockNumberIcon.transform.localPosition = new Vector3(stockNumberIcon.transform.localPosition.x, stockHistory[i], -1f);
		}
	}

	public void Stable() {
		direction = 0;
		hotness = 0;
	}

	public void Move(float moveRate, string moveDirection, float destination) {
		if (moveDirection == "up") {
			direction = 1;
			if (value > destination) {
				stockState = 0;
			}
		}

		if (moveDirection == "down"){
			direction = -1;
			if (value < destination) {
				stockState = 0;
			}
		}

		// 0.1f is fast movement
		// 0.001f is very slow movement
		movementSpeed = moveRate / 1000f;
	}

	public void SupplyChange(string moveDirection) {
		if (moveDirection == "up") {
			if (stockValue + 1f <= maxValue) stockValue+=1f;
			else stockValue = maxValue;
		}
		if (moveDirection == "down") {
			if (stockValue - 1f >= minValue) stockValue-=1f;
			else stockValue = minValue;
		}
	}
	
	public void Change(string moveDirection) {
		if (moveDirection == "up") {
			if (stockPrice + 1 <= maxPrice) {
				targetPrice = stockPrice + 1;
			}
			else {
				targetPrice = maxPrice;
			}
			StartCoroutine("RaisePrice");
		}

		if (moveDirection == "down") {
			if (stockPrice - 1 >= maxPrice) {
				targetPrice = stockPrice - 1;
			}
			else {
				targetPrice = minPrice;
			}

			StartCoroutine("LowerPrice");
		}




	}

	private IEnumerator RaisePrice() {
		while(stockPrice < targetPrice) {
			stockPrice += 0.01f;
			volatility += 0.001f;
			yield return null;
		}
	}
	
	private IEnumerator LowerPrice() {
		while(stockPrice > targetPrice) {
			stockPrice -= 0.01f;
			volatility += 0.001f;
			yield return null;
		}
	}


	public void Cyclical(float rate) {
		movementSpeed = rate / 100f;
		float cycleSpeed = movementSpeed * 25f;
		float tempDirection = Mathf.Sin(Time.time * cycleSpeed);
		if (tempDirection < 0f) direction = -1;
		else direction = 1;
	}

	private IEnumerator SwitchStates() {
		stockState = (int) Random.Range(0, 4);
		float waitTime = Random.Range(5, 10);
		yield return new WaitForSeconds(waitTime);
		StartCoroutine(SwitchStates());

	}

	private void TrackValue() {	
		
		float fluctuationAmmount = 1.5f * Mathf.PerlinNoise(0, Time.time * volatility);

		value = fluctuationAmmount + stockPrice;
		
		if (value < 0.25f) value = 0.25f;
		if (value > 5.75f) value = 5.75f;

		valueHistory.Add(value);
		if (valueHistory.Count > 60) {
			valueHistory.RemoveAt(0);
		}
		
		float dp = value * 100f;
		displayPrice.text = "$" + dp.ToString("00");

	}

	private IEnumerator Fluctuate() {
		variance = (Random.Range(0f, 100f) - 50) / 100f;
		yield return new WaitForSeconds(volatility);
		StartCoroutine(Fluctuate());
	}
	
	
	
}
