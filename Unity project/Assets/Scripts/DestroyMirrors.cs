using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Searches the hierarchy for objects with MirrorTag, ergo "Mirror". If the name is not PlayerMirror, remove it.  
 **/
public class DestroyMirrors : MonoBehaviour {

    [SerializeField] private string MirrorTag;
	
	// Update is called once per frame
	public void ResetBtn () {
        var mirrors = GameObject.FindGameObjectsWithTag(MirrorTag);//!GameObject.FindGameObjectsWithName("PlayerMirror");
        foreach (GameObject o in mirrors)
        {
            if (o.name != "PlayerMirror")       //Check is not used anymore, but for reference or if changes are needed, the if statment stays
            {
                Destroy(o);
            }
        }
    }
}
