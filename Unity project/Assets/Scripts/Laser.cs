
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    [SerializeField] private Camera FirstPersonCamera;
    [SerializeField] private readonly GameObject LightBeam; //Remove readonly if not working
    [SerializeField] private GameObject Goal;
    //[SerializeField] private GameObject GoalLit;
    [SerializeField] private GameObject bigObstacle;
    [SerializeField] private GameObject GoalEffect;
    [SerializeField] private Animator TransitionAnim;
    [SerializeField] private string SceneName;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject movingObject;

    [SerializeField] private string mirrorTag;
    [SerializeField] private string detectedPlaneTag;
    [SerializeField] private string ObstacleTag;

    [SerializeField] private int maxBurnmarkCount = 50;

    //[SerializeField] private GameObject[] LevelOneObjects;
    //[SerializeField] private List<GameObject> LevelOneList;
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
    public Color GoalColor = Color.yellow;
    private float colorModifier = -5f;
    internal Vector3 firstGoalTrans;

    DestroyMirrors DestroyMirrors;

    //private float instantiateTimer;


    // Use this for initialization
    void Start()
    {
        //Color baseColor = Color.yellow;
        Material GoalMat = Goal.GetComponent<MeshRenderer>().sharedMaterial;
        Color GoalMatEmColor = Goal.GetComponent<MeshRenderer>().sharedMaterial.GetColor("_EmissionColor");
        GoalMat.SetVector("_EmissionColor", GoalColor * Mathf.LinearToGammaSpace(-5f));

        //goalSound = GetComponent<AudioSource>();
        //PSysObj.GetComponent<ParticleSystem>().Stop();
        //WinParticleSystem.Stop();
        // WinText.gameObject.SetActive(false);
        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        //StartCoroutine(RedrawLaser());
    }

    /*public void Test()
    {
        //'public void OnMouseClick(){ Application.LoadLevel("Here put the name of the scene"); }'
        Debug.Log("test");
        SceneManager.LoadScene(SceneName);
        //StartCoroutine(PlayGoalEffect());
    }*/

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != spawnedBeam)
        {
            if (timer >= updateFrequency)
            {
                timer = 0;
                //Debug.Log("Redrawing laser");
                foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag(spawnedBeam))
                    Destroy(laserSplit);

                //Debug.Log(GameObject.FindGameObjectWithTag("Obstacle"));
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
        //instantiateTimer = Time.deltaTime;


        //Instantiate gameworld in Laser Script instead of ARController script
        //var cameraTrans = FirstPersonCamera.transform;

        //THIS IS SHIT PLEASE CLOSE YOUR EYES
        GameObject ARSurfObj = GameObject.Find("ARSurfaceManager");                 // Find object ARSurfaceManager
        ARSurfaceManager surfScript = ARSurfObj.GetComponent<ARSurfaceManager>();   // Get script from manager
        bool StartFlag = surfScript.StartFlag;                                      // Fetch bool from script from manager
                                                                                    // Only show laser when we found a plane
        if (StartFlag == true)                                                      // StartFlag true when Startbutton is pressed
        {
            if (!GameObject.FindGameObjectWithTag("Goal"))// == null)
            {
                //var randomVector = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 1.0f), Random.Range(-2.0f, 2.0f));
                //Instantiate(Goal, randomVector, Quaternion.identity);
                firstGoalTrans = new Vector3(FirstPersonCamera.transform.position.x + 0, FirstPersonCamera.transform.position.y - 0.5f, FirstPersonCamera.transform.position.z + 4.5f);
                instantiatedGoal = Instantiate(Goal, firstGoalTrans, Quaternion.identity);
                //firstGoal.add

                //litGoal = Instantiate(GoalLit, firstGoalTrans, Quaternion.identity);
                //litGoal.SetActive(false);

                var laserBeamTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y + 0.0f, FirstPersonCamera.transform.position.z + 0.0f);
                //var laserBeamTrans2 = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y + 0.0f, FirstPersonCamera.transform.position.z + 10.0f);
                //Instantiate(LightBeam, laserBeamTrans, Quaternion.identity);

                //Sets the laser active
                //this.transform.parent.gameObject.SetActive(true);
                //this.gameObject.SetActive(true);
                this.transform.parent.position = laserBeamTrans;    //Move the laser to the right position

                var bigObstacleTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y - 0.25f, FirstPersonCamera.transform.position.z + 3);
                instantiatedObstacle = Instantiate(bigObstacle, bigObstacleTrans, Quaternion.identity);

                //mLineRenderer.SetPosition(0, laserBeamTrans);   //Set the start position of linerenderer
                //mLineRenderer.SetPosition(1, laserBeamTrans2);

            }
        }


    }

    IEnumerator RedrawLaser()
    {
        //Debug.Log("Running");
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit outHit;

        while (loopActive)
        {
            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
            if (Physics.Raycast(lastLaserPosition, laserDirection, out outHit, laserDistance))// && ((hit.transform.gameObject.tag == detectedPlaneTag) || (hit.transform.gameObject.tag == splitTag) || (hit.transform.gameObject.tag == mirrorTag))) // || (hit.transform.gameObject.tag == ObstacleTag)))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * outHit.distance, Color.yellow);
                //Instantiate(LaserMark, outHit.point, Quaternion.identity);

                //Handheld.Vibrate();
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
                        //Instantiate(GoalEffect, firstGoalTrans, Quaternion.identity);
                        // yield return new WaitForSeconds(0.5f);
                        //SceneManager.LoadScene(SceneName);
                        if (ChangeLevelFlag)
                        {
                            StartCoroutine(PlayGoalEffect());
                            GoalMat.SetVector("_EmissionColor", BaseGoalColor * Mathf.LinearToGammaSpace(colorModifier));
                            ChangeLevelFlag = false;
                        }
                        goalTimer = 0.0f;
                    }
                    loopActive = false;
                }

                else if (outHit.transform.gameObject.tag == "Goal2")
                {
                    Debug.Log("Goal");
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
                        }
                        //Rigidbody gameObjectsRigidBody = movingObstacles.AddComponent<Rigidbody>();

                    }
                    loopActive = false;
                }

                else if (outHit.transform.gameObject.tag == ObstacleTag)
                {
                    //Debug.Log("Obstacle");
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
        yield return new WaitForSeconds(0.5f);
        instantiatedGoalEffect = Instantiate(GoalEffect, firstGoalTrans, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Panel.SetActive(false);

        instantiatedObstacle.SetActive(false);
        instantiatedGoal.SetActive(false);
        instantiatedGoalEffect.SetActive(false);
        Destroy(instantiatedGoalEffect);

        var secondGoalTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y - 0.5f, FirstPersonCamera.transform.position.z - 3f);
        for (int i = 0; i < LevelTwoObjects.Length; i++)
        {
            Instantiate(LevelTwoObjects[i], secondGoalTrans, Quaternion.identity);
        }

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
