using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPointSoundScript : MonoBehaviour {

	public Camera FirstPersonCamera;

	public GameObject Cube;
	public GameObject Sphere;
	public GameObject Cube2;

	public float DistanceCamCube;
	public float DistanceCamSphere;
	public float DistanceCubeSphere;

	//public float VectorCamCube;
	//public float VectorCubeSphere;

	//public static Vector3 Project;
	public float Projectx;
	public float Projecty;
	public float Projectz;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {


		DistanceCamCube = Vector3.Distance(FirstPersonCamera.transform.position,Cube.transform.position);
		DistanceCamSphere = Vector3.Distance(FirstPersonCamera.transform.position,Sphere.transform.position);
		DistanceCubeSphere = Vector3.Distance(Cube.transform.position,Sphere.transform.position);

		Vector3 VectorCamCube = FirstPersonCamera.transform.position - Cube.transform.position;
		Vector3 VectorCubeSphere = Cube.transform.position - Sphere.transform.position;
		Vector3 Project =	Vector3.Project(VectorCamCube,VectorCubeSphere);
		Project = Project+Cube.transform.position;

		Projectx = Project.x;
		Projecty = Project.y;
		Projectz = Project.z;

		if (!GameObject.FindGameObjectWithTag("test_cube")){
			var test_object = Instantiate(Cube2, Project, Quaternion.identity);
		}
		else {
			//Vector3.MoveTowards()
		}



	}
}
