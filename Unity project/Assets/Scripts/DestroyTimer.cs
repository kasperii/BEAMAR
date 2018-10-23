using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

    [SerializeField] private float secondsToDestroy = 10.0f;
    // Use this for initialization
    void Start () {
        Destroy(this, secondsToDestroy);
	}

}
