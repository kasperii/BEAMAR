﻿//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    //using static Vibration;


#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class ARController : MonoBehaviour
    {

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject MirrorPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject MirrorPointPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        public GameObject StartBtn;

        public AudioSource placeMirrorSound;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        //private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();
        public List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public GameObject LightBeam;

        public GameObject Goal;

        public GameObject bigObstacle;

        private float transformOffset = 0.5f;

        private float doubleTapTimer;
        private int tapCount;

        private bool StartBtnFlag = true;

        //public bool StartFlag { get; private set; }

        //public Vibration vibration;

        //public bool StartFlag;

        public void Start()
        {
            GameObject ARSurfObj = GameObject.Find("ARSurfaceManager");
            ARSurfaceManager surfScript = ARSurfObj.GetComponent<ARSurfaceManager>();
            bool StartFlag = surfScript.StartFlag;
            /*
             * var cameraTrans = FirstPersonCamera.transform;
            //ar mirrorObject = Instantiate(prefab, cameraTrans.position, cameraTrans.rotation);// hit.Pose.rotation);

            var randomVector = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 1.0f), Random.Range(-2.0f, 2.0f));

            var laserBeamTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y, FirstPersonCamera.transform.position.z);
            Instantiate(LightBeam, laserBeamTrans, cameraTrans.transform.rotation);

            Instantiate(Goal, randomVector, cameraTrans.transform.rotation);
            */
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            //Debug.Log("flag: " + StartFlag);
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }

            var cameraTrans = FirstPersonCamera.transform;

            //THIS IS SHIT PLEASE CLOSE YOUR EYES
            /*GameObject ARSurfObj = GameObject.Find("ARSurfaceManager");     //Find object ARSurfaceManager
            ARSurfaceManager surfScript = ARSurfObj.GetComponent<ARSurfaceManager>();   //Get script from manager
            bool StartFlag = surfScript.StartFlag;                          // Fetch bool from script from manager
            //Only show laser when we found a plane
            if (showSearchingUI == false && StartFlag == true)              //StartFlag true when Startbutton is pressed
            {
                if (!GameObject.FindGameObjectWithTag("Obstacle"))// == null)
                {
                    //var randomVector = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 1.0f), Random.Range(-2.0f, 2.0f));
                    //Instantiate(Goal, randomVector, Quaternion.identity);
                    var firstGoalTrans = new Vector3(FirstPersonCamera.transform.position.x + 0, FirstPersonCamera.transform.position.y + 0.25f, FirstPersonCamera.transform.position.z + 3f);
                    Instantiate(Goal, firstGoalTrans, Quaternion.identity);


                    //Get position from laser script here
                    var laserBeamTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y, FirstPersonCamera.transform.position.z);
                    Instantiate(LightBeam, laserBeamTrans, Quaternion.identity);

                    var bigObstacleTrans = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y, FirstPersonCamera.transform.position.z + 2);
                    Instantiate(bigObstacle, bigObstacleTrans, Quaternion.identity);
                }
            }*/

            SearchingForPlaneUI.SetActive(showSearchingUI);
            /*if(StartBtn.activeSelf && StartBtnFlag)
            {
                Handheld.Vibrate();
                StartBtn.SetActive(!showSearchingUI);
                StartBtnFlag = false;
            }*/
            


            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            /*if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }*/
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tapCount++;
            }
            if (tapCount > 0)
            {
                doubleTapTimer += Time.deltaTime;
            }
            if (tapCount >= 2)
            {
                if (GameObject.FindGameObjectsWithTag("Mirror").Length < 6)
                {
                    placeMirrorSound.Play();
                    Handheld.Vibrate();
                    //Vibration.CreateOneShot(50);
                    //vibration.CreateOneShot(50);
                    //transform.position + transform.forward*distance
                    //Handheld.Vibrate();

                    //Let's the user place the mirror prefab anywhere on the screen, at the camera position and rotation
                    //var cameraTrans = FirstPersonCamera.transform;
                    var mirrorObject = Instantiate(MirrorPointPrefab, cameraTrans.position + FirstPersonCamera.transform.forward * transformOffset, cameraTrans.rotation);// hit.Pose.rotation);

                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    mirrorObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    //var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make Andy model a child of the anchor.
                    //mirrorObject.transform.parent = anchor.transform;
                    //mirrorObject.transform.parent = anchor.transform;
                }

               doubleTapTimer = 0.0f;
               tapCount = 0;
             
            }
            if (doubleTapTimer > 0.3f)
            {
                doubleTapTimer = 0f;
                tapCount = 0;
            }

            // Raycast against the location the player touched to search for planes.

            /*TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Choose the Andy model for the Trackable that got hit.
                    GameObject prefab;
                    if (hit.Trackable is FeaturePoint)
                    {
                        prefab = MirrorPointPrefab;
                    }
                    else
                    {
                        prefab = MirrorPlanePrefab;
                    }
                    */
            // Instantiate Andy model at the hit pose.
            //var mirrorObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

           /* if (GameObject.FindGameObjectsWithTag("Mirror").Length < 6)
            {
                placeMirrorSound.Play();
                Handheld.Vibrate();
                //Vibration.CreateOneShot(50);
                //vibration.CreateOneShot(50);
            //transform.position + transform.forward*distance
            //Handheld.Vibrate();

            //Let's the user place the mirror prefab anywhere on the screen, at the camera position and rotation
            //var cameraTrans = FirstPersonCamera.transform;
            var mirrorObject = Instantiate(MirrorPointPrefab, cameraTrans.position + FirstPersonCamera.transform.forward * transformOffset, cameraTrans.rotation);// hit.Pose.rotation);

                // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                mirrorObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                // world evolves.
                //var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make Andy model a child of the anchor.
                //mirrorObject.transform.parent = anchor.transform;
                //mirrorObject.transform.parent = anchor.transform;
            }*/



            //     }
            // }
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
