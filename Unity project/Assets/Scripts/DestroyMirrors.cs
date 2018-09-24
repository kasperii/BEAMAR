﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMirrors : MonoBehaviour {

    [SerializeField] private string MirrorTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void ResetBtn () { 
        var mirrors = GameObject.FindGameObjectsWithTag(MirrorTag);
        foreach (GameObject o in mirrors)
        {
            Destroy(o);
        }
    }
}
