using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using IBM.Watson.DeveloperCloud.Logging;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Stock : MonoBehaviour {

	public string			stockName;
	public int				stockNumber;
	public string			stockCode; // each stock has a unique code that needs to be dialed

	private List<float> 	valueHistory = new List<float>();
	public float			value;
	public float			volatility = 1;
	public float			stockPrice;
	public float			hotness;
	private int				cycleDir;
	
	public Text				displayName;
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

	public static Stock		S;

	
	
	void Awake() {
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		//displayName.color = stockColor;
		stockPrice = Random.Range(0.5f, 4.5f);
		
		CreateLineChart();
		LoadStock();
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
		string spriteLoc = "_stockIcons/stockIcon_" + stockNumber;
		Sprite s         = Resources.Load<Sprite>(spriteLoc);
		stockNumberIcon.sprite = s;

		float startingPrice = 0;
		
		for (int i = 0; i < 14; i++) {
			startingPrice += Random.Range(-0.25f, 0.25f);
			stockHistory.Add(startingPrice);
			lr.SetPosition(i, new Vector3(i, stockHistory[i], 0));
		}
	}

	private void LoadStock() {
		displayName = transform.GetChild(0).GetComponent<Text>();
		displayPrice = transform.GetChild(4).GetComponent<Text>();
		currentCode = transform.GetChild(2).GetComponent<Text>();
		displayName.text = stockName;
		// enter current code
		currentCode.text = stockCode;
		
		// Values
		volatility = Random.Range(0.25f, 1.5f);
		minValue = 1;
		maxValue = 7;
		hotness = 0f;
		cycleDir = 1;
		stockValue = Random.Range(minValue, maxValue);

		// How is the stock going to behave?
		int behavior = Random.Range(0, 2);
		if (behavior == 0) StartCoroutine(Cycle());
		if (behavior == 1) StartCoroutine(Fluctuate());
	}
	
	public IEnumerator newsEffect(string moveDirection) {
		hotness = Random.Range(2f, 6f);
		if (moveDirection == "down") hotness *= -1;		
		yield return null;
	}

	public IEnumerator UpdateStock() {
		// remove oldest number
		stockHistory.RemoveAt(0);
		
		stockValue += (variance + hotness);
		if (stockValue > maxValue) stockValue =  maxValue - 0.5f;
		if (stockValue < minValue) stockValue = minValue + 0.5f;
		
		

		
		stockHistory.Add(stockValue - 4f);
		displayPriceAmount = stockValue * 100f;
		displayPrice.text = "$" + displayPriceAmount.ToString("00");

		// Draw the Line
		MakeLine();
		hotness = 0;
		yield return new WaitForSeconds(volatility);
	
	}
	
	public void MakeLine() {
		for (int i = 0; i < stockHistory.Count; i++) {
			lr.SetPosition(i, new Vector2(i, stockHistory[i]));
			stockNumberIcon.transform.localPosition = new Vector3(stockNumberIcon.transform.localPosition.x, stockHistory[i], -1f);
		}
	}

	// build out cyclical so that it fluctuated based on variability and depth is based on hotness

	public IEnumerator Cyclical(float rate) {
		yield return new WaitForSeconds(rate);
		StartCoroutine(Cyclical(rate));
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
		//add the variance to the stock

		StartCoroutine(UpdateStock());
			
		yield return new WaitForSeconds(volatility);
		StartCoroutine(Fluctuate());
	}
	
	private IEnumerator Cycle() {
		float cycleDepth = 1f;
		variance = Random.Range(0f, 1f) * cycleDir;

		if (stockValue >= maxValue - cycleDepth) {
			stockValue = maxValue - cycleDepth;
			cycleDir = -1;
		}
		if (stockValue <= minValue + cycleDepth) {
			stockValue = minValue + cycleDepth;
			cycleDir = 1;
		}

		StartCoroutine(UpdateStock());
			
		yield return new WaitForSeconds(volatility);
		StartCoroutine(Cycle());
	}
	
	
	
}
