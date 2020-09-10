using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopFloor : MonoBehaviour {

    private TouchThe0Floor touchThe0Floor;
    private text signalText;
	// Use this for initialization
	void Start () {
        touchThe0Floor = GameObject.Find("Floor0").GetComponent<TouchThe0Floor>();
        signalText = GameObject.Find("Count Hammer").GetComponent<text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        if (Input.GetKeyDown(KeyCode.E) && col.name == "FPSController") {
            print("TopFloorStart: Telos");
            winGame();
        }
    }
    
    void OnTriggerStay(Collider col) {
        if (Input.GetKeyDown(KeyCode.E) && col.name == "FPSController") {
            print("TopFloorStay: Telos");
            winGame();
        }
    }
    public void winGame() {
        signalText.victory = true; 
        touchThe0Floor.GameOver();
    }
}
