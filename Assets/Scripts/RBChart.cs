using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RBChart : MonoBehaviour {
    public GameObject bar;
    public int numBars;
    private List<GameObject> bars = new List<GameObject>(29);
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numBars; i++) {
            GameObject b = GameObject.Instantiate(bar) as GameObject;
            b.transform.parent = this.transform;

            b.transform.localPosition = new Vector3(25 * i + 90, -430, 0);
            Image img = b.GetComponent<Image>();
            img.fillAmount = Random.Range(0f, 1f);
            img.color = new Color(img.color.r, img.color.g, img.color.b, Random.Range(.4f, 1f));
            bars.Add(b);
        }

        StartCoroutine(UpdateChart());
    }

    // Update is called once per frame
    private IEnumerator UpdateChart()
    {
        for (int i = 0; i < numBars; i++) {
            Image img = bars[i].GetComponent<Image>();
            img.fillAmount += Random.Range(-.1f, .1f);
            //img.color = new Color(img.color.r, img.color.g, img.color.b,.5f + Random.Range(-.1f, .1f));
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UpdateChart());
    }
}
