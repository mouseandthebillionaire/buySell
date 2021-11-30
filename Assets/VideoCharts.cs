using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoCharts : MonoBehaviour {
    public VideoClip[] charts;
    public VideoPlayer vp;

    private int currentClip;
    
    // Start is called before the first frame update
    void Start() {
        vp = GetComponentInChildren<VideoPlayer>();
        currentClip = 0;
    }

    // Update is called once per frame
    void Update()
    {
        long playerCurrentFrame = vp.frame;
        long playerFrameCount = Convert.ToInt64(vp.frameCount);
     
        if(playerCurrentFrame >= (playerFrameCount - 5)) {
            currentClip = (currentClip + 1) % charts.Length;
            vp.clip = charts[currentClip];
        }

    }
}
