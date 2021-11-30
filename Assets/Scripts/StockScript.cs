using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StockScript : MonoBehaviour {

	public string			stockName;
	public int				stockNumber;
	public string			stockCode; // each stock has a unique code that needs to be dialed

	private List<float> 	valueHistory = new List<float>();
	public float			value;
	public int				direction;
	public float			volatility = 1;
	public float			stockPrice;
	public float 			targetPrice;
	public float			minPrice, maxPrice;
	public float			movementSpeed;
	public int				hotness;
	
	private Text			displayName;
	private Text			displayPrice;
	private float			displayPriceAmount;
	private Text			currentCode;
	public Color			stockColor;
	
	public int				stockState; // 0 - stable, 1 - growing, 2 - falling, 3 - cycling
	
	// try new stock method
	public List <float>		stockHistory = new List<float>();
	public LineRenderer		lrPrefab;
	private LineRenderer	lr;
	public Material[]		lr_mat;
	public SpriteRenderer	stockNumberIcon;
	public float			stockValue;
	private float			variance;
	public float			minValue, maxValue;

	public static StockScript		S;

	void Awake() {
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		//displayName.color = stockColor;
		volatility = Random.Range(0.25f, 1.5f);
		stockPrice = Random.Range(0.5f, 4.5f);
		targetPrice = stockPrice;
		
		// try new stock method
		// randomly fill our list with stock prices


		// Create the Line Chart
		CreateLineChart();

		minValue = 1;
		maxValue = 7;
		stockValue = Random.Range(minValue, maxValue);
		
		// Populate the Ticker Information
		LoadStockInfo();
		
		
		
		StartCoroutine(Fluctuate());

	}

	public void SetStockCode(string _code) {
		stockCode = _code;
	}

	private void CreateLineChart() {		
		lr = Instantiate(lrPrefab) as LineRenderer;
		lr.transform.SetParent(GameObject.Find("LineGraph").transform);
		lr.sortingLayerName = "Stocks";
		lr.sortingOrder = 20;
		float offset = lr.transform.GetSiblingIndex() * 0.2f;
		lr.transform.localScale = new Vector3(0.6f, 0.75f, 1);
		lr.transform.localPosition = new Vector3(-4.5f + offset, .2f - offset, -1);
		lr.material = lr_mat[lr.transform.GetSiblingIndex()];
		
		stockNumberIcon = lr.GetComponentInChildren<SpriteRenderer>();
		string spriteLoc = "_stockNumberIcons/stockNumber_" + stockNumber;
		Sprite s         = Resources.Load<Sprite>(spriteLoc);
		stockNumberIcon.sprite = s;
		
		for (int i = 0; i < 14; i++) {
			stockHistory.Add(Random.Range(-2, 2));
			lr.SetPosition(i, new Vector3(i, stockHistory[i], 0));
		}
				
		InvokeRepeating("UpdateStock", 0.25f, 1f);
	}

	private void LoadStockInfo() {
		displayName = transform.GetChild(0).GetComponent<Text>();
		displayPrice = transform.GetChild(4).GetComponent<Text>();
		currentCode = transform.GetChild(2).GetComponent<Text>();
		displayName.text = stockName;
		// enter current code
		currentCode.text = stockCode;

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
