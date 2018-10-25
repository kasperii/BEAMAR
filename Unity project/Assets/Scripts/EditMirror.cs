﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * EditMirror sends a raycast from the GameObject Crosshair, if the outHit of the ray hits an object
 * with tag "Mirror", Activate an outline of the mirror, and make it a child of the camera object.
 * This results in the mirror moving and rotating with the camera. 
 * */
public class EditMirror : MonoBehaviour
{
    [SerializeField] private int RaycastDist = 10;      //How far the raycast is drawn  
    [SerializeField] private GameObject playerCamera;   //defined to use as parent of edited mirror
    private GameObject hitMirror;                       //Mirror that is edited, define to save in a variable

    //private GameObject GameObjectWithOutlineScript;
    public Outline outline;
    private bool touchFlag = false;                     // If the player is touching the screen

    void Update()
    {
        bool loopActive = true;                        // Is the raycast loop active
        Ray ForwardRay = new Ray(transform.position, Vector3.forward); //this.transform.forward
       
        RaycastHit outHit;

        while (loopActive)
        {
            if(Physics.Raycast(ForwardRay, out outHit, RaycastDist))
            {
                if (outHit.transform.tag == "Mirror")           // Only edit mirror objects
                {
                    hitMirror = outHit.transform.gameObject;    // Save the raycasted object to variable

                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Began)        // If touching
                        {
                            touchFlag = true;
                        }
                        else if (touch.phase == TouchPhase.Ended)   // If not touching
                        {
                            touchFlag = false;
                        }
                    }

                    if (touchFlag)    // When touching screen, enable outline, vibrate phone, make hitMirror child of camera to move it
                    {
                        Handheld.Vibrate();
                        outHit.collider.gameObject.GetComponent<Outline>().enabled = true;
                        hitMirror.transform.parent = playerCamera.transform;
                        loopActive = false;
                    }

                    else if (!touchFlag)    //Stop moving hitMirror when not touching the screen, remove outline
                    {
                        outHit.collider.gameObject.GetComponent<Outline>().enabled = false;
                        hitMirror.transform.parent = null;
                        loopActive = false;   
                    }
                }
                else      //Needed?
                {
                    loopActive = false;
                }
            }
            else {      //If the raycast doesn't hit
                touchFlag = false;
                loopActive = false;
            }
        }
    }
}
