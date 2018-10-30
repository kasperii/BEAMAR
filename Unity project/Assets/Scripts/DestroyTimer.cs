using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

    /**
     *  Really simple script, put on the burn mark prefab. It makes it remove the burn mark after secondsToDestroy seconds. 
     *  Can be used as alternative to the currently used script that removes old burn marks after the max amount
     *  of 50 is met. 
     **/
    [SerializeField] private float secondsToDestroy = 10.0f;
    // Use this for initialization
    void Start () {
        Destroy(this, secondsToDestroy);
	}

}
