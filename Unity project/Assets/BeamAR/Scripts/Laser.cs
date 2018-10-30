
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserLength; // At what length the laser cuts off
    public float laserYOffset = -0.5f;
    public float laserZOffset = 2f;

    [SerializeField] private Camera FirstPersonCamera;
    [SerializeField] private GameObject LaserObject;
    [SerializeField] private GameObject Goal;
    // [SerializeField] private GameObject GoalLit;
    [SerializeField] private GameObject bigObstacle;
    [SerializeField] private GameObject GoalEffect;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject movingObject;
    [SerializeField] private GameObject winText;


    [SerializeField] private string mirrorTag;
    [SerializeField] private string detectedPlaneTag;
    [SerializeField] private string ObstacleTag;

    [SerializeField] private int maxBurnmarkCount = 50;
    List<GameObject> BurnmarkList = new List<GameObject>();
    public string splitTag;
    public string spawnedBeam;
    public int maxBounce;
    public int maxSplit;
    public int[] laserPoints;
    private float timer = 0;
    private LineRenderer mLineRenderer;
    private bool cloudAnchorSpawned = false;

    internal GameObject instantiatedGoal;
    internal GameObject instantiatedObstacle;
    internal GameObject instantiatedGoalEffect;

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

                Debug.Log(GameObject.FindGameObjectWithTag("Obstacle"));
                if(GameObject.FindGameObjectWithTag("Obstacle") != null)
                    StartCoroutine(RedrawLaser());
            }
            timer += Time.deltaTime;
        }
        else
        {
            mLineRenderer = gameObject.GetComponent<LineRenderer>();
            if (GameObject.FindGameObjectWithTag("Obstacle") != null)
                StartCoroutine(RedrawLaser());
        }
        //instantiateTimer = Time.deltaTime;



        //Instantiate gameworld in Laser Script instead of ARController script
        var cameraTrans = FirstPersonCamera.transform;

        if (!GameObject.FindGameObjectWithTag("Goal"))// == null)
        {
            //var randomVector = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 1.0f), Random.Range(-2.0f, 2.0f));
            //Instantiate(Goal, randomVector, Quaternion.identity);
            // var firstGoalTrans = new Vector3(FirstPersonCamera.transform.position.x + 0, FirstPersonCamera.transform.position.y + 0f, FirstPersonCamera.transform.position.z + 4f);
            // Instantiate(Goal, firstGoalTrans, Quaternion.identity);
            //litGoal = Instantiate(GoalLit, firstGoalTrans, Quaternion.identity);
            //litGoal.SetActive(false);

            this.transform.parent.position = new Vector3(transform.localPosition.x,
                                                        transform.localPosition.y + laserYOffset,
                                                        transform.localPosition.z + laserZOffset);    //Move the laser to the right position
            this.transform.position = new Vector3(transform.localPosition.x,
                                                        transform.localPosition.y + laserYOffset,
                                                        transform.localPosition.z + laserZOffset);
        }


    }

    public void coupleToAnchor(Component anchor)
    {
       LaserObject.transform.parent = anchor.transform;
    }

    //Not used
    /*public float GetDistance(Vector3 rayOrigin, Vector3 rayDir)
    {
        Vector3 point = GameObject.FindGameObjectWithTag("FirstPersonCamera").transform.position;
        float distance = Vector3.Distance(rayOrigin, point);
        float angle = Vector3.Angle(rayDir, point - rayOrigin);
        return (distance * Mathf.Sin(angle * Mathf.Deg2Rad));
    }*/

    IEnumerator RedrawLaser()
    {
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, new Vector3(transform.localPosition.x,
                                                transform.localPosition.y,
                                                laserZOffset));
        RaycastHit outHit;

        while (loopActive)
        {
            if (Physics.Raycast(lastLaserPosition, laserDirection, out outHit, laserLength))// && ((hit.transform.gameObject.tag == detectedPlaneTag) || (hit.transform.gameObject.tag == splitTag) || (hit.transform.gameObject.tag == mirrorTag))) // || (hit.transform.gameObject.tag == ObstacleTag)))
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
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserLength);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserLength));

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

        //instantiatedObstacle.SetActive(false);
        //instantiatedGoal.SetActive(false);
        //instantiatedGoalEffect.SetActive(false);
        Destroy(instantiatedObstacle);
        Destroy(instantiatedGoal);
        Destroy(instantiatedGoalEffect);

        var secondGoalTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y - 0.5f, FirstPersonCamera.transform.position.z - 3f);

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
    }
}
