using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollorAxe : MonoBehaviour {
    public Material mat;
    public Color newColor;
    private Color oldColor;
     
	// Use this for initialization
	void Start () {
        newColor = Color.red;
        mat.color = Color.red;
        oldColor = newColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (newColor != oldColor) {
            mat.color = newColor;
            oldColor = newColor;
        }
	}
}
