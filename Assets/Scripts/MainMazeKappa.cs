using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMazeKappa : MonoBehaviour {
    public GameObject CubeBlue;
    public GameObject CubeRed;
    public GameObject CubeGreen;
    public GameObject CubeT1;
    public GameObject CubeT2;
    public GameObject CubeT3;
    public GameObject CubeBlack;
    public GameObject CubeFloor;
    public GameObject spotLightMaze;
    
    public int hammers;
    public int dimensionX, dimensionY, dimensionZ = -1;
    public int offset;

    private GameObject clone;
    private GameObject cloneFloor;
    private GameObject spotLightMaze1;
    private string[] mazeText;
    private string filePath;
    private string[] splitter;
    private int x, z;
    private int playerGetPositionSafe;
    private int playerGetPositionWarning;
    private int randomPosition;
    private int countTeleportCubes;
    private Vector3[] randomPositionForPlayer;
    private Vector3[] warningPositionForPlayer;
    private Vector3[] teleportCubes;

    // Use this for initialization
    void Awake() {
        //Debug.Log(camInScene.Length);
        //Load file.maze
        filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "file1.maz");
        mazeText = System.IO.File.ReadAllLines(filePath);

        //Dimensions from maze
        for (int i = 0; i < 3; i++) {
            splitter = mazeText[i].Split('=');
            if (splitter.Length == 0) return;
            if (splitter[0].Equals("L")) {
                try {
                    z = Int32.Parse(splitter[1]);
                    teleportCubes = new Vector3[(z - 1) * 2]; //first level no-teleportCube.
                } catch (FormatException e) {
                    Debug.Log(e.Message);
                    z = 0;
                }
            } else if (splitter[0].Equals("N")) {
                try {
                    x = Int32.Parse(splitter[1]);
                } catch (FormatException e) {
                    Debug.Log(e.Message);
                    x = 0;
                }
            } else if (splitter[0].Equals("K")) {
                try {
                    hammers = Int32.Parse(splitter[1]);
                } catch (FormatException e) {
                    Debug.Log(e.Message);
                    hammers = 0;
                }
                GameObject.Find("Count Hammer").GetComponent<text>().countHammer = hammers;
            }
        }
        if (x == 0) return;
        if (z == 0) return;


        randomPositionForPlayer = new Vector3[x * x];
        warningPositionForPlayer = new Vector3[x * x];
        playerGetPositionSafe = 0;
        playerGetPositionWarning = 0;

        //cubes Size 2. so the half for the cente of the center
        //floor
        cloneFloor = Instantiate(CubeFloor, new Vector3((x * 2) - 1, -0.5f, (x * 2) - 1), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3((x * 4) * 2, 1, (x * 4) * 2);
        cloneFloor.AddComponent<TouchThe0Floor>();
        (cloneFloor).gameObject.name = "Floor0";
        //upFloor
        cloneFloor = Instantiate(CubeFloor, new Vector3((x * 2) - 1, (z * 2) + (0.500001f), (x * 2) - 1), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3(x * 2, 1, x * 2);
        cloneFloor.AddComponent<TopFloor>();
        cloneFloor.gameObject.name = "RoofTop";
        //RoofTopBlockFPSController
        cloneFloor = Instantiate(CubeFloor, new Vector3((x * 2) - 1, (z * 2) + (0.500001f) + 2f, (x * 2) - 1), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3(x * 2, 1, x * 2);
        cloneFloor.GetComponent<BoxCollider>().isTrigger = false;
        //Wall the first from X.
        cloneFloor = Instantiate(CubeFloor, new Vector3(x * 2 - 1, z + 0.5f, x - 1.5f), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3(x * 2, z * 2 + 3, 1);
        cloneFloor.GetComponent<BoxCollider>().isTrigger = false;
        //Wall the second for X.
        cloneFloor = Instantiate(CubeFloor, new Vector3(x * 2 - 1, z + 0.5f, x * 3 - 0.5f), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3(x * 2, z * 2 + 3, 1);
        cloneFloor.GetComponent<BoxCollider>().isTrigger = false;
        //wall on y
        cloneFloor = Instantiate(CubeFloor, new Vector3(x - 1.5f, z + 0.5f, x * 2 - 1), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3(x * 2, z * 2 + 3, 1);
        cloneFloor.transform.localRotation = Quaternion.Euler(0, 90, 0);
        cloneFloor.GetComponent<BoxCollider>().isTrigger = false;
        //wall on y second
        cloneFloor = Instantiate(CubeFloor, new Vector3(x * 3 - 0.5f, z + 0.5f, x * 2 - 1), Quaternion.identity);
        cloneFloor.transform.localScale = new Vector3(x * 2, z * 2 + 3, 1);
        cloneFloor.transform.localRotation = Quaternion.Euler(0, 90, 0);
        cloneFloor.GetComponent<BoxCollider>().isTrigger = false;

        offset = x / 2;

        //light
        spotLightMaze1 = Instantiate(spotLightMaze, new Vector3((x * 2) - 1, (z * 4) + (0.500001f), (x * 2) - 1), Quaternion.identity);
        spotLightMaze1.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //Reflection Light on Cubes High.
        spotLightMaze1.GetComponent<Light>().intensity = 0.4f;

        countTeleportCubes = 0;
        //Create Cubes in Maze
        foreach (string line in mazeText) {
            //Remove multi spaces.
            //(get this form:)       info1<multiSpaces>info2<multiSpaces>info3<multiSpaces>etc 
            //(i want this form:)    info1<singleSplace>info2<singleSpace>info3<singleSpace>etc.
            String beforeReplace = "";
            String afterReplace = "";
            do {
                beforeReplace = afterReplace;
                afterReplace = line.Replace("  ", " ");
            } while (!beforeReplace.Equals(afterReplace));

            //When find "LEVEL" new layer is here. dont care the number its not safe.
            splitter = afterReplace.Split(' ');
            if (splitter[0].Equals("LEVEL")) {
                dimensionZ++;
                dimensionY = 0;
            } else if (splitter.Length == x) {
                for (dimensionX = 0; dimensionX < x; dimensionX++) {
                    if (splitter[dimensionX].Equals("B")) {
                        clone = Instantiate(CubeBlue, new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2), Quaternion.identity);
                        //Clopy-paste the Vector3 position of cube for spwan there Weapon when destroy this obj.
                        clone.GetComponent<CubeLife>().posCube = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.4f) * 2, (dimensionY + offset) * 2);
                    } else if (splitter[dimensionX].Equals("R")) {
                        clone = Instantiate(CubeRed, new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2), Quaternion.identity);
                        //Clopy-paste the Vector3 position of cube for spwan there Weapon when destroy this obj.
                        clone.GetComponent<CubeLife>().posCube = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.4f) * 2, (dimensionY + offset) * 2);
                    } else if (splitter[dimensionX].Equals("G")) {
                        clone = Instantiate(CubeGreen, new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2), Quaternion.identity);
                        //Clopy-paste the Vector3 position of cube for spwan there Weapon when destroy this obj.
                        clone.GetComponent<CubeLife>().posCube = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.4f) * 2, (dimensionY + offset) * 2);
                    } else if (splitter[dimensionX].Equals("T1")) {
                        clone = Instantiate(CubeT1, new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2), Quaternion.identity);
                        //Clopy-paste the Vector3 position of cube for spwan there Weapon when destroy this obj.
                        clone.GetComponent<CubeLife>().posCube = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.4f) * 2, (dimensionY + offset) * 2);
                    } else if (splitter[dimensionX].Equals("T2")) {
                        clone = Instantiate(CubeT2, new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2), Quaternion.identity);
                        //Clopy-paste the Vector3 position of cube for spwan there Weapon when destroy this obj.
                        clone.GetComponent<CubeLife>().posCube = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.4f) * 2, (dimensionY + offset) * 2);
                    } else if (splitter[dimensionX].Equals("T3")) {
                        clone = Instantiate(CubeT3, new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2), Quaternion.identity);
                        //Clopy-paste the Vector3 position of cube for spwan there Weapon when destroy this obj.
                        clone.GetComponent<CubeLife>().posCube = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.4f) * 2, (dimensionY + offset) * 2);
                    } else if (splitter[dimensionX].Equals("W") && dimensionZ != 0) { //teleport in 0 level doenst metter..  you lose there.
                        teleportCubes[countTeleportCubes] = new Vector3((dimensionX + offset) * 2, (dimensionZ + 0.500001f) * 2, (dimensionY + offset) * 2);
                        countTeleportCubes++;
                    } else if (splitter[dimensionX].Equals("E") && dimensionZ == 0) {
                        warningPositionForPlayer[playerGetPositionWarning] = new Vector3((dimensionX + offset) * 2, 1.5f, (dimensionY + offset) * 2);
                        playerGetPositionWarning++;
                    } else if (splitter[dimensionX].Equals("E") && dimensionZ == 1) {
                        Vector3 pos = new Vector3((dimensionX + offset) * 2, 1.5f, (dimensionY + offset) * 2);
                        bool safe = true;
                        for (int j = 0; j < warningPositionForPlayer.Length; j++) {
                            if (pos == warningPositionForPlayer[j]) {
                                safe = false;
                                break;
                            }
                        }
                        if (safe) {
                            randomPositionForPlayer[playerGetPositionSafe] = pos;
                            playerGetPositionSafe++;
                        }
                    }
                }
                dimensionY++;
            }
        }
        if (countTeleportCubes > 1) {
            for (int index = 1; index < countTeleportCubes; index += 2) {
                clone = Instantiate(CubeBlack, teleportCubes[index - 1], Quaternion.identity);
                clone.GetComponent<TpCB>().teleportPosition = teleportCubes[index];
                clone = Instantiate(CubeBlack, teleportCubes[index], Quaternion.identity);
                clone.GetComponent<TpCB>().teleportPosition = teleportCubes[index - 1];
            }
        }
        if (randomPositionForPlayer.Length == 0) return;
        randomPosition = UnityEngine.Random.Range(0, playerGetPositionSafe);
        //i hold in this table the Center of Cubes. Cube size(2) so need +1. and .080007 its the center of fps controller.
        GameObject.Find("FPSController").transform.localPosition = randomPositionForPlayer[randomPosition] + new Vector3(0f, 1.080007f, 0f);

        //try to clear memory.
        teleportCubes = null;
        randomPositionForPlayer = null;
        warningPositionForPlayer = null;
        mazeText = null;
        splitter = null;
        clone = null;
        cloneFloor = null;
        spotLightMaze1 = null;
        filePath = null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
