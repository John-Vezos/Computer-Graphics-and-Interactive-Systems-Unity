using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAxeWithCube : MonoBehaviour {
    public AudioClip axeHitCube;
    public int dmgTakenCube;

    private WeaponAnime animAxe;
    private int dmgReturnToAxeFromCube;
       
    // Use this for initialization
    void Start() {
        animAxe = this.gameObject.GetComponent<WeaponAnime>();
        dmgReturnToAxeFromCube = -10;
        dmgTakenCube = 0;
    }

    void OnCollisionEnter (Collision col) {
        if (animAxe.destroy) {
            if (col.collider.name == "CubeBlue(Clone)"
               || col.collider.name == "CubeRed(Clone)"
               || col.collider.name == "CubeGreen(Clone)"
               || col.collider.name == "CubeMat1(Clone)"
               || col.collider.name == "CubeMat2(Clone)"
               || col.collider.name == "CubeMat3(Clone)") {
                hitSomething(col);
            }
        }
    }

    void OnCollisionStay (Collision col) {
        if (animAxe.destroy) {
            if (col.collider.name == "CubeBlue(Clone)"
                || col.collider.name == "CubeRed(Clone)"
                || col.collider.name == "CubeGreen(Clone)"
                || col.collider.name == "CubeMat1(Clone)"
                || col.collider.name == "CubeMat2(Clone)"
                || col.collider.name == "CubeMat3(Clone)") {
                hitSomething(col);
            }
        }
    }

    private void hitSomething(Collision col) {
        col.collider.GetComponent<AudioSource>().PlayOneShot(axeHitCube, 1f);
        col.collider.GetComponent<CubeLife>().life += animAxe.dmgTakenCube;
        this.gameObject.GetComponent<LifeOfAxe>().lifeAxe += dmgReturnToAxeFromCube;
        animAxe.destroy = false; 
    }
}
