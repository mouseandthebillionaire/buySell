using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chart : MonoBehaviour {
    
    public Sprite[]                charts;
    private SpriteRenderer         sr;
    
    // Start is called before the first frame update
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine("LoadChart");
    }

    public IEnumerator LoadChart() {
        int i = (int)Random.Range(0, charts.Length);
        sr.sprite = charts[i];
        yield return new WaitForSeconds(5f);
        StartCoroutine("LoadChart");
    }
}
