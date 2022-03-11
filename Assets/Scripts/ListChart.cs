using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListChart : MonoBehaviour
{
    public GameObject[] lines;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateChart());
    }

        
    private IEnumerator UpdateChart()
    {
        for (int i = 0; i < lines.Length; i++) {
            Image img = lines[i].GetComponent<Image>();
            img.fillAmount += Random.Range(-.01f, .01f);
            //img.color = new Color(img.color.r, img.color.g, img.color.b,.5f + Random.Range(-.1f, .1f));
        }

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(UpdateChart());
    }
}
