using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Rotates the obstacle from the second level around its own axis as well as around the goal. 
 * It also lerps the color of the gem shader on the ICO Sphere from white to red. 
 * */
public class RotateObject : MonoBehaviour {

    [SerializeField] [Range(0.0f, 40.0f)] private float speedMultiplier = 40.0f;
    [SerializeField] private GameObject ICOSphere;
    [SerializeField] private GameObject Sphere;
    [SerializeField] private MeshRenderer ICOSphereRend;
    [SerializeField] private GameObject Goal;

    private float time;

    Color lerpedColor = Color.white;
    

    private void Start()
    {
        //ICOSphereRend = GetComponent<MeshRenderer>();
    }


    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;

        lerpedColor = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));     //Loops the color from white to red

        ICOSphereRend.material.SetColor("_Color", lerpedColor);                             //Change the color of the material 

        if(transform.parent != null && transform.parent.tag == "Row1")                      //Rotate the top row clockwise and the bottom row anti-clockwise
            transform.RotateAround(Goal.transform.position, Goal.transform.up, 40 * Time.deltaTime);

        else
            transform.RotateAround(Goal.transform.position, Goal.transform.up, -40 * Time.deltaTime);


        //Handles rotation of the obstacle, it changes rotation every 5 seconds.
        if (time < 5.0f)
        {
            ICOSphere.transform.Rotate(Vector3.right * Time.deltaTime * speedMultiplier);
        }
        else if (time >= 5.0f && time < 10.0f)
        {
           ICOSphere.transform.Rotate(Vector3.up * Time.deltaTime * speedMultiplier);
        }
        else if (time >= 10.0f && time < 15.0f)
        {
            ICOSphere.transform.Rotate(Vector3.left * Time.deltaTime * speedMultiplier);
        }
        else if (time >= 15.0f && time < 20.0f)
        {
            ICOSphere.transform.Rotate(Vector3.down * Time.deltaTime * speedMultiplier);
        }
        else if(time >= 20.0f && time < 25.0f)
        {
            ICOSphere.transform.Rotate(Vector3.back * Time.deltaTime * speedMultiplier);
        }

        else if (time >= 25.0f)
        {
            time = 0.0f;
        }

        Sphere.transform.Rotate(Vector3.down * Time.deltaTime * speedMultiplier);
        
    }
}
