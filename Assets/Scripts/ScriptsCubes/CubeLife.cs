using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLife : MonoBehaviour {

    public int life;
    public GameObject WeaponAxe1;
    public GameObject includeAxe;
    public AudioClip crashDustCube;

    private int cubeDustDestoy;
    private GameObject cubeDust;
    public GameObject cloneAxe; //private Warning never used.
    public GameObject cloneInlcudeAxe;
    public Vector3 posCube;
    private Vector3[] cubeDustPosition;

    // Use this for initialization
    void Start () {
        life = 3;
        cubeDustDestoy = 0;
        cubeDustPosition = new Vector3[8];
        //the position of the smaller cubes.
        //Random can give you the same position or small disistance bettwen two or more cubes.
        cubeDustPosition[0] = new Vector3(0.3f, 0f, 0f);
        cubeDustPosition[1] = new Vector3(0.4f, 0.65f, 0f);
        cubeDustPosition[2] = new Vector3(0.4f, 0.45f, 0.5f);
        cubeDustPosition[3] = new Vector3(0f, 0.5f, 0f);
        cubeDustPosition[4] = new Vector3(0f, 0.5f, 0.6f);
        cubeDustPosition[5] = new Vector3(0.6f, 0f, 0.7f);
        cubeDustPosition[6] = new Vector3(0.9f, 0.5f, 0.4f);
        cubeDustPosition[7] = new Vector3(0.8f, 0.75f, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
        if (life <= 0) {
            if (Random.Range(1, 7) == 1) {
                cloneInlcudeAxe = Instantiate(includeAxe.gameObject, posCube, Quaternion.identity);
                cloneAxe = Instantiate(WeaponAxe1.gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
                cloneAxe.transform.parent = cloneInlcudeAxe.transform;   
            }
            for (int i = 0; i < 8; i++) {
                cubeDust = Instantiate(this.gameObject, posCube + cubeDustPosition[i], Quaternion.identity);
                cubeDust.GetComponent<Transform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
                cubeDust.AddComponent<Rigidbody>();
                cubeDust.AddComponent<AudioSource>();
                cubeDust.GetComponent<AudioSource>().enabled = true;
                cubeDust.AddComponent<AudioSource>().PlayOneShot(crashDustCube, 2f);
            }
            Destroy(this.gameObject);
        }
        //dust cube destroy after 380 frames.
        if (gameObject.name == "CubeBlue(Clone)(Clone)" 
            || gameObject.name == "CubeRed(Clone)(Clone)" 
            || gameObject.name == "CubeGreen(Clone)(Clone)" 
            || gameObject.name == "CubeMat1(Clone)(Clone)" 
            || gameObject.name == "CubeMat2(Clone)(Clone)" 
            || gameObject.name == "CubeMat3(Clone)(Clone)") {
            if (cubeDustDestoy > 380) {
                Destroy(this.gameObject);
            }
            cubeDustDestoy++;
        }
	}
}
