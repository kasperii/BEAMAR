using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    [SerializeField] private string mirrorTag;
    [SerializeField] private string detectedPlaneTag;
    [SerializeField] private string ObstacleTag;

    //[SerializeField] private GameObject Goal;

    public string splitTag;
    public string spawnedBeam;
    public int maxBounce;
    public int maxSplit;
    public int[] laserPoints;
    private float timer = 0;
    private LineRenderer mLineRenderer;

    public AudioSource goalSound;

    private string GoalName = "Goal";

    // Use this for initialization
    void Start()
    {
        goalSound = GetComponent<AudioSource>();

        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
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

                StartCoroutine(RedrawLaser());
            }
            timer += Time.deltaTime;
        }
        else
        {
            mLineRenderer = gameObject.GetComponent<LineRenderer>();
            StartCoroutine(RedrawLaser());
        }
    }

    public float GetDistance(Vector3 rayOrigin, Vector3 rayDir)
    {
        Vector3 point = GameObject.FindGameObjectWithTag("FirstPersonCamera").transform.position;
        float distance = Vector3.Distance(rayOrigin, point);
        float angle = Vector3.Angle(rayDir, point - rayOrigin);
        return (distance * Mathf.Sin(angle * Mathf.Deg2Rad));
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
        RaycastHit hit;

        while (loopActive)
        {
            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance))// && ((hit.transform.gameObject.tag == detectedPlaneTag) || (hit.transform.gameObject.tag == splitTag) || (hit.transform.gameObject.tag == mirrorTag))) // || (hit.transform.gameObject.tag == ObstacleTag)))
            {
                //Debug.Log("Bounce");
                //If the tag is a mirror, bounce it. 
                if ((hit.transform.gameObject.tag == mirrorTag) || (hit.transform.gameObject.tag == splitTag)) //(hit.transform.gameObject != Goal) &&
                {
                    laserReflected++;
                    vertexCounter += 3;
                    mLineRenderer.SetVertexCount(vertexCounter);
                    mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                    mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                    mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                    mLineRenderer.SetWidth(.01f, .01f);
                    lastLaserPosition = hit.point;

                    Vector3 prevDirection = laserDirection;
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);

                    //When using prisms, we also want to split the beam. 
                    if (hit.transform.gameObject.tag == splitTag)
                    {
                        //Debug.Log("Split");
                        if (laserSplit >= maxSplit)
                        {
                            Debug.Log("Max split reached.");
                        }
                        else
                        {
                            //Debug.Log("Splitting...");
                            laserSplit++;
                            Object go = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
                            go.name = spawnedBeam;
                            ((GameObject)go).tag = spawnedBeam;
                        }
                    }
                }

                //When hitting the goal
                else if (hit.transform.gameObject.tag == ObstacleTag)
                {
                    laserReflected = maxBounce + 1;
                    vertexCounter += 3;
                    mLineRenderer.SetVertexCount(vertexCounter);
                    mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                    mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                    mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                    mLineRenderer.SetWidth(.01f, .01f);
                    lastLaserPosition = hit.point;

                    Vector3 prevDirection = laserDirection;
                    laserDirection = Vector3.Reflect(laserDirection, hit.normal);

                    //goalSound.Play();
                    Handheld.Vibrate();
                    if (hit.collider.GetComponent<ChangeColorOnGoal>() != null)
                    {
                        hit.collider.GetComponent<ChangeColorOnGoal>().materialChange(hit.collider.GetComponent<Renderer>());
                    }
                    else
                    {
                        Debug.Log("Need to attach a script ChangeColorOnGoal to object");
                    }
                    loopActive = false;
                }

                //When hitting a plane, do stuff
                else if (hit.transform.gameObject.tag == detectedPlaneTag)
                {
                    laserReflected = maxBounce + 1; //Ugly code, don't use 
                    //TODO Burn hole animation
                    loopActive = false;
                }
              
                else
                {
                    Handheld.Vibrate();
                    print("Do nothing I guess");
                    loopActive = false;
                }

            }
            else
            {
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