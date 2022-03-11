using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LineGraph : MonoBehaviour
{

	public GameObject lr_0;
	private LineRenderer line_0;
	private Vector3[] points = new Vector3[7];
	private float[] yVals = new float[7];

	void Start() {
		line_0 = lr_0.GetComponent<LineRenderer>();
		line_0.SetVertexCount(points.Length);
		
		for (int i = 0; i < points.Length; i++) {
			yVals[i] = Random.Range(0, 20f);
			points[i] = new Vector3(i * 5, yVals[i]);
			line_0.SetPositions(points);
			StartCoroutine(SetPoint(i));
		}
		
	}

	private IEnumerator SetPoint(int _point) {
		float timeElapsed = 0;
		float duration    = Random.Range(2f, 6f);
		float start       = yVals[_point];
		float end         = Random.Range(0, 20f);
		
		while (timeElapsed < duration) {
				yVals[_point] = Mathf.Lerp(start, end, timeElapsed / duration);
				points[_point] = new Vector3(_point * 5, yVals[_point]);
				line_0.SetPositions(points);
				timeElapsed += Time.deltaTime;
				yield return null;
		}
		yVals[_point] = end;
		yield return new WaitForSeconds(.05f);
		StartCoroutine(SetPoint(_point));
	}
}