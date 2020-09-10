using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnime : MonoBehaviour {
    public Animator anim;
    public bool animPlay;
    public bool destroy;
    public int dmgTakenCube;
    
    
    private float animTime;
    private GameObject myPlayer;


    // Update is called once per frame
    void Start() {
        myPlayer = GameObject.Find("FPSController");
        anim = GetComponent<Animator>();
        animPlay = false;   //disable by default.
        destroy = false;    //disable by default.
        dmgTakenCube = 0;
    }
    void Update() {
        //fire1 AND Avatar -is Active.
        //Lap top lag with this touchPad leftClick when jump and click it.
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.B)) && myPlayer.activeSelf) {
            //Anim itsnt in progress.
            if (!animPlay) {
                Debug.Log("Hit");
                anim.Play("HitAnimationAxe", 0, 0f);    //Animation Start.
                animPlay = true;    //Flag for animation Start.
                destroy = true; //Flag you can break Cubes now.
                animTime = 0;   //Anim progress = 0.just begin.
                dmgTakenCube = -1;
            }
        }
        //Anim is in progress.
        if (animPlay) {
            //Anim ended.
            if ((animTime += Time.deltaTime) /*count the time of anim.*/ > 0.6f) {
                animPlay = false;
                destroy = false;
                dmgTakenCube = 0;
            }
        }
    }
}
       
    

