using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelScript : MonoBehaviour {

	private float		xLoc;

	// Use this for initialization
	void Start () {
		xLoc = 4.9f;
	}
	
	// Update is called once per frame
	void Update () {
		xLoc -= GameManager.S.tradingSpeed * (Time.deltaTime * 100);
		
		this.transform.localPosition = new Vector3(xLoc, this.transform.localPosition.y);

		if (xLoc < -5) {
			Destroy(this.gameObject);
		}
	}
}
