using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPointSoundScript : MonoBehaviour {

	//public GameObject scriptRunner;

	private Camera FirstPersonCamera;
	public LineRenderer Line1;
	public GameObject Cube2;


	public float LineIndex1;
	int lastPositionCount;
	private bool firstTimeFlag = true;
	private Vector3[] LineIndex;
	private Vector3[] LineIndexOld;

	public float angleToIndex1;
	public float angleToIndex2;

	// Use this for initialization
	void Start () {
		lastPositionCount = Line1.positionCount;
		FirstPersonCamera = gameObject.GetComponent<Camera>();
	}
	// Update is called once per frame
	void Update () {

		//Only update array when the positionCount of line renderer change
		if (Line1.positionCount != lastPositionCount || firstTimeFlag) // || LineIndex != LineIndexOld doesn't do the thing I want it to do
		{
			Debug.Log("index: " + Line1.positionCount + " last index: " + lastPositionCount);
			LineIndex = new Vector3[Line1.positionCount]; //size of array depending on amount of indexes in line renderer
			if (firstTimeFlag)
			{
				LineIndexOld = new Vector3[Line1.positionCount];
			}
			//Loop through every index and put x,y,z coordinates in array
      for(var i = 0; i < Line1.positionCount; i++)
      {
				//Debug.Log("i: " + i);
				LineIndex[i] = Line1.GetPosition(i);
				//Debug.Log("X: " + LineIndex[i].x + " Y: " + LineIndex[i].y + " Z: " + LineIndex[i].z);
      }
			lastPositionCount = Line1.positionCount;
			firstTimeFlag = false;

			for (var u = 1; u < Line1.positionCount; u++)
			{
				//soundScript.InstantiateSoundSource(u, LineIndex);
				//(Object.Instantiate(Cube2) as GameObject).GetComponent<InstantiateSoundSourceScript>().InstantiateSoundSource(u, LineIndex);
				//InstantiateSoundSource(u);
				if (LineIndex!=LineIndexOld)
				{
						GameObject[] gos = GameObject.FindGameObjectsWithTag("test_cube");
						foreach(GameObject go in gos)
     				Destroy(go);
				}
					Debug.Log("u: " + u);
					Debug.Log("X: " + LineIndex[u].x + " Y: " + LineIndex[u].y + " Z: " + LineIndex[u].z);
					Debug.Log("XOld: " + LineIndexOld[u].x + " YOld: " + LineIndexOld[u].y + " ZOld: " + LineIndexOld[u].z);
					GameObject SoundSource = Instantiate(Cube2, LineIndex[u], Quaternion.identity) as GameObject;
					SoundSource.GetComponent<InstantiateSoundSourceScript>().InstantiateSoundSource(u, LineIndex);
					LineIndexOld = LineIndex;

			}

		}

	}
/*
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
	}*/

}
