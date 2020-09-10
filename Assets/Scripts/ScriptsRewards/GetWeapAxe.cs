using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapAxe : MonoBehaviour {

    public AudioClip getUpWeapon;
    public AudioClip dropWeapon;
    private AxeRandom axeRandom;
    private AudioSource audSource;
    private MeshRenderer meshRenderer;
    private ChangeAxeInRow changeAxeInRow;
    private text printText;
    private int dieAfter5Frames;
    private int count;
    private bool fpsTouchMe;

    void Awake() {
        this.GetComponent<AudioSource>().PlayOneShot(dropWeapon, 2f);
        axeRandom = GetComponent<AxeRandom>();
        audSource = this.GetComponent<AudioSource>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        changeAxeInRow = GameObject.Find("Weapon_30").GetComponent<ChangeAxeInRow>();
        printText = GameObject.Find("Count Hammer").GetComponent<text>();
        dieAfter5Frames = 0;
        fpsTouchMe = false;
    }

    void OnTriggerEnter(Collider col) {
        Debug.Log(col.name);
        //For destroy axeDrop need to you are myPlayer(FPSController).
        //and axeHaveMeshRenderer.
        if (col.name == "FPSController" && meshRenderer.enabled) {
            fpsTouchMe = true;
            printText.countHammer += 1;
            count = axeRandom.selectedAxe;
            Debug.Log("Vrhkes ena axe:" + count);
            changeAxeInRow.holdAxes[count]++;
            //get weapon sound.
            audSource.PlayOneShot(getUpWeapon, 5f);
            //Disable MeshRenderer.
            //You cant get this weapon up to one time.
            meshRenderer.enabled = false;
        }
        //Wait 5frames. and destroy.
        //I want Hear the sound.
        if (fpsTouchMe) {
            if (dieAfter5Frames > 5) {
                Destroy(transform.parent.gameObject);
            }
            dieAfter5Frames++;
        }
    }
}
