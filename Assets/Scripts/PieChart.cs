using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour
{
    public GameObject[] wedges = new GameObject[4];
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateChart());
    }

    private IEnumerator UpdateChart()
    {
        for (int i = 0; i < wedges.Length; i++) {
            Image img = wedges[i].GetComponent<Image>();
            img.fillAmount += Random.Range(-.05f, .05f);
            //img.color = new Color(img.color.r, img.color.g, img.color.b,.5f + Random.Range(-.1f, .1f));
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateChart());
    }
}
