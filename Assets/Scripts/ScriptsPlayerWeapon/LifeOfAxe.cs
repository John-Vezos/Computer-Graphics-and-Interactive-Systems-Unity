using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOfAxe : MonoBehaviour {

    private int lifeAxeBackUp;

    public int lifeAxe;
    private ChangeAxeInRow getNextAxe;
    private CollorAxe colorAxe;
    private text printText;
    private MeshRenderer meshRendererAxe;

    // Use this for initialization
    void Start() {
        lifeAxe = 100;
        getNextAxe = GetComponent<ChangeAxeInRow>();
        colorAxe = GetComponent<CollorAxe>();
        meshRendererAxe = GetComponent<MeshRenderer>();
        printText = GameObject.Find("Count Hammer").GetComponent<text>();
    }

    // Update is called once per frame
    void Update() {
        if (printText.countHammer > 0 && !meshRendererAxe.enabled) {
            getNextAxe.changeAxes = true;
            lifeAxe = 100;
            lifeAxeBackUp = lifeAxe;
            printText.countHammer--;
            axeGetColor();
        }

        //for dont call all the time the axeGetColor.
        if (lifeAxeBackUp != lifeAxe) {
            if (lifeAxe <= 0) {
                if (printText.countHammer > 0) {
                    printText.countHammer--;
                    if (printText.countHammer == 0) lifeAxe = 0;
                    else lifeAxe = 100;
                }
                getNextAxe.changeAxes = true;
            }
            lifeAxeBackUp = lifeAxe;
            axeGetColor();
        }
        
    }
    //Set color on axe.
    public void axeGetColor() { 
        //Life and collor!
        if (lifeAxe > 60) {
            colorAxe.newColor = Color.Lerp(Color.red, Color.black, 0);
        } else if (lifeAxe > 30) {
            colorAxe.newColor = Color.Lerp(Color.red, Color.black, 0.5f);
        } else if (lifeAxe > 0) {
            colorAxe.newColor = Color.Lerp(Color.red, Color.black, 1f);
        }
        Debug.Log("AxeLife: " + lifeAxe);
    }
}
