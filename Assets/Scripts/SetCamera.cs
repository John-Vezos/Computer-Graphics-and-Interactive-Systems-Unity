using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour {
    //The sphere. for player.
    public GameObject sphere;
    //Cameras 2.
    public Camera MainCamera;
    public Camera FPSCamera;

    //printText hold the status of player. (win or lose).
    private text printText;
    //Fps controller disable when peripherial camera and change pos. with Black Sphere.
    private GameObject fpsControler;
    private GameObject sphereInPlayerPos; //instatiante.
    //Disable and endable audioListener.
    private AudioListener mainCamerAudioSource;
    //Material change in periferial camera and playerCamera.
    private Material goMat;
    //SetCamera in World.
    private Vector3[] cameraPosition;
    private Quaternion[] cameraRotation;
    private int offset;
    private int countR; // 4 position for camera.
    private float mazeX; // mazeX * (size of Cubes) = the real mazeX.
    private float mazeY;
    private bool startMove; //camera Moving. disable by default.
    private bool moveRight; //true == right / false == left;
    private bool pauseCamera;   //disable by default. enable with input(P).
    //Lights.
    private Light terrainLight;
    //Camera ZoomIn and ZoomOut.
    private float zoomCamera;
    private float sensitivity;
    private int maxZoomIn;
    private int maxZooomOut;
    

	// Use this for initialization
	void Start () {
        mazeX = GameObject.Find("emptyMainMazeKappa").GetComponent<MainMazeKappa>().dimensionX;
        mazeY = GameObject.Find("emptyMainMazeKappa").GetComponent<MainMazeKappa>().dimensionY;
        offset = GameObject.Find("emptyMainMazeKappa").GetComponent<MainMazeKappa>().offset;
        terrainLight = GameObject.Find("Spotlight(Clone)").GetComponent<Light>();
        fpsControler = GameObject.Find("FPSController");
        printText = GameObject.Find("Count Hammer").GetComponent<text>();
        mainCamerAudioSource = GameObject.Find("MainCamera").GetComponent<AudioListener>();

        cameraPosition = new Vector3[4];
        cameraRotation = new Quaternion[4];
        //5 possition for Camera Main_Camer (not Controller Camera).
        //4 possitions in Table. rotate.
        //1 possitions only for the first time.
        cameraPosition[0] = new Vector3 ((mazeX * 2), (mazeY * 2) + (mazeY / 2), -(mazeX + offset));
        cameraRotation[0] = Quaternion.Euler(45, 0, 0);
        cameraPosition[1] = new Vector3((mazeX * 5) + offset, (mazeY * 2) + (mazeY / 2), (mazeX * 2));
        cameraRotation[1] = Quaternion.Euler(45, -90, 0);
        cameraPosition[2] = new Vector3((mazeX * 2), (mazeY * 2) + (mazeY / 2), (mazeX * 5) + offset);
        cameraRotation[2] = Quaternion.Euler(45, -180, 0);
        cameraPosition[3] = new Vector3(-(mazeX + offset), (mazeY * 2) + (mazeY / 2), (mazeX * 2));
        cameraRotation[3] = Quaternion.Euler(45, -270, 0);
        //When start this. Disable my Player Camera. I cant start with player cammera OFF.
        //So here disable it and start with Player Camera off after create myPlayer(created in Awake() MainMazeKappa).
        MainCamera.enabled = !MainCamera.enabled;
        FPSCamera.enabled = !MainCamera.enabled;
        fpsControler.SetActive(false);
        //Only for the first time.
        MainCamera.transform.position = new Vector3(mazeX * 2, (mazeY * 2) + (mazeY / 2), mazeX * 2);
        MainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        sphereInPlayerPos = Instantiate(sphere, fpsControler.transform.position, Quaternion.identity);
        countR = 1;
        startMove = false;
        moveRight = true;
        pauseCamera = false;
        sensitivity = 10f;
        maxZoomIn = 10;
        maxZooomOut = 60;
    }
	
	// Update is called once per frame
	void Update () { 
        if (Input.GetKeyDown(KeyCode.V)) {
            //Change camera.
            if (startMove) startMove = !startMove;
            MainCamera.enabled = !MainCamera.enabled;
            FPSCamera.enabled = !MainCamera.enabled;
            //Create Player created with disable this. and i want disable this every time to change camera from myPlayer to MainCamera(sceneCamera) and back.
            if (MainCamera.enabled) {
                sphereInPlayerPos.SetActive(true);
                sphereInPlayerPos.transform.position = fpsControler.transform.position;
                fpsControler.SetActive(false);
                mainCamerAudioSource.enabled = true;
                GameObject[] gos = GameObject.FindGameObjectsWithTag("CubeInScene"); 
                foreach (GameObject go in gos) {
                    //a = 0.667. add some transparate. 1 = 100% = 255, 0 = 0 = 0, 0.667f = 0.667% = 170
                    goMat = go.GetComponent<Renderer>().material;
                    Color altColor = goMat.color;
                    //altColor.a = 0.667f;
                    altColor.a = 0.367f;
                    goMat.color = altColor;
                    //Transparate. Need this code for upadte your changes.
                    // With out this. cubes never changed from Opaque in Transparate.
                    goMat.SetFloat("_Mode", 3);
                    goMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    goMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    goMat.SetInt("_ZWrite", 0);
                    goMat.DisableKeyword("_ALPHATEST_ON");
                    goMat.DisableKeyword("_ALPHABLEND_ON");
                    goMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    goMat.renderQueue = 3000;
                }
            } else {
                sphereInPlayerPos.SetActive(false);
                //When you lose or Win you cant move anymore. but you can play with camera!.
                if (!printText.gameOver && !printText.victory) {
                    fpsControler.SetActive(true);
                    mainCamerAudioSource.enabled = false;
                }
                GameObject[] gos = GameObject.FindGameObjectsWithTag("CubeInScene");
                foreach (GameObject go in gos) {
                    //a == 1. 0 transaprate.
                    goMat = go.GetComponent<Renderer>().material;
                    Color altColor = goMat.color;
                    altColor.a = 1f;
                    goMat.color = altColor;
                    //Opaque. Need this code for update your changes.
                    //With out this. in big distance from cubes start to scale.
                    goMat.SetFloat("_Mode", 0);
                    goMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    goMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    goMat.SetInt("_ZWrite", 1);
                    goMat.DisableKeyword("_ALPHATEST_ON");
                    goMat.DisableKeyword("_ALPHABLEND_ON");
                    goMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    goMat.renderQueue = -1;
                } 
            }
        }
        //Get the cameras possition in Row. Not Random.
        if (Input.GetKeyDown(KeyCode.R) && MainCamera.enabled) {
            terrainLight.intensity = 1f;
            MainCamera.transform.position = cameraPosition[Mathf.Abs(countR) % 4];
            MainCamera.transform.rotation = cameraRotation[Mathf.Abs(countR) % 4];
            if (!startMove) startMove = !startMove;
            if (moveRight) countR++;
            else countR--; 
        }
        if (startMove) {
            zoomCamera = MainCamera.fieldOfView;
            zoomCamera += Input.GetAxis("Mouse ScrollWheel") * -sensitivity;
            zoomCamera = Mathf.Clamp(zoomCamera, maxZoomIn, maxZooomOut);
            MainCamera.fieldOfView = zoomCamera;
            if (Input.GetKeyDown(KeyCode.D) && !moveRight) moveRight = !moveRight;
            if (Input.GetKeyDown(KeyCode.A) && moveRight) moveRight = !moveRight;
            if (Input.GetKeyDown(KeyCode.P)) pauseCamera = !pauseCamera;
            if (pauseCamera) MainCamera.transform.RotateAround(new Vector3(mazeX * 2, 1, mazeX * 2), new Vector3(0f, 1f, 0f), 0f);
            else if (moveRight) MainCamera.transform.RotateAround(new Vector3(mazeX * 2, 1, mazeX * 2), new Vector3(0f, 1f, 0f), -20 * Time.deltaTime);
            //Center (target position) , diference x.0f, y.1f, z.0f, speedRotate.
            else MainCamera.transform.RotateAround(new Vector3(mazeX * 2, 1, mazeX * 2), new Vector3 (0f, 1f, 0f), 20 * Time.deltaTime);
            //Target on posision and lock ur camera there.
            MainCamera.transform.LookAt(new Vector3(mazeX * 2, 1, mazeX * 2));
        }
    }
}
