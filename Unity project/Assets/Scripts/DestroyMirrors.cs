using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMirrors : MonoBehaviour {

    [SerializeField] private string MirrorTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void ResetBtn () {
        var mirrors = GameObject.FindGameObjectsWithTag(MirrorTag);//!GameObject.FindGameObjectsWithName("PlayerMirror");
        foreach (GameObject o in mirrors)
        {
            if (o.name != "PlayerMirror")
            {
                Debug.Log("Destroying");
                Destroy(o);
            }
        }
    }
}
