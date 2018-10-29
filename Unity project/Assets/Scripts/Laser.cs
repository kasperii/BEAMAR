
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/***
 * This script handles most of the game mechanics related to the laser.
 * I have not had time to comment on this monstrosity of a script. If you have any questions, ask me Erik Lindström for clarifications. 
 * */

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    [SerializeField] private Camera FirstPersonCamera;
    [SerializeField] private readonly GameObject LightBeam; //Remove readonly if not working
    [SerializeField] private GameObject Goal;
    [SerializeField] private GameObject bigObstacle;
    [SerializeField] private GameObject GoalEffect;
    [SerializeField] private Animator TransitionAnim;
    [SerializeField] private string SceneName;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject movingObject;
    [SerializeField] private GameObject winText;

    [SerializeField] private string mirrorTag;
    [SerializeField] private string detectedPlaneTag;
    [SerializeField] private string ObstacleTag;

    [SerializeField] private int maxBurnmarkCount = 50;
    [SerializeField] private GameObject[] LevelTwoObjects;
    List<GameObject> BurnmarkList = new List<GameObject>();
    public string splitTag;
    public string spawnedBeam;
    public int maxBounce;
    public int maxSplit;
    public int[] laserPoints;
    private float timer = 0;
    private LineRenderer mLineRenderer;

    internal GameObject instantiatedGoal;
    internal GameObject instantiatedObstacle;
    internal GameObject instantiatedGoalEffect;
    internal bool ChangeLevelFlag = true;


    //public AudioSource goalSound;

    private string GoalName = "Goal";
    GameObject litGoal; //Goal with light to be instantiated

    public GameObject WinText;
    //public ParticleSystem WinParticleSystem;
    [SerializeField] private GameObject LaserMark;

    private float goalTimer = 0.0f; //Start timer when raycast hits goal

    public Color BaseGoalColor = Color.black;   
    public Color GoalColor = Color.yellow;  //Color to interpolate to when raycast hits goal
    private float colorModifier = -5f;
    internal Vector3 firstGoalTrans;

    DestroyMirrors DestroyMirrors;
    internal bool levelTwoFlag = false;     //Flag to start 'level 2'

    void Start()
    {
        Material GoalMat = Goal.GetComponent<MeshRenderer>().sharedMaterial;        
        Color GoalMatEmColor = Goal.GetComponent<MeshRenderer>().sharedMaterial.GetColor("_EmissionColor");     //Fetch material and emission-variable of shader of goal
        GoalMat.SetVector("_EmissionColor", GoalColor * Mathf.LinearToGammaSpace(-5f));                         //gradially change color of emission
        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        //StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != spawnedBeam)
        {
            if (timer >= updateFrequency)
            {
                timer = 0;
                foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag(spawnedBeam))
                    Destroy(laserSplit);
                if (GameObject.FindGameObjectWithTag("Obstacle") != null)
                    StartCoroutine(RedrawLaser());
            }
            timer += Time.deltaTime;
        }
        else
        {
            mLineRenderer = gameObject.GetComponent<LineRenderer>();
            if (GameObject.FindGameObjectWithTag("Obstacle") != null)
            {
                StartCoroutine(RedrawLaser());
            }
                
        }

        //THIS IS SHIT PLEASE CLOSE YOUR EYES
        GameObject ARSurfObj = GameObject.Find("ARSurfaceManager");                 // Find object ARSurfaceManager
        ARSurfaceManager surfScript = ARSurfObj.GetComponent<ARSurfaceManager>();   // Get script from manager
        bool StartFlag = surfScript.StartFlag;                                      // Fetch bool from script from manager
                                                                                    // Only show laser when we found a plane
        if (StartFlag == true && levelTwoFlag == false)                             // StartFlag true when Startbutton is pressed. levelTwoFlag is true when the first goal has been hit. only want to instantiate once
        {
            if (!GameObject.FindGameObjectWithTag("Goal"))// == null)
            {
                firstGoalTrans = new Vector3(FirstPersonCamera.transform.position.x + 0, FirstPersonCamera.transform.position.y - 0.5f, FirstPersonCamera.transform.position.z + 4.5f);
                instantiatedGoal = Instantiate(Goal, firstGoalTrans, Quaternion.identity);

                var laserBeamTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y + 0.0f, FirstPersonCamera.transform.position.z + 0.0f);
                transform.parent.position = transform.localPosition;//laserBeamTrans;    //Move the laser to the right position
                //transform.localPosition = laserBeamTrans;
                transform.position = transform.localPosition;                           //move the linerenderer with the laser housing object

                var bigObstacleTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y - 0.25f, FirstPersonCamera.transform.position.z + 3);
                instantiatedObstacle = Instantiate(bigObstacle, bigObstacleTrans, Quaternion.identity);
            }
        }
    }

    IEnumerator RedrawLaser()
    {
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.localPosition);
        RaycastHit outHit;

        while (loopActive)
        {
            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
            if (Physics.Raycast(lastLaserPosition, laserDirection, out outHit, laserDistance))// && ((hit.transform.gameObject.tag == detectedPlaneTag) || (hit.transform.gameObject.tag == splitTag) || (hit.transform.gameObject.tag == mirrorTag))) // || (hit.transform.gameObject.tag == ObstacleTag)))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * outHit.distance, Color.yellow);
                laserReflected++;
                vertexCounter += 3;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(outHit.point, lastLaserPosition, 0.01f));
                mLineRenderer.SetPosition(vertexCounter - 2, outHit.point);
                mLineRenderer.SetPosition(vertexCounter - 1, outHit.point);
                mLineRenderer.SetWidth(.02f, .02f);
                lastLaserPosition = outHit.point;

                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, outHit.normal);

                if (outHit.transform.tag == splitTag) //|| (outHit.transform.gameObject.tag == splitTag)) //(hit.transform.gameObject != Goal) &&
                {
                    //Handheld.Vibrate();
                    if (laserSplit >= maxSplit)
                    {
                        Debug.Log("Max split reached.");
                    }
                    else
                    {
                        //Debug.Log("Splitting...");
                        laserSplit++;
                        Object go = Instantiate(gameObject, outHit.point, Quaternion.LookRotation(prevDirection));
                        go.name = spawnedBeam;
                        ((GameObject)go).tag = spawnedBeam;
                    }
                }

                //When beam hitting the floor, stop beam and create a mark
                else if (outHit.transform.tag == detectedPlaneTag)
                {
                    goalTimer = 0.0f;

                    GameObject Burnmark = Instantiate(LaserMark, outHit.point, Quaternion.identity);
                    BurnmarkList.Add(Burnmark);
                    while (BurnmarkList.Count > maxBurnmarkCount)
                    {
                        if (BurnmarkList[0] != null)
                            Destroy(BurnmarkList[0].gameObject);
                        BurnmarkList.RemoveAt(0);
                    }
                    loopActive = false;
                }

                //When beam hits the goal, change color, start new coroutine playgoaleffect
                else if (outHit.transform.gameObject.tag == "Goal")
                {
                    goalTimer += Time.deltaTime;
                    if (goalTimer >= 0.3)
                    {

                        colorModifier += Time.deltaTime * 10;
                        Material GoalMat = Goal.GetComponent<MeshRenderer>().sharedMaterial;
                        Color GoalMatEmColor = Goal.GetComponent<MeshRenderer>().sharedMaterial.GetColor("_EmissionColor");
                        GoalMat.SetVector("_EmissionColor", GoalColor * Mathf.LinearToGammaSpace(colorModifier));

                        yield return new WaitForSeconds(0.5f);
                        if (ChangeLevelFlag)
                        {
                            StartCoroutine(PlayGoalEffect());
                            GoalMat.SetVector("_EmissionColor", BaseGoalColor);
                            ChangeLevelFlag = false;
                        }
                        goalTimer = 0.0f;
                    }
                    loopActive = false;
                }

                //When beam hits the second goal, play ending effect
                else if (outHit.transform.gameObject.tag == "Goal2")
                {
                    goalTimer += Time.deltaTime;
                    if (goalTimer >= 0.3)
                    {
                        colorModifier += Time.deltaTime * 10;
                        Material GoalMat = Goal.GetComponent<MeshRenderer>().sharedMaterial;
                        Color GoalMatEmColor = Goal.GetComponent<MeshRenderer>().sharedMaterial.GetColor("_EmissionColor");
                        GoalMat.SetVector("_EmissionColor", GoalColor * Mathf.LinearToGammaSpace(colorModifier));

                        yield return new WaitForSeconds(0.5f);
                        GameObject[] movingObstacles = GameObject.FindGameObjectsWithTag("movingObstacles");
                        for(int i=0; i < movingObstacles.Length; i++)
                        {
                            Rigidbody gameObjectsRigidBody = movingObstacles[i].AddComponent<Rigidbody>();
                            gameObjectsRigidBody.drag = 10;

                            Destroy(movingObstacles[i], 4);
                        }
                        yield return new WaitForSeconds(2.0f);
                        winText.SetActive(true);
                        GameObject.FindGameObjectWithTag("Mirror").SetActive(false);
                        GameObject[] PlayerObstacle = GameObject.FindGameObjectsWithTag("Player");
                        for(int i=0; i<PlayerObstacle.Length; i++)
                        {
                            PlayerObstacle[i].SetActive(false);
                        }
                    }
                    loopActive = false;
                }

                else if (outHit.transform.gameObject.tag == ObstacleTag)
                {
                    goalTimer = 0.0f;
                    loopActive = false;
                }
            }
            else
            {
                goalTimer = 0.0f;
                //Debug.Log("No Bounce");
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

                loopActive = false;
            }
            if (laserReflected > maxBounce)
                loopActive = false;
        }

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PlayGoalEffect()
    {
        levelTwoFlag = true;
        yield return new WaitForSeconds(0.5f);
        instantiatedGoalEffect = Instantiate(GoalEffect, firstGoalTrans, Quaternion.identity);
        yield return new WaitForSeconds(4.0f);
        Panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Panel.SetActive(false);

        //instantiatedObstacle.SetActive(false);
        //instantiatedGoal.SetActive(false);
        //instantiatedGoalEffect.SetActive(false);
        Destroy(instantiatedObstacle);
        Destroy(instantiatedGoal);
        Destroy(instantiatedGoalEffect);

        var secondGoalTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y - 0.5f, FirstPersonCamera.transform.position.z + 4.5f);
        Instantiate(LevelTwoObjects[0], secondGoalTrans, Quaternion.identity);

        //Destroy mirrors again...
        var mirrors = GameObject.FindGameObjectsWithTag("Mirror");//!GameObject.FindGameObjectsWithName("PlayerMirror");
        foreach (GameObject o in mirrors)
        {
            if (o.name != "PlayerMirror")
            {
                Debug.Log("Destroying");
                Destroy(o);
            }
        }

        ChangeLevelFlag = false;

        //DestroyMirrors.ResetBtn();

       
    }
}
