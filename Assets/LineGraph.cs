using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LineGraph : MonoBehaviour
{

	public GameObject lr_0;

	void Start()
	{
		LineRenderer line_0 = lr_0.GetComponent<LineRenderer>();
	}

	void Update()
	{
		
	}

	private IEnumerator RunGraph()
	{
		List<Vector3> pos = new List<Vector3>();
		pos.Add(new Vector3(0, 0));
		pos.Add(new Vector3(10, 10));
		line_0.SetPositions(pos.ToArray());

		yield return null;
	}
}