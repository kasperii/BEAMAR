using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateSoundSourceScript : MonoBehaviour {

	private Camera FirstPersonCamera;
	private Transform cube2Trans;
	//public LineRenderer Line1;

	private Vector3[] LineIndex = null;
	int u = 0;

	public float angleToIndex1;
	public float angleToIndex2;

	//private bool InstantiateFirstTime = true;
	void Awake() {
		 FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<Camera>();
		 cube2Trans = gameObject.GetComponent<Transform>();
	}

	// Use this for initialization
	void Start () {
		// FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<Camera>();
		//Cube2 = GameObject.GetComponent<Transform>();
	}
	// Update is called once per frame
	void Update () {

		// FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<Camera>();
		//Debug.Log("Uu: " + u);
		//Debug.Log("Xx: " + LineIndex[u-1].x + " Yy: " + LineIndex[u-1].y + " Zz: " + LineIndex[u-1].z);
		Vector3 VectorIndex1 = FirstPersonCamera.transform.position - LineIndex[u-1];
		Vector3 VectorIndex2 = FirstPersonCamera.transform.position - LineIndex[u];
		Vector3 VectorIndextoIndex = LineIndex[u-1] - LineIndex[u]; //noteToSelf: jump over the first index in loop

		Vector3 ProjectonLine =	Vector3.Project(VectorIndex2,VectorIndextoIndex);
		ProjectonLine = ProjectonLine+LineIndex[u];

		angleToIndex1 = Vector3.Angle(VectorIndex1, VectorIndextoIndex);
		angleToIndex2 = Vector3.Angle(VectorIndex2, VectorIndextoIndex);

		if (angleToIndex1>90 && angleToIndex2<90){

					cube2Trans.position = ProjectonLine;
		}

	}

	public void InstantiateSoundSource(int uIn, Vector3[] LineIndexIn)
	{
		LineIndex = LineIndexIn;
		u = uIn;
	}
}
