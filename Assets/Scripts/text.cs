using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text : MonoBehaviour {
    public Text textCountHammer;
    public int countHammer = 0;
    private int oldHammers = 0;
    public bool gameOver;
    public bool victory;
    public float points;
    
    // Use this for initialization
    void Start () {
        gameOver = false;
        victory = false;
        points = GameObject.Find("emptyMainMazeKappa").GetComponent<MainMazeKappa>().dimensionX;
        points = points * points;
    }
	
	// Update is called once per frame
	void Update () {

        if (!gameOver && !victory) {
            points += -Time.deltaTime;
            if (points < 0) {
                GameObject.Find("Floor0").GetComponent<TouchThe0Floor>().GameOver();
                gameOver = true;
            }
        }
        if (gameOver) {
            points = 0;
            setTextCountHammer("Game Over" + "\nPoints: " + points);
            transform.position = new Vector3(900, 600, 0f);
        } else if (countHammer != oldHammers) {
            if (countHammer < oldHammers) {
                points += -50;
            }
            oldHammers = countHammer;     
        } else if (victory) {
            setTextCountHammer("Victory" + "\nPoints: " + points.ToString());
            transform.position = new Vector3(900, 600, 0f);
        } else setTextCountHammer("Hammers: " + countHammer.ToString() + "\nPoints: " + points.ToString());
    }

    public void setTextCountHammer(string printString) {
        textCountHammer.text = printString.ToString();
    }
}
