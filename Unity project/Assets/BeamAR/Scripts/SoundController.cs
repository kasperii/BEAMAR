using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	private Camera FirstPersonCamera;
	public LineRenderer Line1;
	public GameObject Cube2;

	int lastPositionCount;
	private bool firstTimeFlag = true;
	private Vector3[] LineIndex;
	private Vector3[] LineIndexOld;

	// Use this for initialization
	void Start () {
		lastPositionCount = Line1.positionCount;
		//FirstPersonCamera = gameObject.GetComponent<Camera>();
		FirstPersonCamera = GameObject.Find("First Person Camera").GetComponent<Camera>();
	}
	// Update is called once per frame
	void Update () {
        //Only update array when the positionCount of line renderer change
        if (Line1.positionCount != lastPositionCount || firstTimeFlag || LineIndex != LineIndexOld) // || LineIndex != LineIndexOld doesn't do the thing I want it to do
        {
            doTheCode();
        }

    }

    public void doTheCode()
        {
            
                //Handheld.Vibrate();
                //Debug.Log("index: " + Line1.positionCount + " last index: " + lastPositionCount);
                LineIndex = new Vector3[Line1.positionCount]; //size of array depending on amount of indexes in line renderer
                if (firstTimeFlag) //First time - creat a empty LineIndexOld that can be used later for comparison
                {
                    LineIndexOld = new Vector3[Line1.positionCount];
                }
                for (var i = 0; i < Line1.positionCount; i++) //Loop through every index and put x,y,z coordinates in array
                {
                    //Debug.Log("i: " + i);
                    LineIndex[i] = Line1.GetPosition(i);
                    //Debug.Log("X: " + LineIndex[i].x + " Y: " + LineIndex[i].y + " Z: " + LineIndex[i].z);
                }
                lastPositionCount = Line1.positionCount;
                firstTimeFlag = false;

                for (var u = 1; u < Line1.positionCount; u++)
                {
                    if (LineIndex != LineIndexOld)
                    {
                        GameObject[] gos = GameObject.FindGameObjectsWithTag("test_cube");
                        foreach (GameObject go in gos)
                            Destroy(go);
                    }
                    //Debug.Log("u: " + u);
                    //Debug.Log("X: " + LineIndex[u].x + " Y: " + LineIndex[u].y + " Z: " + LineIndex[u].z);
                    //Debug.Log("XOld: " + LineIndexOld[u].x + " YOld: " + LineIndexOld[u].y + " ZOld: " + LineIndexOld[u].z);
                    GameObject SoundSource = Instantiate(Cube2, LineIndex[u], Quaternion.identity) as GameObject;
                    SoundSource.GetComponent<SoundSource>().InstantiateSoundSource(u, LineIndex);
                    LineIndexOld = LineIndex;
            }
        }
}
