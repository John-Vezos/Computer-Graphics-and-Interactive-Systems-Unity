using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchThe0Floor : MonoBehaviour {

    private GameObject myPlayer;
    private Camera mainCamera;
    private Vector3 posFPSController;
    private AudioListener myCameraAudioListener;
    private GameObject myObjectMainCammera;
    private text printText;
	// Use this for initialization
	void Start () {
        myPlayer = GameObject.Find("FPSController");
        myObjectMainCammera = GameObject.Find("MainCamera");
        mainCamera = myObjectMainCammera.GetComponent<Camera>();
        myCameraAudioListener = myObjectMainCammera.GetComponent<AudioListener>();
        printText = GameObject.Find("Count Hammer").GetComponent<text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.X)) {
            printText.gameOver = true;
            GameOver();
        }
    }
    void OnTriggerEnter(Collider col) {
        if (col.name == "FPSController") {
            print("startTouchFloor0S: Telos");
            printText.gameOver = true;
            GameOver();
        }
    }

   void OnTriggerStay(Collider col) {
        if (col.name == "FPSController") {
            print("stayTouchFloor0: Telos");
            printText.gameOver = true;
            GameOver();
        }
    }

    public void GameOver() {
        posFPSController = myPlayer.transform.localPosition;
        myPlayer.SetActive(false);
        mainCamera = Instantiate(mainCamera, posFPSController, Quaternion.identity);
        myCameraAudioListener.enabled = true;
        mainCamera.enabled = true;
    }
}
