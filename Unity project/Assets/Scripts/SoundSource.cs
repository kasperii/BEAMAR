using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{

    private Camera FirstPersonCamera;
    private Transform cylinder2Trans;

    private Vector3[] LineIndex = null;
    int u = 0;

	// Use this for initialization
	void Start()
    {
        FirstPersonCamera = GameObject.Find("FirstPersonCamera").GetComponent<Camera>();
        cylinder2Trans = gameObject.GetComponent<Transform>();
    }
    void Update()
    {


        Vector3 VectorIndextoIndex = LineIndex[u - 1] - LineIndex[u]; //noteToSelf: jump over the first index in loop
        float distanceVectors = VectorIndextoIndex.magnitude;


        transform.localScale = new Vector3(0.1f, 0.1f, distanceVectors);
        transform.position = LineIndex[u] + (VectorIndextoIndex/2f);
        transform.LookAt(LineIndex[u - 1]);
        //transform.LookAt(VectorIndextoIndex);
        //transform.Rotate(9f, 9f, 9f, Space.Self);


    }

    public void InstantiateSoundSource(int uIn, Vector3[] LineIndexIn)
    {
        LineIndex = LineIndexIn;
        u = uIn;
    }

}