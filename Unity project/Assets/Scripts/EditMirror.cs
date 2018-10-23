using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMirror : MonoBehaviour
{
    [SerializeField] private int RaycastDist = 10;
    
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
            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + RaycastDist + ")");
            //if (Physics.Raycast(lastLaserPosition, laserDirection, out outHit, RaycastDist))// && ((hit.transform.gameObject.tag == detectedPlaneTag) || (hit.transform.gameObject.tag == splitTag) || (hit.transform.gameObject.tag == mirrorTag))) // || (hit.transform.gameObject.tag == ObstacleTag)))
            if(Physics.Raycast(ForwardRay, out outHit, RaycastDist))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * outHit.distance, Color.red);

                if (outHit.transform.tag == "Mirror")
                {
                    Touch touch;

                    if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase != TouchPhase.Ended)
                    {
                        touchFlag = true;
                    }

                    if (Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(0).phase != TouchPhase.Canceled)
                    {
                        touchFlag = true;
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                    {
                        touchFlag = true;
                    }

                    if (touchFlag)
                    {
                        outHit.transform.gameObject.GetComponent<Outline>().enabled = true;
                        loopActive = false;
                    }
                }
                else
                {
                    outHit.transform.gameObject.GetComponent<Outline>().enabled = false;
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
