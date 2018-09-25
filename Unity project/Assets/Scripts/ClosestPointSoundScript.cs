using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPointSoundScript : MonoBehaviour {

	public Camera FirstPersonCamera;

	public GameObject Cube;
	public GameObject Sphere;
	public GameObject Cube2;

	public LineRenderer Line1;

	public float DistanceCamCube;
	public float DistanceCamSphere;
	public float DistanceCubeSphere;

	//public float VectorCamCube;
	//public float VectorCubeSphere;

	//public static Vector3 Project;
	public float Projectx;
	public float Projecty;
	public float Projectz;

	public float angleToSphere;
	public float angleToCube;

	public float LineRenderPositions;
	public float LineRenderPositionsx;
	public float LineRenderPositionsy;
	public float LineRenderPositionsz;

	// Use this for initialization
	void Start () {
		Line1 = gameObject.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update () {

	/*	DistanceCamCube = Vector3.Distance(FirstPersonCamera.transform.position,Cube.transform.position);
		DistanceCamSphere = Vector3.Distance(FirstPersonCamera.transform.position,Sphere.transform.position);
		DistanceCubeSphere = Vector3.Distance(Cube.transform.position,Sphere.transform.position);*/

	/*	LineRenderPositions = Line1.GetPosition;
		LineRenderPositionsx = LineRenderPositions.x;
		LineRenderPositionsy = LineRenderPositions.y;
		LineRenderPositionsz = LineRenderPositions.z; */

		Vector3 VectorCamSphere = FirstPersonCamera.transform.position - Sphere.transform.position;
		Vector3 VectorCamCube = FirstPersonCamera.transform.position - Cube.transform.position;
		Vector3 VectorCubeSphere = Sphere.transform.position - Cube.transform.position;

		Vector3 Project =	Vector3.Project(VectorCamCube,VectorCubeSphere);
		Project = Project+Cube.transform.position;

		Projectx = Project.x;
		Projecty = Project.y;
		Projectz = Project.z;

		angleToSphere = Vector3.Angle(VectorCamSphere, VectorCubeSphere);
		angleToCube = Vector3.Angle(VectorCamCube, VectorCubeSphere);

		if (angleToSphere>90 && angleToCube<90){
			if (!GameObject.FindGameObjectWithTag("test_cube")){
				var test_object = Instantiate(Cube2, Project, Quaternion.identity);
			}
			else {
					GameObject.FindGameObjectWithTag("test_cube").transform.position = Project;
				//Destroy(GameObject.FindGameObjectWithTag("test_cube"));
				//var test_object = Instantiate(Cube2, Project, Quaternion.identity);
			}
		}


	}
}
