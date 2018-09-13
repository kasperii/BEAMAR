using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Proximity : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //public Vector3 beamX = GameObject.FindWithTag("spawnedBeam").transform.position;

        var cameraPos = GameObject.FindWithTag("FirstPersonCamera").transform.position;

        Debug.Log(cameraPos);


    }
}
