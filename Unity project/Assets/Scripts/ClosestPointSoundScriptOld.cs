using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPointSoundScriptOld : MonoBehaviour {

	public Camera FirstPersonCamera;

	public GameObject Cube;
	public GameObject Sphere;
	public GameObject Cube2;

	public LineRenderer Line1;
	//public Vector3 GetPosition(arrayIndex);

	/*public float DistanceCamCube;
	public float DistanceCamSphere;
	public float DistanceCubeSphere;*/

	//public float VectorCamCube;
	//public float VectorCubeSphere;

	//public static Vector3 Project;
	public float Projectx;
	public float Projecty;
	public float Projectz;

	public float angleToSphere;
	public float angleToCube;

	public float angleToIndex1;
	public float angleToIndex2;


	/*
	public float LineRenderPositions;
	public float LineRenderPositionsx;
	public float LineRenderPositionsy;
	public float LineRenderPositionsz;
	*/

	public float LineIndex1;
	int lastPositionCount;
	private bool firstTimeFlag = true;
	private bool hiddenCode = true;

	private Vector3[] LineIndex;
	private Vector3[] pastLineIndex;

	// Use this for initialization
	void Start () {
		//LineIndex.Clear();
		lastPositionCount = Line1.positionCount;

	}

	// Update is called once per frame
	void Update () {

		//Only update array when the positionCount of line renderer change
		if (Line1.positionCount != lastPositionCount || firstTimeFlag)
		{
			Debug.Log("index: " + Line1.positionCount + " last index: " + lastPositionCount);
			LineIndex = new Vector3[Line1.positionCount]; //size of array depending on amount of indexes in line renderer

			//Loop through every index and put x,y,z coordinates in array
      for(var i = 0; i < Line1.positionCount; i++)
      {
				Debug.Log("i: " + i);
				LineIndex[i] = Line1.GetPosition(i);
				Debug.Log("X: " + LineIndex[i].x + " Y: " + LineIndex[i].y + " Z: " + LineIndex[i].z);
      }
			lastPositionCount = Line1.positionCount;
			pastLineIndex = LineIndex;
			firstTimeFlag = false;
		}

		//get values from LineIndex-array

		//Jump between indexes 1-2, 2-3- 3-4 and so on and play
		for (var u = 1; u < Line1.positionCount; u++)
		{
			InstantiateSoundSource(u);
		}

		//
		//Debug.Log(LineIndex);
		//Debug.Log("X: " + LineIndex[1].x + " Y: " + LineIndex[1].y + " Z: " + LineIndex[1].z);
		/*Vector3 VectorIndex1 = FirstPersonCamera.transform.position - LineIndex[1];
		Vector3 VectorIndex2 = FirstPersonCamera.transform.position - LineIndex[2];
		Vector3 VectorIndextoIndex = LineIndex[1] - LineIndex[2]; //noteToSelf: jump over the first index in loop

		Vector3 ProjectonLine =	Vector3.Project(VectorIndex2,VectorIndextoIndex);
		ProjectonLine = ProjectonLine+LineIndex[2];

		angleToIndex1 = Vector3.Angle(VectorIndex1, VectorIndextoIndex);
		angleToIndex2 = Vector3.Angle(VectorIndex2, VectorIndextoIndex);

		if (angleToIndex1>90 && angleToIndex2<90){
			if (!GameObject.FindGameObjectWithTag("test_cube")){
				var test_object = Instantiate(Cube2, ProjectonLine, Quaternion.identity);
			}
			else {
					GameObject.FindGameObjectWithTag("test_cube").transform.position = ProjectonLine;
				//Destroy(GameObject.FindGameObjectWithTag("test_cube"));
				//var test_object = Instantiate(Cube2, Project, Quaternion.identity);
			}
		}*/

		/*	DistanceCamCube = Vector3.Distance(FirstPersonCamera.transform.position,Cube.transform.position);
			DistanceCamSphere = Vector3.Distance(FirstPersonCamera.transform.position,Sphere.transform.position);
			DistanceCubeSphere = Vector3.Distance(Cube.transform.position,Sphere.transform.position);*/

		/*	LineRenderPositions = Line1.GetPosition;
			LineRenderPositionsx = LineRenderPositions.x;
			LineRenderPositionsy = LineRenderPositions.y;
			LineRenderPositionsz = LineRenderPositions.z; */
/*
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

*/
	}

	private void InstantiateSoundSource(int u)
	{
		Vector3 VectorIndex1 = FirstPersonCamera.transform.position - LineIndex[u-1];
		Vector3 VectorIndex2 = FirstPersonCamera.transform.position - LineIndex[u];
		Vector3 VectorIndextoIndex = LineIndex[u-1] - LineIndex[u]; //noteToSelf: jump over the first index in loop

		Vector3 ProjectonLine =	Vector3.Project(VectorIndex2,VectorIndextoIndex);
		ProjectonLine = ProjectonLine+LineIndex[u];

		angleToIndex1 = Vector3.Angle(VectorIndex1, VectorIndextoIndex);
		angleToIndex2 = Vector3.Angle(VectorIndex2, VectorIndextoIndex);

		if (angleToIndex1>90 && angleToIndex2<90){
			if (!GameObject.FindGameObjectWithTag("test_cube")){
				var test_object = Instantiate(Cube2, ProjectonLine, Quaternion.identity);
			}
			else {
					GameObject.FindGameObjectWithTag("test_cube").transform.position = ProjectonLine;
				//Destroy(GameObject.FindGameObjectWithTag("test_cube"));
				//var test_object = Instantiate(Cube2, Project, Quaternion.identity);
			}
		}
	}

}
