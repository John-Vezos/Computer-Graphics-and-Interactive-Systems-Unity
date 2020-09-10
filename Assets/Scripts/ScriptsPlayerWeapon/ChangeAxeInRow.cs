using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAxeInRow : MonoBehaviour {

    public int[] holdAxes;
    public bool changeAxes;
    public Mesh[] meshAxes;
    private text printText;
    private int indexAxeToUse;
    private MeshFilter meshFilterAxe;
    private MeshRenderer meshRendererAxe;
    private BoxCollider boxColliderAxe;



    // Use this for initialization
    void Start() {
        holdAxes = new int[4];
        meshRendererAxe = GetComponent<MeshRenderer>();
        boxColliderAxe = GetComponent<BoxCollider>();
        printText = GameObject.Find("Count Hammer").GetComponent<text>();
        meshFilterAxe = GetComponent<MeshFilter>();
        for (int i = printText.countHammer; i > 0; i--) {
            holdAxes[Random.Range(0, holdAxes.Length)]++;
        }
        for (int i = 0; i < 4; i++) {
            Debug.Log(holdAxes[i]);
        }
        changeAxes = false;
        setAxe();
       
}
	
	// Update is called once per frame
	void Update () {
		if (changeAxes) {
            setAxe();
        }
	}
    public void setAxe () {
        indexAxeToUse = -1;
        for (int i = 0; i < holdAxes.Length; i++) {
            if (holdAxes[i] != 0) {
                indexAxeToUse = i;
                holdAxes[i]--;
                break;
            }
        }

        if (indexAxeToUse == -1) {
            meshRendererAxe.enabled = false;
            boxColliderAxe.enabled = false;
            meshFilterAxe.mesh = null;
        } else {
            if (!meshRendererAxe.enabled) {
                meshRendererAxe.enabled = true;
                boxColliderAxe.enabled = true;
            }

            meshFilterAxe.mesh = meshAxes[indexAxeToUse];
        }
        changeAxes = false;
    }

}
