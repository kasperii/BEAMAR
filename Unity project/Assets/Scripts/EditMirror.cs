using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMirror : MonoBehaviour
{
    [SerializeField] private int RaycastDist = 10;
    [SerializeField] private GameObject playerCamera;
    private GameObject hitMirror;

    private GameObject GameObjectWithOutlineScript;
    public Outline outline;

    /* [SerializeField] private Shader Standard;
     [SerializeField] private Shader Outline;
     [SerializeField] private Renderer rend;
     */

    private bool touchFlag = false;
    void Start()
    {
       
        // rend = GetComponent<Renderer>();
        //Standard = Shader.Find("Standard");
        //Outline = Shader.Find("Custom/Outline");



    }

    // Update is called once per frame
    void Update()
    {

        bool loopActive = true; //Is the reflecting loop active?

        //Vector3 laserDirection = transform.forward; //direction of the next laser
        //Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser
        Ray ForwardRay = new Ray(transform.position, Vector3.forward); //this.transform.forward
       

        RaycastHit outHit;

        while (loopActive)
        {
            if(Physics.Raycast(ForwardRay, out outHit, RaycastDist))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * outHit.distance, Color.red);

                if (outHit.transform.tag == "Mirror")
                {
                    hitMirror = outHit.transform.gameObject;

                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Began)
                        {
                            touchFlag = true;
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            touchFlag = false;
                        }
                    }

                    if (touchFlag)
                    {
                        outHit.collider.gameObject.GetComponent<Outline>().enabled = true;
                        Handheld.Vibrate();

                        hitMirror.transform.parent = playerCamera.transform;
                        //touchFlag = false;
                        loopActive = false;
                    }

                    else if (!touchFlag)
                    {
                        outHit.collider.gameObject.GetComponent<Outline>().enabled = false;
                        hitMirror.transform.parent = null;
                        loopActive = false;   
                    }
                }
                else
                {
                    outHit.collider.gameObject.GetComponent<Outline>().enabled = false;
                    //hitMirror.transform.parent = null;
                    loopActive = false;
                }
            }
            else
            {
                touchFlag = false;
                loopActive = false;
            }
        }
    }
}
