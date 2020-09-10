using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeRandom : MonoBehaviour {


    public Mesh[] meshAxes;
    public int selectedAxe;
    public Material[] mats;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
   
    // Use this for initialization
    void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        //mats[0] is the first mat of weapon and here is Clone to have red color.
        //else the spwan Axe have the same color that axe i hold.
        meshRenderer.materials[0] = mats[0];
        meshRenderer.materials[1] = mats[1];         
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = meshAxes[selectedAxe = Random.Range(0, 4)];  
    }

}
