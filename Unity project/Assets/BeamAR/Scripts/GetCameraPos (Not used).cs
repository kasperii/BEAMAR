using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCameraPos : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
	
	// Update is called once per frame
	void Update () {
        var Pos = mainCamera.transform.position;
        Debug.Log(Pos);

    }
}
