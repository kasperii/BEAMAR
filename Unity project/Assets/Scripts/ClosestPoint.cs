using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPoint : MonoBehaviour {
    public Vector3 location;
		public AudioClip clip; //make sure you assign an actual clip here in the inspector

    public void OnDrawGizmos()
    {
        var collider = GetComponent<Collider>();

        if (!collider)
        {
            return; // nothing to do without a collider
        }
				var cameraPos = GameObject.FindWithTag("FirstPersonCamera").transform.position;
        Vector3 closestPoint = collider.ClosestPoint(cameraPos);

        Gizmos.DrawSphere(cameraPos, 0.1f);
        Gizmos.DrawWireSphere(closestPoint, 0.1f);
				AudioSource.PlayClipAtPoint(clip, closestPoint);
    }
}
/*
public class Closest : MonoBehaviour {


public static Vector3 ClosestPoint(Vector3 cameraPos, Collider collider, Vector3 position, Quaternion rotation);


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		  var cameraPos = GameObject.FindWithTag("FirstPersonCamera").transform.position;

	}
}*/
