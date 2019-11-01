using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TraderLogin : MonoBehaviour {
    public KeyCode enter;
    public Button b;
    public Color logged_onColor;

    private bool entered;
    
    // Start is called before the first frame update
    void Start() {
        b = GetComponentInChildren<Button>();
        b.image.color = Color.white;
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(enter)) {
            if (!entered) {
                b.image.color = logged_onColor;
                Login.S.AddTrader();
                entered = true;
            }
        }
    }
}
