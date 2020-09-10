using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpCB : MonoBehaviour {

    public AudioClip teleportSound;
    public Vector3 teleportPosition;

    private GameObject fpsController;
    private Light light1;
    private AudioSource audioSourceTpCB;
    private float coolDown;

    // Use this for initialization
    void Awake() {
        teleportPosition = this.gameObject.transform.position;
        fpsController = GameObject.Find("FPSController");
        light1 = this.GetComponent<Light>();
        audioSourceTpCB = this.GetComponent<AudioSource>();
        light1.enabled = false;
        coolDown = 0;
    }
    
    void OnTriggerStay(Collider col) {
        if (col.name == "FPSController") {
            if ((coolDown += Time.deltaTime) >= 3f) fpsController.transform.localPosition = teleportPosition; //teleport possition.
            else if (coolDown > 1.8f && coolDown < 2f) audioSourceTpCB.PlayOneShot(teleportSound, 2f); // start the sound.
            else if (coolDown > 2.5f) light1.enabled = !light1.enabled; //This light turn off-on. for effe.
        }
    }
    void OnTriggerExit(Collider col) {
        coolDown = 0;
        light1.enabled = false;
    }  
}
