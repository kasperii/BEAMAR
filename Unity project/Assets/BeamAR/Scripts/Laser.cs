
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
    public int laserDistance;
    [SerializeField] private Camera FirstPersonCamera;
    [SerializeField] private GameObject LightBeam;
    [SerializeField] private GameObject Goal;
    [SerializeField] private GameObject GoalLit;
    [SerializeField] private GameObject bigObstacle;


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


    //public AudioSource goalSound;

    private string GoalName = "Goal";
    GameObject litGoal; //Goal with light to be instantiated

    public GameObject WinText;
    //public ParticleSystem WinParticleSystem;
    [SerializeField] private GameObject LaserMark;

    private float goalTimer = 0.0f; //Start timer when raycast hits goal

    public Color GoalColor = Color.yellow;
    private float colorModifier = -5f;

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

            var laserBeamTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y + 0.0f, FirstPersonCamera.transform.position.z + 0.0f);
            //Instantiate(LightBeam, laserBeamTrans, Quaternion.identity);

            //Sets the laser active
            //this.transform.parent.gameObject.SetActive(true);
            //this.gameObject.SetActive(true);
            this.transform.parent.position = laserBeamTrans;    //Move the laser to the right position


            // var bigObstacleTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y, FirstPersonCamera.transform.position.z + 3);
            // Instantiate(bigObstacle, bigObstacleTrans, Quaternion.identity);
        }


    }

    public void coupleToAnchor(Component anchor)
    {
       this.transform.parent.parent = anchor.transform;
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
                    Debug.Log("Goal");
                    goalTimer += Time.deltaTime;
                    if (goalTimer >= 0.3)
                    {

                        colorModifier += Time.deltaTime * 10;
                        Material GoalMat = Goal.GetComponent<MeshRenderer>().sharedMaterial;
                        Color GoalMatEmColor = Goal.GetComponent<MeshRenderer>().sharedMaterial.GetColor("_EmissionColor");
                        GoalMat.SetVector("_EmissionColor", GoalColor * Mathf.LinearToGammaSpace(colorModifier));

                        /*outHit.transform.gameObject.SetActive(false);
                        litGoal.SetActive(true);*/
                        goalTimer = 0.0f;
                    }
                   loopActive = false;
                }

                else if (outHit.transform.gameObject.tag == ObstacleTag)
                {
                    //Debug.Log("Obstacle");
                    goalTimer = 0.0f;
                    //Change color on goal when raycast hits
                    /*if (outHit.collider.GetComponent<ChangeColorOnGoal>() != null)
                    {
                        //goalSound.Play();
                        //WinText.GetComponent<WinTextScript>().UpdateText();
                        //updateWinText.UpdateText();
                        //WinText.gameObject.SetActive(true);  Instantiate(bigObstacle, bigObstacleTrans, Quaternion.identity);
                        //Instantiate(WinText, FirstPersonCamera.transform.position, Quaternion.identity);
                        outHit.collider.GetComponent<ChangeColorOnGoal>().materialChange(outHit.collider.GetComponent<Renderer>());
                    }
                    else
                    {
                        Debug.Log("Need to attach a script ChangeColorOnGoal to object");
                    }*/
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
}
