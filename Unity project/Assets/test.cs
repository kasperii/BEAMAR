using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public Outline outline;

    // Use this for initialization
    public void Start()
    {
        GetComponent<Outline>();
        //OutlineTrue();
    }

    // Update is called once per frame
    public void OutlineTrue()
    {
        outline.enabled = true;
        //gameObject.GetComponent<Outline>().enabled = true;

    }
    public void OutlineFalse()
    {
        outline.enabled = false;
    }
}
